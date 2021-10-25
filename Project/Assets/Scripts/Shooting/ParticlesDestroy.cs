using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesDestroy : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(DestroyAfterSeconds(2));
	}

	IEnumerator DestroyAfterSeconds(int i)
	{
		yield return new WaitForSeconds(i);
		Destroy(gameObject);
	}
}
