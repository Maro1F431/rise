using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkController : Photon.MonoBehaviour
{

	public string _gameversion = "0.2";

	public float LastPlayerJoinTime;
	
	public int NbPlayers;
	
	public int NbBots;
	
	// Use this for initialization
	void Start ()
	{
		LastPlayerJoinTime = -1;
		NbPlayers = 0;
		NbBots = 0;
		if (Global.Instance.Offline)
		{
			PhotonNetwork.offlineMode = true;
			NbBots = Global.Instance.MaxNumberOfBots;
			CreateRoom();
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings(_gameversion);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (PhotonNetwork.isMasterClient && LastPlayerJoinTime != -1 &&
		    LastPlayerJoinTime + Global.Instance.PlayerTimeout <= Time.time && 
		    NbBots < Global.Instance.MaxNumberOfBots)
		{
			NbBots++;
			LastPlayerJoinTime = Time.time;
			Global.Instance.NumberOfBots = NbBots;
		}
		if (PhotonNetwork.isMasterClient && NbBots + NbPlayers == 4)
		{
			if (!PhotonNetwork.offlineMode)
			{
				PhotonNetwork.room.IsOpen = false;
			}
			photonView.RPC("SyncScene", PhotonTargets.All);
			PhotonNetwork.LoadLevel(Global.Instance.TargetMap);
		}
	}

	void OnJoinedLobby()
	{
		Debug.Log("Trying to connect to a random room");
		if (Global.Instance && Global.Instance.Create)
		{
			CreateRoom();
		}
		else
		{
			PhotonNetwork.JoinRandomRoom();
		}
	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room! A room is being created.");
		CreateRoom();
	}

	void OnJoinedRoom()
	{
		Debug.Log("A room has been joined !");
		photonView.RPC("UpdateTime", PhotonTargets.MasterClient);
		Global.Instance.ClientId = PhotonNetwork.playerList.Length - 1;

	}

	void CreateRoom()
	{
		Debug.Log("Creating room...");
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 4;
		Debug.Log("Setting room options");
		PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default);
	}
	
	[PunRPC]
	public void UpdateTime()
	{
		LastPlayerJoinTime = Time.time;
		NbPlayers++;
	}

	[PunRPC]
	public void SyncScene()
	{
		PhotonNetwork.automaticallySyncScene = true;
	}
}
