using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayBehavior : MonoBehaviour
{

	public Toggle CreateToggle;
	public Toggle JoinToggle;
	
	void Start () {
		InitToggle();
	}
	
	public void InitToggle()
	{
		CreateToggle.isOn = true;
		JoinToggle.isOn = false;
	}

	public void StartButton()
	{
		
	}
}
