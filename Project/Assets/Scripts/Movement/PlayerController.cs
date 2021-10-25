using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ProBuilder2.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float speedBuff;
	public float damageBuff;
	private bool dashing;

	public bool Dashing
	{
		get { return dashing; }
	}
	
	public bool isBot;
	private float dashstart;
	public float dashTime;
	public float dashCooldown;
	public float dashSpeed;
	public float dashCDR = 0;
	private Vector3 input;
	public Vector3 mousPos;
	private PhotonView view;

	public Transform shootingHand;
	public Dictionary<string, int> inventory;
	public int nbGuns = 6; // changed from 5 to 6
	public Item[] guns;
	public int curGunId; // must be between 0 and 4 ( for the moment) (now 0 and 5)
	public Item baseGun;
	public BulletBehavior bullet;
	private GameObject curGunObject; //must be instantiated.
	public bool isFiring = false;
	private float shotCounter;

	private Animator anim;
	private Color color;
	public Renderer PlayerRenderer;
	public Renderer ScarfRenderer;

	public Slider healthBar;
	public Slider armorBar;
	public int maxHealth = 100;
	public int maxArmor = 100;
	public bool isDead;
	public int armor;
	public int health;
	public int armorDamageReduction;
	private Vector3 SpawnPos;

	public Image okHand;

	private CameraEffects cameraEffects;

	public SpawnManager SpawnManager;
	private bool sharedEvents;

	private void Start()
	{
		if (SceneManager.GetActiveScene().name == "Boss Map")
		{
			GetComponent<PhotonView>().enabled = false;
		}
		sharedEvents = false;
		anim = GetComponent<Animator>();
		anim.updateMode = AnimatorUpdateMode.Normal;
		guns = new Item[nbGuns];
		armor = 0;
		health = maxHealth;
		healthBar.value = (float)health/maxHealth;
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Boss Map"))
		{
			armorBar.gameObject.SetActive(true);
		}
		else
		{
			okHand.gameObject.SetActive(false);
			armorBar.gameObject.SetActive(false);
		}
		armorBar.value = (float)armor/maxArmor;
		isDead = false;
		dashing = false;
		dashstart = 0;
		input = Vector3.zero;
		view = GetComponent<PhotonView>();
		inventory = new Dictionary<string, int>();
		if (view.isMine && !isBot)
		{
			InGameUI.Instance.PlayerController = this;
			InGameUI.Instance.InvUpdate();
			if (PhotonNetwork.isMasterClient)
			{
				view.RPC("SetOffset", PhotonTargets.All, InGameUI.Instance.offset);
			}
		}
		if (!view.isMine)
		{
			GetComponent<Rigidbody>().useGravity = false;
		}
		AddGun(baseGun);
		updateGun();
		SpawnPos = transform.position;
		cameraEffects = Camera.main.GetComponent<CameraEffects>();
		if (PhotonNetwork.offlineMode || view.isMine)
		{
			ChooseColor();
		}
	}

	[PunRPC]
	void SetOffset(double time)
	{
		InGameUI.Instance.SetOffset(time);
	}

	[PunRPC]
	void SetEventList(Vector2 event0, Vector2 event1, Vector2 event2, Vector2 event3, Vector2 event4, Vector2 event5, Vector2 event6, Vector2 event7, Vector2 event8, Vector2 event9)
	{
		var list = new List<Vector2>();
		list.Add(event0);
		list.Add(event1);
		list.Add(event2);
		list.Add(event3);
		list.Add(event4);
		list.Add(event5);
		list.Add(event6);
		list.Add(event7);
		list.Add(event8);
		list.Add(event9);
		InGameUI.Instance.SetEventList(list);
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting) {
 
			stream.SendNext(health);
			stream.SendNext(isDead);
			
			var colorV3 = new Vector3(color.r, color.g, color.b);
			stream.SendNext(colorV3);
		}
		else {
			health = (int)stream.ReceiveNext();
			healthBar.value = (float)health / maxHealth;
			
			isDead = (bool)stream.ReceiveNext();
			
			//var dash = (bool)stream.ReceiveNext();
			//if (!dashing && dash)
			//{
			//	anim.SetTrigger("Roll");
			//}
			//
			//anim.SetFloat("FwdSpeed", (float)stream.ReceiveNext());
			//anim.SetFloat("SideSpeed", (float)stream.ReceiveNext());
			//anim.SetBool("HoldsWeapon", (bool)stream.ReceiveNext());
			
			Vector3 colorV3 = (Vector3)stream.ReceiveNext();
			Color color  = new Color(colorV3.x, colorV3.y, colorV3.z);
			SetColor(color);
		}
	}

	public int GetViewID()
	{
		return view.viewID;
	}

	public void ChooseColor()
	{
		color = Random.ColorHSV(0,1,1,1,0.5f,1,1,1);
		PlayerRenderer.material.color = color;
		PlayerRenderer.material.SetColor ("_EmissionColor", color);
		ScarfRenderer.material.color = color;
		ScarfRenderer.material.SetColor ("_EmissionColor", color);
		
	}
	
	public void SetColor(Color color)
	{
		PlayerRenderer.material.color = color;
		PlayerRenderer.material.SetColor ("_EmissionColor", color);
		ScarfRenderer.material.color = color;
		ScarfRenderer.material.SetColor ("_EmissionColor", color);
		
	}

	[PunRPC]
	void RPCSetColor(Color color)
	{
		SetColor(color);
	}

	public Color GetColor()
	{
		return color;
	}

	private void Update()
	{
		if (!isBot && SpawnManager && PhotonNetwork.isMasterClient && !sharedEvents)
		{
			sharedEvents = true;
			
			List<Vector2> list = new List<Vector2>();

			for (int i = 0; i < 10; i++)
			{
				int eventId;
				int spawnId;
				if (i == 0)
				{
					eventId = 13;
					spawnId = 0;
				}
				else
				{
					eventId = Random.Range(0, Global.Instance.EventList.Count);
					spawnId = Random.Range(0, SpawnManager.EventSpawn.SpawnList.Count);
				}
				list.Add(new Vector2(eventId, spawnId));
			}
			Debug.Log("Calling Set Event List");
			view.RPC("SetEventList", PhotonTargets.All, list[0], list[1], list[2], list[3], list[4], list[5], list[6], list[7], list[8], list[9]);
		}
		
		if (isDead)
		{
			isFiring = false;
			return;
		}
		
		if (view.isMine && !isBot && Input.GetButtonDown("Fire"))
		{
			isFiring = true;
		}
		if (view.isMine && !isBot && Input.GetButtonUp("Fire"))
		{
			isFiring = false;
		}
		if (dashing)
		{
			return;
		}
		Shoot();
	}

	public bool IsPlayer(bool countDeath = true)
	{
		if (countDeath)
		{
			return view.isMine && !isDead && !isBot;
		}
		return view.isMine && !isBot;
	}

	void FixedUpdate ()
	{
		if (!view.isMine || isDead)
		{
			return;
		}
		if (!dashing && !isBot)
		{
			input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		}
		if (!dashing && !isBot && view.isMine && Input.GetButton("Drop"))
		{
			DropSort();
		}
		if (!isBot && view.isMine && Input.GetButton("Emote"))
		{
			view.RPC("RPCOkHand", PhotonTargets.All, view.viewID);
		}
		if (!isBot && view.isMine && Input.GetButton("cur1") && (guns[0] != null))
		{
			setGun(0);
		}
		if (!isBot && view.isMine && Input.GetButton("cur2") && (guns[1] != null))
		{
			setGun(1);
		}
		if (!isBot && view.isMine && Input.GetButton("cur3") && (guns[2] != null))
		{
			setGun(2);
		}
		if (!isBot && view.isMine && Input.GetButton("cur4") && (guns[3] != null))
		{
			setGun(3);
		}
		if (!isBot && view.isMine && Input.GetButton("cur5") && (guns[4] != null))
		{
			setGun(4);
		}
		if (!isBot && view.isMine && Input.GetButton("cur6") && (guns[5] != null))
		{
			setGun(5);
		}
		Vector3 direction = input.normalized;
		Vector3 velocity = direction * ((dashing ? dashSpeed : speed) * (100 + speedBuff)/100);
		
		gameObject.GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, gameObject.GetComponent<Rigidbody>().velocity.y, velocity.z) ;

		Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		//refplane, would be like a flat plane just below the feet of our player
		Plane refRayPlane = new Plane(Vector3.up, transform.position);
		float rayLength;

		if (!dashing && !isBot && refRayPlane.Raycast(cameraRay,out rayLength)) //there rayLength is the length of the ray when it hit the refGround
		{
			Vector3 pointToLook = cameraRay.GetPoint(rayLength); // this give us a point coordinate at a certain distance of the ray, here we take the distance when the ray touch the refGround
			Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
			mousPos = new Vector3(pointToLook.x, transform.position.y, pointToLook.z);
			transform.LookAt(mousPos);//we dont want our player looking in a different way on the Y axis
		}

		var dashCD = (100 - dashCDR)*(dashCooldown + dashTime)/100;

		if(!isBot && Input.GetButtonDown("Roll") && !dashing && Time.time >= dashstart + dashCD && direction != Vector3.zero)
		{
			dashstart = Time.time;
			dashing = true;
			
			transform.rotation = Quaternion.LookRotation(direction);
			if (anim.runtimeAnimatorController != null)
			{
				anim.SetBool("Roll", true);
				anim.Play("Roll");
				GetComponent<AudioManagerInstance>().Play("roll");
			}
			
		}
		if (dashing && Time.time >= dashstart + dashTime)
		{
			if(view.isMine && !isBot) Mouse.Instance.Roll(dashCooldown/2);
			dashing = false;
			if (anim.runtimeAnimatorController != null)
			{
				anim.SetBool("Roll", false);
			}
			
		}

		if (!dashing && direction != Vector3.zero)
		{
			Vector2 playerAngle = new Vector2(transform.forward.x, transform.forward.z);
			Vector2 dirAngle = new Vector2(direction.x, direction.z);
			float angleDiff = Vector2.Angle(playerAngle, dirAngle);
			
			//Debug.Log("Player: " + playerAngle + " Direction: " + dirAngle + " AngleDiff: " + angleDiff);
			if (anim.runtimeAnimatorController != null)
			{
				anim.SetFloat("FwdSpeed",  2 * Mathf.Cos(angleDiff * Mathf.Deg2Rad));
				anim.SetFloat("SideSpeed", 2 * Mathf.Sin(angleDiff * Mathf.Deg2Rad));
			}
			
		}
		else if (direction == Vector3.zero)
		{
			if (anim.runtimeAnimatorController != null)
			{
				anim.SetFloat("FwdSpeed",  0);
				anim.SetFloat("SideSpeed", 0);
			}
			
		}
	}

	public void SetInput(Vector3 input)
	{
		this.input = input;
	}

	public void Damage(int hp)
	{
		if (isDead || !view.isMine) return;
		
		if (armor > 0)
		{
			var armorDamage = 100 * armor / (100 - armorDamageReduction);
			armorDamage -= hp;
			if (armorDamage > 0)
			{
				armor =  armorDamage * (100 - armorDamageReduction) /100;
			}
			else if (armorDamage == 0)
			{
				armor = 0;	
			}
			else
			{
				armor = 0;
				health += armorDamage;		
			}
			armorBar.value = (float)armor / maxArmor;
			healthBar.value = (float)health / maxHealth;
		}
		else
		{
			health -= hp;
			healthBar.value = (float)health / maxHealth;
      	}
		if (health <= 0)
		{
			health = 0;
			healthBar.value = (float)health / maxHealth;
			Death();
		}

		if (view.isMine && !isBot)
		{
			cameraEffects.Damage();
		}
	}

	public void Heal(int hp)
	{
		if (view.isMine && !isBot)
		{
			cameraEffects.Heal();
		}
		if (isDead) return;
		
		var maxHP = maxHealth - health;
		health += Math.Min(hp, maxHP);
		healthBar.value = (float)health / maxHealth;
	}

	public void Death(bool drop = true)
	{
		if (isDead) return;
		isDead = true;
		anim.SetBool("Dead", true);
		GetComponent<AudioManagerInstance>().Play("death");
		GetComponent<AudioManagerInstance>().Play("applause");
		anim.SetFloat("FwdSpeed",  0);
		anim.SetFloat("SideSpeed", 0);
		anim.SetBool("HoldsWeapon", false);
		if (SceneManager.GetActiveScene().name == "Boss Map")
		{
			BossTimer.Instance.EndGame(false);
		}
		else
		{
			StartCoroutine(Respawn(drop));
		}
		
	}

	[PunRPC]
	void DropStuff(Vector3 pos, int wood, int iron, int lemon, int chemicals)
	{
		var itemObject = Resources.Load("ItemHolder", typeof(GameObject)) as GameObject;
		GameObject ItemHolder = Instantiate(itemObject, pos, Quaternion.identity);
		ItemHolder.GetComponent<ItemHolder>().SetItem(Global.Instance.ItemList[0]);
		for (int i = 0; i < wood; i++)
		{
			ItemHolder = Instantiate(itemObject, pos, Quaternion.identity);
			ItemHolder.GetComponent<ItemHolder>().SetItem(Global.Instance.ItemList[8]);
		}
		for (int i = 0; i < iron; i++)
		{
			ItemHolder = Instantiate(itemObject, pos, Quaternion.identity);
			ItemHolder.GetComponent<ItemHolder>().SetItem(Global.Instance.ItemList[9]);
		}
		for (int i = 0; i < lemon; i++)
		{
			ItemHolder = Instantiate(itemObject, pos, Quaternion.identity);
			ItemHolder.GetComponent<ItemHolder>().SetItem(Global.Instance.ItemList[10]);
		}
		for (int i = 0; i < chemicals; i++)
		{
			ItemHolder = Instantiate(itemObject, pos, Quaternion.identity);
			ItemHolder.GetComponent<ItemHolder>().SetItem(Global.Instance.ItemList[11]);
		}
	}

	List<int> GetHalfRessources()
	{
		List<int> half = new List<int>();
		half.Add(0);
		half.Add(0);
		half.Add(0);
		half.Add(0);
		foreach (var key in inventory.Keys)
		{
			var value = inventory[key];
			if (key != "skull" && key != "cristal")
			{
				switch (key)
				{
					case "wood":
						half[0] = value/2;
						break;
					case "iron":
						half[1] = value/2;
						break;
					case "lemon":
						half[2] = value/2;
						break;
					case "chemicals":
						half[3] = value/2;
						break;
				}
			}
		}
		return half;
	}

	IEnumerator Respawn(bool drop)
	{
		if (view.isMine && !isBot)
		{
			cameraEffects.GreyScale(true);
		}
		
		if (drop)
		{
			for (int i = nbGuns - 1; i >= 0; i--)
			{
				curGunId = i;
				DropGun();
			}
			updateGun();
		}
		
		yield return new WaitForSeconds(2);
		
		var deathPos = transform.position;
		transform.position = SpawnPos;
		transform.rotation = Quaternion.identity;
		gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
		isDead = false;
		health = maxHealth;
		healthBar.value = (float)health / maxHealth;
		anim.SetBool("Dead", false);
		anim.SetBool("HoldsWeapon", true);
		anim.Play("Roll");
		
		yield return new WaitForSeconds(0.2f);
		if (drop)
		{
			var list = GetHalfRessources();
			if (inventory.ContainsKey("wood"))
			{
				inventory["wood"] = inventory["wood"] - list[0];
			}
			if (inventory.ContainsKey("iron"))
			{
				inventory["iron"] = inventory["iron"] - list[1];
			}
			if (inventory.ContainsKey("lemon"))
			{
				inventory["lemon"] = inventory["lemon"] - list[2];
			}
			if (inventory.ContainsKey("chemicals"))
			{
				inventory["chemicals"] = inventory["chemicals"] - list[3];
			}
			InGameUI.Instance.InvUpdate();
			view.RPC("DropStuff", PhotonTargets.All, deathPos, list[0], list[1], list[2], list[3]);
		}
		
		if (view.isMine && !isBot)
		{
			setParent(null);
			cameraEffects.GreyScale(false);
		}
	}

	public void AddItem(int id)
	{
		AddItem(Global.Instance.ItemList[id]);
	}
	
	public void AddItem(Item item)
	{
		GetComponent<AudioManagerInstance>().Play("itempick");
		if (inventory.ContainsKey(item.Name))
		{
			inventory[item.Name] += 1;
		}
		else
		{
			inventory[item.Name] = 1;
		}
		
		if (view.isMine && !isBot)
		{
			InGameUI.Instance.InvUpdate();
		}
	}

	public int GetItems(string id)
	{
		return inventory[id];
	}

	public bool setGun(int id)
	{
		if (id < 0 || id >= nbGuns || id == curGunId || guns[id] == null) return false;
    
		curGunId = id;
		updateGun();
		return true;
	}

	public void updateGun()
	{
		if (curGunObject != null)
		{
			Destroy(curGunObject);
		}
		if (view.isMine && !isBot)
		{
			InGameUI.Instance.ClearWeapons();
		}
		var curGun = guns[curGunId];
		curGunObject = Instantiate(curGun.Prefab,shootingHand.position,shootingHand.rotation);
		curGunObject.transform.parent = shootingHand;

	}
	

	public void Shoot()
	{
		if (curGunObject == null){return;}
		
		var curGun = guns[curGunId];
		Transform firePoint = curGunObject.GetComponent<FirePoint>().firePoint;
		if (shotCounter > 0)
		{
			shotCounter -= Time.deltaTime;
		}

		if (isFiring && shotCounter <= 0)
		{
			shotCounter = curGun.TimeBeforeShoot;
			if(view.isMine && !isBot) Mouse.Instance.Shoot(shotCounter);
			Vector3 colorV3 = new Vector3(color.r, color.g, color.b);

			if (curGun.BulletPatterns.Count > 0)
			{
				foreach (var pattern in curGun.BulletPatterns)
				{
					StartCoroutine(PlayShootPattern(pattern, curGun, colorV3, firePoint));
				}
			}
			else
			{
				view.RPC("BulletSpawn", PhotonTargets.All, curGun.BulletPrefab.name, firePoint.position, transform.rotation, curGun.BulletSpeed, colorV3, (int)(curGun.Damage + curGun.Damage*damageBuff/100), view.viewID);
			}
			
		}
	}

	IEnumerator PlayShootPattern(BulletPattern pattern, Item curGun, Vector3 colorV3,Transform firePoint)
	{
		yield return new WaitForSeconds(pattern.delay);
		var bullet = pattern.bullet;
		view.RPC("BulletSpawn", PhotonTargets.All, bullet.name, firePoint.position, Quaternion.Euler(transform.rotation.eulerAngles + pattern.angleOffset), curGun.BulletSpeed, colorV3, (int)(curGun.Damage + curGun.Damage*damageBuff/100), view.viewID);
	}

	[PunRPC]
	void BulletSpawn(string bulletName, Vector3 pos, Quaternion rotation, float speed, Vector3 colorV3, int damage, int id, PhotonMessageInfo info)
	{
		double timestamp = info.timestamp;
		GameObject bullet = Resources.Load(bulletName, typeof(GameObject)) as GameObject;
		GameObject newBullet = Instantiate(bullet, pos, rotation);
		Color color = new Color(colorV3.x, colorV3.y, colorV3.z);
		newBullet.GetComponent<BulletBehavior>().Setup(timestamp, damage, color, speed, id);
	}

	public void PlayWalkSound()
	{
		GetComponent<AudioManagerInstance>().Play("walk");
		//Debug.Log("Walk sound");
	}

	public Sprite getGunImg(int id)
	{
		if (id >= guns.Length || id < 0 || guns[id] == null)
		{
			return null;
		}
		return guns[id].Image;
	}

	public bool AddGun(Item gun, bool set = false)
	{
		for (int i = 0; i < nbGuns ; i++)
		{
			if (guns[i] == null)
			{
				guns[i] = gun;
				GetComponent<AudioManagerInstance>().Play("weaponpick");
				if (set)
				{
					setGun(i);
				}
				return true;
			}
		}
		return false;
	}

	public void DropSort()
	{
		var removed = DropGun();
		if (removed > 0 && removed < guns.Length -1)
		{
			for (int i = removed + 1; i < guns.Length; i++)
			{
				guns[i - 1] = guns[i];
			}
			guns[nbGuns - 1] = null;
		}
		updateGun();
	}

	public int DropGun()
	{
		if (guns[curGunId] != null && curGunId != 0)
		{
			int id = 0;
			for (int i = 0; i < Global.Instance.ItemList.Count; i++)
			{
				if (Global.Instance.ItemList[i] && Global.Instance.ItemList[i].name == guns[curGunId].name)
				{
					id = i;
					break;
				}
			}
			view.RPC("DropGun", PhotonTargets.All, transform.position + 5*transform.forward, id);
			var toRemove = curGunId;
			guns[curGunId] = null;
			curGunId = 0;
			return toRemove;
		}
		else
		{
			Debug.Log("can't drop item");
			return -1;
		}
	}

	[PunRPC]
	public void DropGun(Vector3 pos, int id)
	{
		var itemObject = Resources.Load("ItemHolder", typeof(GameObject)) as GameObject;
		GameObject ItemHolder = Instantiate(itemObject, pos, Quaternion.identity);
		ItemHolder.GetComponent<ItemHolder>().SetItem(Global.Instance.ItemList[id]);
	}

	public bool SameID(int id)
	{
		return id == view.viewID;
	}

	public void setParent(Transform parent)
	{
		if (parent != null)
		{
			view.RPC("SetParent", PhotonTargets.All, view.viewID, parent.name);
		}
		else
		{
			view.RPC("RemParent", PhotonTargets.All, view.viewID);
		}
	}

	[PunRPC]
	private void SetParent(int id, string name)
	{
		GameObject parent = GameObject.Find(name);
		var Players = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < Players.Length; i++)
		{
			var cur = Players[i];
			if (cur.GetComponent<PlayerController>().SameID(id))
			{
				cur.transform.parent = parent.transform;
			}
		}
	}
	
	[PunRPC]
	private void RemParent(int id)
	{
		var Players = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < Players.Length; i++)
		{
			var cur = Players[i];
			if (cur.GetComponent<PlayerController>().SameID(id))
			{
				cur.transform.parent = null;
			}
		}
	}

	[PunRPC]
	void RPCOkHand(int id)
	{
		var Players = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < Players.Length; i++)
		{
			var cur = Players[i];
			if (cur.GetComponent<PlayerController>().SameID(id))
			{
				StartCoroutine(cur.GetComponent<PlayerController>().okHandE());
			}
		}
	}
	
	
	IEnumerator okHandE()
	{
		okHand.gameObject.SetActive(true);
		yield return new WaitForSeconds(1.5f);
		okHand.gameObject.SetActive(false);
	}
	
	
}
