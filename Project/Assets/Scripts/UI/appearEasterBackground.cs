using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class appearEasterBackground : MonoBehaviour
{

	public List<Image> ImageList;
	public int WaitTime;
	private int cur;
	private System.Random rand = new System.Random();
	
	// Use this for initialization
	void Start () {
		foreach (var image in ImageList)
		{
			image.CrossFadeAlpha(0, 0, true);
		}
		ImageList[0].CrossFadeAlpha(1, 0, true);
		StartCoroutine(LoopThru());
	}

	IEnumerator LoopThru()
	{
		while (true)
		{
			yield return new WaitForSeconds(WaitTime);
			ImageList[1].CrossFadeAlpha(1,1,true);
			StopCoroutine("LoopThru");
		}
	}
}
