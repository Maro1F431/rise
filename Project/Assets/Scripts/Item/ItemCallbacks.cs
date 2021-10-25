using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemCallbacks {

	public static void Eval(PlayerController player, Item item, string callback)
	{
		var callbackarray = callback.Split(' ');
		Debug.Log("Item Call back evalled with " + callback);
		switch (callbackarray[0].ToLower())
		{
				case "heal":
					Debug.Log("Heal!");
					Heal(player, callbackarray[1] != null? int.Parse(callbackarray[1]) : 0);
					break;
					
				case "addgun":
					Debug.Log("Equip gun!");
					AddGun(player, item, callbackarray[1] != null && bool.Parse(callbackarray[1]));
					break;
		}
	}

	static void Heal(PlayerController player, int amount)
	{
		player.Heal(amount);
	}

	static void AddGun(PlayerController player, Item item, bool equip)
	{
		player.AddGun(item, equip);
	}
}
