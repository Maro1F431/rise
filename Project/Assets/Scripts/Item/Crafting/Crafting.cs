using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Crafting
{

	public static Dictionary<string, int> ressources;
	public static List<Item> crafted;

	public static Dictionary<string, int> RessourcesList
	{
		set { ressources = value; }
	}
	
	private static List<Recipee> getRecipeeList()
	{
		return Global.Instance.Recipees;
	}
	

	public static bool CanCraft(Recipee recipee)
	{
		if (crafted != null && crafted.Contains(recipee.result)) return false;
		var recipeeItems = recipee.itemList;
		foreach (var recipeeItem in recipeeItems)
		{
			var amount = recipeeItem.amount;
			var item = recipeeItem.item;
			var counter = 0;
			
			if (!ressources.ContainsKey(item.Name) || ressources[item.Name] < amount)
			{
				return false;
			}
		}
		return true;
	}

	public static void Craft(Recipee recipee)
	{
		Debug.Log("Crafted " + recipee.result.Name);
		if (crafted == null)
		{
			crafted = new List<Item>();
		}
		var recipeeItems = recipee.itemList;
		foreach (var recipeeItem in recipeeItems)
		{
			var amount = recipeeItem.amount;
			var item = recipeeItem.item;

			ressources[item.Name] -= amount;
		}
		crafted.Add(recipee.result);
	}
	
	
}
