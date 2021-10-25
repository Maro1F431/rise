using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManagerInstance : MonoBehaviour {

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;
	
	void Awake()
	{
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.spatialBlend = 1;
			s.source.rolloffMode = s.rolloffmode;
			s.source.minDistance = s.minDistance;
			s.source.maxDistance = s.maxDistance;
			s.source.outputAudioMixerGroup = mixerGroup;

			if (s.playOnStart)
			{
				Play(s.name);
			}
			
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}
}
