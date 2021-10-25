using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsBehavior : MonoBehaviour
{
	public AudioMixer master;
	public AudioMixer music;
	public AudioMixer sounds;

	private Resolution[] resolutions;
	public Dropdown resDropdown;
	
	public Toggle VideoToggle;
	public Toggle AudioToggle;
	
	public Slider MasterSlider;
	public Slider MusicSlider;
	public Slider SoundSlider;
	
	void Start()
	{
		InitResolution();
		InitToggle();
		InitAudio();
	}

	void InitResolution()
	{
		resolutions = Screen.resolutions;
		
		resDropdown.ClearOptions();
		
		List<string> options = new List<string>();
		int curResIndex = 0;

		for (int i = 0; i < resolutions.Length; i++)
		{
			var res = resolutions[i];
			
			string option = res.width + "x" + res.height;
			options.Add(option);

			if (res.width == Screen.currentResolution.width &&
			    res.height == Screen.currentResolution.height)
			{
				curResIndex = i;
			}
		}
		
		resDropdown.AddOptions(options);
		resDropdown.value = curResIndex;
		resDropdown.RefreshShownValue();
	}

	public void InitToggle()
	{
		AudioToggle.isOn = true;
		VideoToggle.isOn = false;
	}

	void InitAudio()
	{
		float temp;
		if (master.GetFloat("volume", out temp))
		{
			MasterSlider.value = temp;
		}
		
		if (music.GetFloat("volume", out temp))
		{
			MusicSlider.value = temp;
		}
		
		if (sounds.GetFloat("volume", out temp))
		{
			SoundSlider.value = temp;
		}
		
	}
	
	public void SetMaster(float volume)
	{
		master.SetFloat("volume", volume);
	}
	public void SetMusic(float volume)
	{
		music.SetFloat("volume", volume);
	}
	public void SetSound(float volume)
	{
		sounds.SetFloat("volume", volume);
	}
	public void SetQuality(int quality)
	{
		QualitySettings.SetQualityLevel(quality);
	}
	public void SetFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}
	public void SetResolution(int resIndex)
	{
		Resolution res = resolutions[resIndex];
		Screen.SetResolution(res.width, res.height, Screen.fullScreen);
	}
}
