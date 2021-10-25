using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainBehavior : MonoBehaviour {

	[Header("Starting Game")] 
	public ToggleGroup CreateOrJoin;
	public ToggleGroup NbBots;
	
	public void QuitBtn()
	{
		Application.Quit();
		Debug.Log("Quit");
	}
	public void GalleryBtn()
	{
		SceneManager.LoadScene("Gallery");
	}
	public void TraningBtn()
	{
		Global.Instance.Training();
	}
	public void PlayBtn()
	{
		Global.Instance.LaunchGame(CreateOrJoin, NbBots);
	}
}
