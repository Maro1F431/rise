using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : Photon.MonoBehaviour
{
	public SpawnManager SpawnManager;
	public Camera MainCamera;
	public GameObject Player;
	public static GameObject MainPlayer;
	public static List<GameObject> PlayerList;

	//private bool Preview;
	
	// Use this for initialization
	void Start ()
	{
		PlayerList = new List<GameObject>();
		if (Global.Instance == null)
		{
			GameObject player = Instantiate(Player, SpawnManager.PlayersSpawn.SpawnList[0].position, Quaternion.identity);
			PlayerList.Add(player);
			player.GetComponent<PlayerController>().SpawnManager = SpawnManager;
			MainPlayer = player;
			MainCamera.GetComponent<CameraFollow>().Player = player;
			return;
		}
		//Preview = false;
		
		if (Global.Instance.Offline)
		{
			//MainCamera.GetComponent<CameraFollow>().Player = PhotonNetwork.Instantiate(Player.name, SpawnManager.PlayersSpawn.SpawnList[0].position, Quaternion.identity, 0);
			GameObject MainPlayer = PhotonNetwork.Instantiate(Player.name, SpawnManager.PlayersSpawn.SpawnList[0].position, Quaternion.identity, 0);
			PlayerList.Add(MainPlayer);
			MainPlayer = MainPlayer;
			MainCamera.GetComponent<CameraFollow>().Player = MainPlayer;
			MainPlayer.GetComponent<PlayerController>().SpawnManager = SpawnManager;
			
			for (int i = 1; i < SpawnManager.PlayersSpawn.SpawnList.Count; i++)
			{
				GameObject player = PhotonNetwork.Instantiate(Player.name, SpawnManager.PlayersSpawn.SpawnList[i].position, Quaternion.identity, 0);
				PlayerList.Add(player);
				player.GetComponent<PlayerController>().isBot = true;
				player.GetComponent<PlayerController>().SpawnManager = SpawnManager;
			}
		}
		else if (PhotonNetwork.isMasterClient)
		{
			CreateLocal();
			for (int i = 3; i >= 4-Global.Instance.NumberOfBots; i--)
			{
				GameObject player = PhotonNetwork.Instantiate(Player.name, SpawnManager.PlayersSpawn.SpawnList[i].position, Quaternion.identity, 0);
				PlayerList.Add(player);
				player.GetComponent<PlayerController>().isBot = true;
				player.GetComponent<PlayerController>().SpawnManager = SpawnManager;
			}
		}
		else
		{
			CreateLocal();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateLocal()
	{
		var SpawnPnt = Global.Instance.ClientId;
		Debug.Log("Local is created for " + Player.name);
		GameObject player = PhotonNetwork.Instantiate(Player.name, SpawnManager.PlayersSpawn.SpawnList[SpawnPnt].position, Quaternion.identity,0);
		PlayerList.Add(player);
		player.GetComponent<PlayerController>().SpawnManager = SpawnManager;
		MainPlayer = player;
		MainCamera.GetComponent<CameraFollow>().Player = player;
	}
}
