using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraEffects : MonoBehaviour
{
	public float fadeOutEffects = 0.01f;
	public float fadeOutShake = 0.01f;

	private PostProcessingProfile PostProcess;
	// Use this for initialization
	void Start ()
	{
		PostProcess = GetComponent<PostProcessingBehaviour>().profile;
		
	}

	public void Damage()
	{
		PostProcess.vignette.enabled = true;
		var vignetteSettings = PostProcess.vignette.settings;
		vignetteSettings.intensity = 0.3f;
		vignetteSettings.color = Color.red;
		PostProcess.vignette.settings = vignetteSettings;
		
		PostProcess.chromaticAberration.enabled = true;
		var chromaSettings = PostProcess.chromaticAberration.settings;
		chromaSettings.intensity = 1;
		PostProcess.chromaticAberration.settings = chromaSettings;
	}
	
	public void Heal()
	{
		PostProcess.vignette.enabled = true;
		var vignetteSettings = PostProcess.vignette.settings;
		vignetteSettings.intensity = 0.3f;
		vignetteSettings.color = Color.green;
		PostProcess.vignette.settings = vignetteSettings;
	}

	public void GreyScale(bool apply)
	{
		var colorGradingSettings = PostProcess.colorGrading.settings;
		colorGradingSettings.basic.saturation = apply? 0:1;
		PostProcess.colorGrading.settings = colorGradingSettings;
	}

	public void Shake()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		if (PostProcess.vignette.settings.intensity >= 0.01)
		{
			PostProcess.vignette.enabled = true;
			var vignetteSettings = PostProcess.vignette.settings;
			vignetteSettings.intensity = Mathf.Lerp(vignetteSettings.intensity, 0, fadeOutEffects);
			PostProcess.vignette.settings = vignetteSettings;
		}
		else
		{
			PostProcess.vignette.enabled = false;
		}
		
		if (PostProcess.chromaticAberration.settings.intensity >= 0.01)
		{
			PostProcess.chromaticAberration.enabled = true;
			var chromaSettings = PostProcess.chromaticAberration.settings;
			chromaSettings.intensity = Mathf.Lerp(chromaSettings.intensity, 0, fadeOutEffects);
			PostProcess.chromaticAberration.settings = chromaSettings;
		}
		else
		{
			PostProcess.chromaticAberration.enabled = false;
		}
	}
}
