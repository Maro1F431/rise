using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DynamicResText : MonoBehaviour {

	private Text Timer;
	private double offset;
	private double time;
	
	// Use this for initialization
	void Start ()
	{
		offset = Time.time;
		Timer = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		time = Global.Instance.CraftTime*60 - Time.time + offset;
		if (time < 0) time = 0;
		string minutes = ((int)(time / 60)).ToString();
		minutes = (minutes.Length > 1 ? minutes : "0" + minutes);
		string seconds = ((int)(time % 60)).ToString();
		seconds = (seconds.Length > 1 ? seconds : "0" + seconds);
		Timer.text = minutes + ":" + seconds;

		if (time == 0)
		{
			SceneManager.LoadScene("Boss Map");
		}
	}
}
