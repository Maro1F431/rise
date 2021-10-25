using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomTipText : MonoBehaviour
{
	public List<String> TipList;
	
	// Use this for initialization
	void Start ()
	{
		var text = GetComponent<Text>();
		text.text = "Tip: " + TipList[Random.Range(0, TipList.Count)];
	}
}
