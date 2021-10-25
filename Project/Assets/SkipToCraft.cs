using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipToCraft : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(wait(5));
	}


	IEnumerator wait(int sec)
	{
		yield return new WaitForSeconds(sec);
		SceneManager.LoadScene("Crafting");
	}
}
