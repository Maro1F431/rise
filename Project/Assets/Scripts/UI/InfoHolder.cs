using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class InfoHolder : MonoBehaviour
{
	public List<PlayerInfo> PlayerInfos;
	public static InfoHolder Instance;
	public Dictionary<string, int> localInv;

	private void Start()
	{
		if (Instance)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public Color getMaxSkulls()
	{
		PlayerInfo info = PlayerInfos[0];
		foreach (var playerInfo in PlayerInfos)
		{
			if (playerInfo.inventory.ContainsKey("skulls") && playerInfo.inventory["skulls"] > info.inventory["skulls"])
			{
				info = playerInfo;
			}
		}
		return info.Color;
	}
	
	public Color getMaxCristals()
	{
		PlayerInfo info = PlayerInfos[0];
		foreach (var playerInfo in PlayerInfos)
		{
			if (playerInfo.inventory.ContainsKey("cristal") && playerInfo.inventory["cristal"] > info.inventory["cristal"])
			{
				info = playerInfo;
			}
		}
		return info.Color;
	}

	public void GatherInfo()
	{
		var Players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in Players)
		{
			var info = new PlayerInfo();
			var PlayerController = player.GetComponent<PlayerController>();
			info.Color = PlayerController.GetColor();
			info.isBot = PlayerController.isBot;
			info.isMe = PlayerController.IsPlayer(false);
			info.inventory = PlayerController.inventory;
			if (info.isMe)
			{
				localInv = info.inventory;
			}
			PlayerInfos.Add(info);
		}
	}
}
