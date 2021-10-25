using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingOnButtonClick : MonoBehaviour
{
	public static CraftingOnButtonClick Instance;

	public Image ItemImage;
	public Text ItemText;
	public Text Skull;
	public Text Cristal;
	public Text Wood;
	public Text Iron;
	public Text Lemons;
	public Text Chemicals;
	public Text FlavorText;
	public Button CraftBtn;
	public Recipee CurrentRecipee;
	public List<CraftingItem> CraftingItems;
	
	// Use this for initialization
	void Start ()
	{
		Crafting.RessourcesList = InfoHolder.Instance.localInv;
		ItemImage.gameObject.SetActive(false);
		ItemText.gameObject.SetActive(false);
		Skull.gameObject.SetActive(false);
		Cristal.gameObject.SetActive(false);
		Wood.gameObject.SetActive(false);
		Iron.gameObject.SetActive(false);
		Lemons.gameObject.SetActive(false);
		Chemicals.gameObject.SetActive(false);
		CraftBtn.gameObject.SetActive(false);
		FlavorText.gameObject.SetActive(false);
		
		CurrentRecipee = null;
		Instance = this;
	}

	public void Craft()
	{
		Crafting.Craft(CurrentRecipee);
		OnClick(CurrentRecipee);
		var curItem = CraftingItems.Find(item => item.recipee == CurrentRecipee);
		curItem.ChangeColor();
	}

	public void OnClick(Recipee recipee)
	{
		ItemImage.gameObject.SetActive(true);
		ItemText.gameObject.SetActive(true);
		Skull.gameObject.SetActive(true);
		Cristal.gameObject.SetActive(true);
		Wood.gameObject.SetActive(true);
		Iron.gameObject.SetActive(true);
		Lemons.gameObject.SetActive(true);
		Chemicals.gameObject.SetActive(true);
		FlavorText.gameObject.SetActive(true);
		CurrentRecipee = recipee;
		ItemImage.sprite = recipee.result.Image;
		ItemText.text = recipee.name;
		FlavorText.text = recipee.result.FlavorText;
		if (Crafting.CanCraft(recipee))
		{
			CraftBtn.gameObject.SetActive(true);
		}
		else
		{
			CraftBtn.gameObject.SetActive(false);
		}
		Skull.text = "x 0";
		Cristal.text = "x 0";
		Wood.text = "x 0";
		Iron.text = "x 0";
		Lemons.text = "x 0";
		Chemicals.text = "x 0";
		
		foreach (RecipeeItem recipeeItem in recipee.itemList)
		{
			switch (recipeeItem.item.Name.ToLower())
			{
				case "skull":
					Skull.text = "x " + recipeeItem.amount;
					break;
				case "cristal":
					Cristal.text = "x " + recipeeItem.amount;
					break;
				case "wood":
					Wood.text = "x " + recipeeItem.amount;
					break;
				case "iron":
					Iron.text = "x " + recipeeItem.amount;
					break;
				case "lemon":
					Lemons.text = "x " + recipeeItem.amount;
					break;
				case "chemicals":
					Chemicals.text = "x " + recipeeItem.amount;
					break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
