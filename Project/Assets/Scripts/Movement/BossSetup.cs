using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSetup : MonoBehaviour {

	// Use this for initialization
	void Start()
	{
		if (SceneManager.GetActiveScene().name != "Boss Map") return;

		PhotonNetwork.offlineMode = true;
		PhotonNetwork.CreateRoom("boss");
		GetComponent<Rigidbody>().useGravity = true;
		var player = GetComponent<PlayerController>();
		player.SetColor(InfoHolder.Instance.PlayerInfos.Find(info => info.isMe).Color);
		
		int health = 0;
		int armor = 0;
		int damage = 0;
		float speed = 0;
		int cdr = 0;

		if (Crafting.crafted != null)
		{
			foreach (var item in Crafting.crafted)
			{
				if (item.IsBuff)
				{
					health += item.HpBuff;
					armor += item.armorBuff;
					damage += item.damageBuff;
					speed += item.speedBuff;
					cdr += item.DodgeCDR;
				}
				else if(item.IsWeapon)
				{
					player.AddGun(item);
				}
			}
	
			player.maxHealth += health;
			player.health = player.maxHealth;
			player.armor = Mathf.Min(armor, player.maxArmor);
			player.dashCDR = cdr;
			player.speedBuff = speed;
			player.damageBuff = damage;
			player.armorBar.value = (float)armor/player.maxArmor;
		}
		player.updateGun();
	}
}
