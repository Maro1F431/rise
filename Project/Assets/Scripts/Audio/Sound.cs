using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[System.Serializable]
public class Sound {

	public string name;

	public AudioClip clip;

	public bool playOnStart;
	
	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(0f, 1f)]
	public float volumeVariance = .1f;

	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	[Range(0f, 256f)] 
	public float priority = 128f;
	
	public AudioRolloffMode rolloffmode;

	[Range(0, 500)] 
	public int minDistance;
	
	[Range(0, 500)] 
	public int maxDistance;

	public bool loop = false;

	public AudioMixerGroup mixerGroup;

	[HideInInspector]
	public AudioSource source;

}
