﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	[Header("OBJECTS")]
	public Transform loadingBar;
	public Transform textPercent;

	[Header("VARIABLES (IN-GAME)")]
	public bool isOn;
	public bool restart;
	[Range(0, 100)] public float currentPercent;
	[Range(0, 100)] public float speed;

    [Header("SPECIFIED PERCENT")]
    public bool enableSpecified;
    public bool enableLoop;
    [Range(0, 100)] public float specifiedValue;

    void Update ()
	{
		if (currentPercent <= 100 && isOn == true && enableSpecified == false) 
		{
			currentPercent += speed;
		}

        if (currentPercent <= 100 && isOn == true && enableSpecified == true)
        {
            if(currentPercent <= specifiedValue)
            {
                currentPercent += speed;
            }

            if (enableLoop == true && currentPercent >= specifiedValue)
            {
                currentPercent = 0;
            }
        }

        if (currentPercent >= 100 && restart == true)
		{
			currentPercent = 0;
		}
		if (currentPercent > 100) currentPercent = 100;
		loadingBar.GetComponent<Image> ().fillAmount = currentPercent / 100;
	}
}