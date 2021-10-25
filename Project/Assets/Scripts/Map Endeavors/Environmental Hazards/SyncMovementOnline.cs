using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncMovementOnline : MonoBehaviour {

	// Use this for initialization
	void Update ()
	{
		var animator = GetComponent<Animator>();
		if (animator)
		{
			var time = PhotonNetwork.time;
			var length = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
			float progress = (float)(time % length) / length;
			animator.Play(0, 0, progress);
		}
	}
}
