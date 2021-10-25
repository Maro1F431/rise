using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class betterPlayVideo : MonoBehaviour {

	public string nextScene;
	private VideoPlayer videoPlayer;
	private bool hasPlayed;
	
	// Use this for initialization
	void Start ()
	{
		videoPlayer = GetComponent<VideoPlayer>();
		videoPlayer.Prepare();
		hasPlayed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!videoPlayer.isPlaying && hasPlayed)
		{
			SceneManager.LoadScene(nextScene);
		}
		if (videoPlayer.isPrepared && !videoPlayer.isPlaying)
		{
			videoPlayer.Play();
			hasPlayed = true;
		}
	}
}
