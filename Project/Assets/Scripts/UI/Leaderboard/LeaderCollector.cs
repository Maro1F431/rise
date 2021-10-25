using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderCollector : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		GetComponent<Image>().color = InfoHolder.Instance.getMaxCristals();
	}
}
