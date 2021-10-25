using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Toggle = UnityEngine.Experimental.UIElements.Toggle;

public class Global : MonoBehaviour
{
	public static Global Instance { get; private set; }

	public int GameTime;
	public int CraftTime;
	public int BossTime;
	
	[Header( "Map List")]
	public List<String> MapList;
	
	[Header( "Item List")]
	public List<Item> ItemList;

	[Header("Recipee List")] 
	public List<Recipee> Recipees;

	[Header("Event List")] 
	public List<GameEvent> EventList;

	[Header("Room Joining values")] 
	public bool Offline;
	
	public bool Create;

	public int MaxNumberOfBots;

	public int NumberOfBots;

	public string TargetMap;

	public int DefaultNumberOfBots;

	public int PlayerTimeout;

	public int ClientId;

	private void Awake()
	{
		Cursor.visible = true;
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void LaunchGame(ToggleGroup CreateOrJoin, ToggleGroup NbBots)
	{
		Create = CreateOrJoin.ActiveToggles().First().name == "Create";
		if (Create)
			MaxNumberOfBots = int.Parse(NbBots.ActiveToggles().First().name);
		else
			MaxNumberOfBots = DefaultNumberOfBots;
		
		NumberOfBots = 0;
		TargetMap = MapList[Random.Range(0, MapList.Count -1)];
		SceneManager.LoadScene("PUN");
	}

	public void Training()
	{
		Offline = true;
		Create = true;
		MaxNumberOfBots = 3;
		NumberOfBots = 3;
		TargetMap = MapList[Random.Range(0, MapList.Count -1)];
		SceneManager.LoadScene("PUN");
	}
}
