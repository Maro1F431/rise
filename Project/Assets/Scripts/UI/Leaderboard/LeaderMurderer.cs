using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderMurderer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Image>().color = InfoHolder.Instance.getMaxSkulls();
	}
}
