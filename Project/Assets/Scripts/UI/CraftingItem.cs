using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItem : MonoBehaviour
{
	public Item item;
	public Recipee recipee;
	public Text text;
	
	// Use this for initialization
	void Start ()
	{
		if (recipee)
		{
			GetComponent<Image>().sprite = recipee.result.Image;
		}
		else
		{
			GetComponent<Image>().sprite = item.Image;
		}
		
	}

	public void ChangeColor()
	{
		text.color = Color.blue;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
