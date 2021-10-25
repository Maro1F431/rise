using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRessourcesUI : MonoBehaviour {

	public Text Skull;
	public Text Cristal;
	public Text Wood;
	public Text Iron;
	public Text Lemons;
	public Text Chemicals;
	
	// Update is called once per frame
	void Update ()
	{
		Skull.text = "x " + (Crafting.ressources.ContainsKey("skull")? Crafting.ressources["skull"]: 0);
		Cristal.text = "x " + (Crafting.ressources.ContainsKey("cristal")? Crafting.ressources["cristal"]: 0);
		Wood.text = "x " + (Crafting.ressources.ContainsKey("wood")? Crafting.ressources["wood"]: 0);
		Iron.text = "x " + (Crafting.ressources.ContainsKey("iron")? Crafting.ressources["iron"]: 0);
		Lemons.text = "x " + (Crafting.ressources.ContainsKey("lemon")? Crafting.ressources["lemon"]: 0);
		Chemicals.text = "x " + (Crafting.ressources.ContainsKey("chemicals")? Crafting.ressources["chemicals"]: 0);
	}
}
