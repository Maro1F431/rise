using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouthLookPlayer : MonoBehaviour
{

    private GameObject Player;
	// Use this for initialization
	void Start () {
		Player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(Player.transform);
	}
}
