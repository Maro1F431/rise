using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingButton : MonoBehaviour
{

	public CraftingItem Item;
	
	// Update is called once per frame
	public void Click () {
		Debug.Log("Clicked");
		CraftingOnButtonClick.Instance.OnClick(Item.recipee);
	}
}
