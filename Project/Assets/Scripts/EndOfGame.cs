using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EndOfGame
{
	public static bool end = false;
	
	public static void End(bool won, Color color)
	{
		end = true;
		
		for (int i = 3; i >= 0; i--)
		{
			if (PlayerPrefs.HasKey("chara" + i))
			{
				PlayerPrefs.SetString("chara" + (i + 1), PlayerPrefs.GetString("chara"+i));
			}
		}
		PlayerPrefs.SetString("chara0", (won? "1":"0") + "," + color.r +"," + color.g +"," + color.b);
	}
}
