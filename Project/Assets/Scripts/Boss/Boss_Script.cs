using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Boss_Script : MonoBehaviour
{
	// animation part :
	
	public Animator Boss_Animator;
	private int idleHash = Animator.StringToHash("idleTrig");
	private int rangedHash = Animator.StringToHash("rangedTrig");
	private int flameThrowHash = Animator.StringToHash("flameThrowTrig");
	private int meleeHash = Animator.StringToHash("meleeTrig");
	private int healHash = Animator.StringToHash("healTrig");
	private int hurtBackHash = Animator.StringToHash("hurtBackTrig");
	private int groundSmashJumpHash = Animator.StringToHash("groundSmashJumpTrig");
	private int groundSmashAirHash = Animator.StringToHash("groundSmashAirTrig");
	private int groundSmashLandHash = Animator.StringToHash("groundSmashLandTrig");
	private int deathHash = Animator.StringToHash("deathTrig");
	
	
	//Other part
	public Slider healthBar;
	public Slider healCastBar;
	public Slider healthToBreakBar;
	public GameObject bossCollider;
	private PlayerController PlayerControl;
	public int bossHpMax;
	public int bossHP;
	public int healthToBreakMax;
	private int healthToBreak;
	private bool isDead;
	private float healCD;
	private float lastTimeHealed;
	public GameObject Player;
	public float AutoAttackDistance;
	public int autoAttackDamage;
	public int groundSmashDamage;
	public int groundSmashRadius;
	public GameObject aoeIndicator;
	public Transform aoeYPosition;
	public int jumpHeight;
	public int jumpSpeed;
	public Transform firepointOfBoss;
	private int idleBeforeGS;
	private int NBidle;

	public List<Item> itemPats;
	private bool isRanging;
	public List<Item> patsWHeal;
	private bool mustShootHeal;


	// Use this for initialization
	void Start()
	{
		isDead = false;
		bossHP = bossHpMax;
		healthToBreak = healthToBreakMax;
		healthBar.value = 1;
		healCastBar.value = 0;
		healthToBreakBar.value = 1;
		healCastBar.gameObject.SetActive(false);
		healthToBreakBar.gameObject.SetActive(false);
		//put some UI code to make the show host speak
		// Patterns init.
		idleBeforeGS = Random.Range(4, 8);
		healCD = Random.Range(10f, 20f);
		mustShootHeal = false;
		lastTimeHealed = -20;
		Player = GameObject.FindWithTag("Player");
		if (Player != null)
		{
			PlayerControl = Player.GetComponent<PlayerController>();
		}
		else
		{
			PlayerControl = null;
		}
		AutoAttackDistance = 10;
		NBidle = 0;
		isRanging = false;
		StartCoroutine("Idle");


	}

	// Update is called once per frame
	void Update()
	{
		if (isDead) return;
		
		if (!isRanging)
		{
			Vector3 LookAtPos = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);
            transform.LookAt(LookAtPos);
		}
		
		if (bossHP <= 0)
		{
			Death();
		}
		// Sorting Pattern list with action of the player
	}

	IEnumerator Idle()
	{
		isRanging = false;

		NBidle++;
		yield return new WaitForSeconds(2f);
		if ((Time.time - lastTimeHealed  > healCD) && bossHP <= (int)(bossHpMax/2) && !isDead && bossHP > 0)
		{
			StartCoroutine("Heal");
		}
		else if(Player != null && NBidle >= idleBeforeGS && !isDead)
		{
			StartCoroutine("Groundsmash");
		}
		else if (Player != null && (Vector2.Distance(new Vector2(transform.position.x, transform.position.z),new Vector2(Player.transform.position.x, Player.transform.position.z))) <= AutoAttackDistance && !isDead)
		{
			StartCoroutine("Melee");
		}
		
		else if (!isDead)
		{
			
			StartCoroutine("Ranged");
		}
	}



	IEnumerator Melee()
	{
		//melee animation
		
		//-
		if (!PlayerControl.Dashing)
		{	
			GetComponent<AudioManagerInstance>().Play("nomnom");
			PlayerControl.Damage(autoAttackDamage);
			Boss_Animator.SetTrigger(meleeHash);

		}		
		yield return null;
		StartCoroutine("Idle");
	}

	IEnumerator Ranged()
	{
		
		isRanging = true;
		var itemPat = chooseAPat(itemPats);
		//ranged animation
		if (itemPat.name == "delayedLeftFireArc" ||itemPat.name == "delayedRightFireArc" )
		{
			Boss_Animator.SetTrigger(rangedHash);
			GetComponent<AudioManagerInstance>().Play("bloublou");
		}
		else
		{
			Boss_Animator.SetTrigger(flameThrowHash);
			GetComponent<AudioManagerInstance>().Play("roar");
		}
		//-
		float delay = 0f;
		foreach (var pattern in itemPat.BulletPatterns)
		{
			if (pattern.delay > delay)
				delay = pattern.delay;
			StartCoroutine(PlayShootPattern(pattern, itemPat.Damage,itemPat.BulletSpeed, firepointOfBoss));
		}
		
		yield return new WaitForSeconds(delay);
		StartCoroutine("Idle");
	}

	IEnumerator Groundsmash(){
		
		var playerPos = Player.transform.position;
		var bossPos = transform.position;
		var rigidbody = GetComponent<Rigidbody>();
		var instanceIndicator = Instantiate(aoeIndicator, new Vector3(playerPos.x, aoeYPosition.position.y, playerPos.z), Quaternion.identity);
		instanceIndicator.transform.localScale = new Vector3(groundSmashRadius,groundSmashRadius,groundSmashRadius)/5;
		//anim
		Boss_Animator.SetTrigger(groundSmashJumpHash);
		//-
		rigidbody.useGravity = false;	
		while( (transform.position.y - bossPos.y) < jumpHeight)
		{
			transform.position += jumpSpeed * Vector3.up * Time.deltaTime;
			yield return null;
		}
		//anim
		Boss_Animator.SetTrigger(groundSmashAirHash);
		//-
		
		while(Mathf.Abs(playerPos.x -transform.position.x) > 1 || Mathf.Abs(playerPos.z -transform.position.z) > 1)
		{
			var toAdd = new Vector3(playerPos.x -transform.position.x,0,playerPos.z - transform.position.z);
			transform.position += jumpSpeed*toAdd.normalized *Time.deltaTime;	
			yield return null;
		}
		rigidbody.useGravity = true;
		//anim
		Boss_Animator.SetTrigger(groundSmashLandHash);
		//-
		/*while (transform.position.y > bossPos.y)
		{
			transform.position += jumpSpeed*Vector3.down*Time.deltaTime;   //work without gravity
			yield return null;
		}*/
		while (transform.position.y > bossPos.y + 1)
		{
			Debug.Log("is waiting");
			yield return null;
		}
		GetComponent<AudioManagerInstance>().Play("gsmashland");

		Debug.Log("out of waiting");
		if ((Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
			    new Vector2(Player.transform.position.x, Player.transform.position.z))) <= groundSmashRadius
		    && !PlayerControl.Dashing)
		{
			Debug.Log("before damage");
			PlayerControl.Damage(groundSmashDamage); // this is broken on training (put damage and stop working)
			Debug.Log("damaged melee player");// it doest go on this step
		}
		Debug.Log("bite");
		Destroy(instanceIndicator);	
		Debug.Log("bite2");
		NBidle = 0;
		idleBeforeGS = Random.Range(8, 15);
		yield return null;
		StartCoroutine("Idle");
	}

	IEnumerator Heal()
	{
		//heal ( can be interupted by shooting at the boss), the heal will be fast, will need reaction), will need to break by a certain number of damage
		//heal animation
		Boss_Animator.SetTrigger(healHash);
		//-
		isRanging = true;
		healCastBar.gameObject.SetActive(true);
		healthToBreakBar.gameObject.SetActive(true);
		var start = Time.time;
		int patToUse = 0;
		mustShootHeal = true;
		StartCoroutine("playHealPattern",0);
		GetComponent<AudioManagerInstance>().Play("heal");
		while (healCastBar.value < 6 && healthToBreak > 0)
		{
			healCastBar.value = Time.time - start;
			
			yield return null;
		}
		mustShootHeal = false;
		if (healthToBreak > 0)// heal succed, no need for "daze" animation
		{
			bossHP += (int)(0.2 * bossHpMax);
			healthBar.value = (float)bossHP / bossHpMax;
			Boss_Animator.SetTrigger(idleHash);
		}
		else
		{
			//animation "hurt back" (interuption of the heal)
			Boss_Animator.SetTrigger(hurtBackHash);
			//-
			bossHP -= (int) (0.1 * bossHpMax);
			healthBar.value = (float)bossHP / bossHpMax;
			
		}
		
		healCastBar.value = 0;
		healCastBar.gameObject.SetActive(false);
		healthToBreak = healthToBreakMax;
		healthToBreakBar.value = 1;
		healthToBreakBar.gameObject.SetActive(false);
		lastTimeHealed = Time.time;
		yield return null;
		StartCoroutine("Idle");
	}

	public void Damage(int damage)
	{
		if (healthToBreakBar.gameObject.activeSelf == false)
		{
			bossHP -= damage;
			healthBar.value = (float)bossHP / bossHpMax;

		}
		else
		{
			healthToBreak -= damage;
			healthToBreakBar.value = (float)healthToBreak / healthToBreakMax;

		}

	
		
	}

	void Death()
	{
		isDead = true;
		//death animation of the boss and pop ui elements... + change scene.
		//anim
		Boss_Animator.SetTrigger(deathHash);
		GetComponent<AudioManagerInstance>().Play("death");
		BossTimer.Instance.EndGame(true);
		//-
	}

	public bool IsDead
	{
		get { return isDead; }
	}

	Item chooseAPat(List<Item> Patterns) // do not take into account the fact that the list.count can be 1 or 2 (we have list.count >=3) (if count = 3 it is like choosing a random pattern)
	{
		var randomNb = Random.Range(0, 2);
		var toReturn = Patterns[randomNb];
		Patterns.RemoveAt(randomNb);
		Patterns.Add(toReturn);
		return toReturn;

	}
	
	IEnumerator PlayShootPattern(BulletPattern pattern,int damage,float bulletSpeed,Transform firePoint)
	{
		yield return new WaitForSeconds(pattern.delay);
		var bullet = pattern.bullet;
		var newBullet = Instantiate(bullet, firePoint.position, Quaternion.Euler(firepointOfBoss.transform.rotation.eulerAngles + pattern.angleOffset));
		newBullet.GetComponent<BulletBehavior>().Setup(damage,bulletSpeed);
	}

	IEnumerator playHealPattern(int nbListPat)
	{
		var itemPat = patsWHeal[nbListPat];
		float delay = 0f;
		foreach (var pattern in itemPat.BulletPatterns)
		{
			if (pattern.delay > delay)
				delay = pattern.delay;
			StartCoroutine(PlayShootPattern(pattern, itemPat.Damage,itemPat.BulletSpeed, firepointOfBoss));
		}
		yield return new WaitForSeconds(delay);
		
		if (nbListPat == 0 && mustShootHeal)
		{
			StartCoroutine("playHealPattern", 1);
		}
		else if (nbListPat == 1 && mustShootHeal)
		{
			StartCoroutine("playHealPattern", 0);
		}
	}

	
}
