using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityScript.Macros;

public class InGameUI : MonoBehaviour
{
	public static InGameUI Instance;
	
	public PlayerController PlayerController;

	public Image Selector;
	
	public List<Image> Weapons;
	public List<RessourceUI> Ressources;

	public Text Timer;

	public double time;

	public double offset;

	private int id;
	private int nbGuns;

	public GameObject EndOfTimeText;

	public GameObject PauseMenu;

	private void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		time = 69;
		offset = (Global.Instance != null ? PhotonNetwork.time : Time.time);
		id = 0;
		foreach (RessourceUI ressourceUi in Ressources)
		{
			ressourceUi.image.sprite = ressourceUi.Item.Image;
		}
	}

	public void SetOffset(double time)
	{
		offset = time;
	}

	public void SetEventList(List<Vector2> list)
	{
		GetComponent<TimeEvents>().SetList(list);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (PlayerController)
		{
			TimerUpdate();
			WeaponSlotUpdate();
			
		}
		else if(NetworkManager.MainPlayer)
		{
			PlayerController = NetworkManager.MainPlayer.GetComponent<PlayerController>();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
		}
	}

	public void SkipToCraft()
	{
		PlayerController.inventory["skull"] = 99;
		PlayerController.inventory["cristal"] = 99;
		PlayerController.inventory["lemon"] = 99;
		PlayerController.inventory["wood"] = 99;
		PlayerController.inventory["iron"] = 99;
		PlayerController.inventory["chemicals"] = 99;
		StartCoroutine(GoToCrafting(0));
	}
	
	public IEnumerator GoToCrafting(int i)
	{
		InfoHolder.Instance.GatherInfo();
		EndOfTimeText.SetActive(true);
		PhotonNetwork.Disconnect();
		yield return new WaitForSeconds(i);
		SceneManager.LoadScene("LeaderBoard");
	}

	public void Quit()
	{
		PhotonNetwork.Disconnect();
		SceneManager.LoadScene("MainMenu");
	}

	void TimerUpdate()
	{
		if (SceneManager.GetActiveScene().name == "Boss Map") return;
		time = (Global.Instance != null? Global.Instance.GameTime : 15)* 60 - (Global.Instance != null? PhotonNetwork.time: Time.time) + offset;
		if (time < 0) time = 0;
		string minutes = ((int)(time / 60)).ToString();
		minutes = (minutes.Length > 1 ? minutes : "0" + minutes);
		string seconds = ((int)(time % 60)).ToString();
		seconds = (seconds.Length > 1 ? seconds : "0" + seconds);
		Timer.text = minutes + ":" + seconds;
	}

	public void ClearWeapons()
	{
		nbGuns = 0;
		for (int i = 0; i < Weapons.Count; i++)
		{
			var cur = Weapons[i];
			var gunImg = PlayerController.getGunImg(i);
			if (gunImg)
			{
				nbGuns++;
				cur.GetComponent<WeaponSlot>().weaponImage.enabled = true;
				cur.GetComponent<WeaponSlot>().weaponImage.sprite = gunImg;
			}
			else
			{
				cur.GetComponent<WeaponSlot>().weaponImage.enabled = false;
			}
		}
		Selector.transform.position = Weapons[PlayerController.curGunId].transform.position;
	}

	void WeaponSlotUpdate()
	{
		var id1 = id;
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			id = id == nbGuns-1? 0 : id + 1;
			if (!PlayerController.setGun(id))
			{
				id = 0;
			}
			
		}
		else if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			id = id == 0? nbGuns-1 : id - 1;
			if (!PlayerController.setGun(id))
			{
				id = nbGuns-1;
			}
		}
		if (id != id1)
		{
			Selector.transform.position = Weapons[id].transform.position;
			// ligne changement d'arme
		}
	}

	public void InvUpdate()
	{
		var inv = PlayerController.inventory;
		foreach (RessourceUI ressourceUi in Ressources)
		{
			if (inv.ContainsKey(ressourceUi.Item.Name))
			{
				ressourceUi.text.text = inv[ressourceUi.Item.Name].ToString();
			}
			else
			{
				ressourceUi.text.text = "0";
			}
		}
	}
}
