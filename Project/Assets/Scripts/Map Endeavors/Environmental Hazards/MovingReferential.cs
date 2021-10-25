using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingReferential : MonoBehaviour
{
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player" && other.GetComponent<PlayerController>().IsPlayer(false))
		{
			other.GetComponent<PlayerController>().setParent(transform.parent);
		}
	}

	private void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"  && other.GetComponent<PlayerController>().IsPlayer(false) && other.transform.parent == transform.parent)
		{
			other.GetComponent<PlayerController>().setParent(null);
		}
	}
}
