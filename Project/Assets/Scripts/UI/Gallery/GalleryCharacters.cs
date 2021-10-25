using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryCharacters : MonoBehaviour
{
	public List<GameObject> characters;
	public List<Image> images;
	public List<Image> bgs;
	public List<Text> texts;
	
	// Use this for initialization
	void Start () {
		for (int i = 0; i < characters.Count; i++)
		{
			var character = characters[i];
			bool dead = false;
			Color color = Color.white;
			string state = "Dead";
			if (PlayerPrefs.HasKey("chara" + i))
			{
				var key = PlayerPrefs.GetString("chara" + i);
				var keyA = key.Split(',');
				
				dead = keyA[0] != "1";
				color = new Color(float.Parse(keyA[1]), float.Parse(keyA[1]), float.Parse(keyA[3]));
				if (!dead)
				{
					state = "Rise";
				}
				character.GetComponent<Animator>().SetBool("Dead", dead);
				images[i].color = color;
				texts[i].text = state;
			}
			else
			{
				Destroy(character);
				Destroy(texts[i]);
				Destroy(images[i]);
				Destroy(bgs[i]);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
