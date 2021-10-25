using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{

	public GameObject loadingScreenObj;
	public Slider slider;

	private AsyncOperation async;

	public void LoadScene(string scene)
	{
		StartCoroutine(LoadingScreen(scene));
	}

	IEnumerator LoadingScreen(string scene)
	{
		loadingScreenObj.SetActive(true);
		async = SceneManager.LoadSceneAsync(scene);
		async.allowSceneActivation = false;

		while (!async.isDone)
		{
			slider.value = async.progress;
			if (async.progress == 0.9f)
			{
				slider.value = 1f;
				async.allowSceneActivation = true;
			}
			yield return null;
		}
	}
}
