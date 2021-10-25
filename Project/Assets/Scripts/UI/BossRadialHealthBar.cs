using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRadialHealthBar : MonoBehaviour
{
	private bool linked;
	public int ID;

	public BossHealthOnlineSync bossHealth;
	private ProgressBar bar;
	
	// Use this for initialization
	void Start ()
	{
		linked = false;
		bar = GetComponent<ProgressBar>();
	}

	public void Useless()
	{
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
		bar.currentPercent = bossHealth.GetHealth(ID);
	}
}
