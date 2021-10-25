using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotController : MonoBehaviour
{

	private PlayerController player;
	
	public float IdleTimeMin;
	public float IdleTimeMax;
	private float IdleTime;
	
	public float WanderTimeMin;
	public float WanderTimeMax;
	private float WanderTime;
	private float WanderAngle;
	
	public float ChaseRadius;
	public float ChaseInnerRadius;
	public float ChaseTimeout;
	private GameObject ChaseTarget;

	private string State;
	
	private double StartTime;
	
	private Vector3 input;
	
	
	// Use this for initialization
	void Start ()
	{
		State = "Idle";
		player = GetComponent<PlayerController>();
		Vector3 input = Vector3.zero;
		IdleTime = Random.Range(IdleTimeMin, IdleTimeMax);
		StartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (!player.isBot || player.isDead)
		{
			return;
		}

		GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
		Transform[] playerTransforms = new Transform[Players.Length - 1];
		GameObject[] OtherPlayers = new GameObject[Players.Length - 1];
		int offset = 0;
		for (int i = 0; i < Players.Length; i++)
		{
			if (Players[i] != gameObject)
			{
				playerTransforms[i - offset] = Players[i].GetComponent<Transform>();
				OtherPlayers[i - offset] = Players[i];	
			}
			else
			{
				offset++;
			}
		}
		
		if (State == "Idle")
		{
			input = Vector3.zero;
			if (StartTime + IdleTime <= Time.time)
			{
				WanderAngle = Random.Range(0, 360);
				WanderTime = Random.Range(WanderTimeMin, WanderTimeMax);
				State = "Wander";
				StartTime = Time.time;
			}
		}
		else if (State == "Wander")
		{
			input = new Vector3(Mathf.Cos(WanderAngle), 0, Mathf.Sin(WanderAngle));
			if (StartTime + WanderTime <= Time.time)
			{
				IdleTime = Random.Range(IdleTimeMin, IdleTimeMax);
				StartTime = Time.time;
				State = "Idle";
			}
		}
		else if (State == "Chase")
		{
			var pl = ChaseTarget.GetComponent<Transform>();
			if (Vector3.Distance(transform.position, pl.position) < ChaseRadius && Vector3.Distance(transform.position, pl.position) > ChaseInnerRadius)
			{
				StartTime = Time.time;
				input = pl.position - transform.position;
				transform.LookAt(pl.position);
			}
			if (Vector3.Distance(transform.position, pl.position) < ChaseInnerRadius)
			{
				State = "Shoot";
			}
			if (StartTime + ChaseTimeout <= Time.time)
			{
				IdleTime = Random.Range(IdleTimeMin, IdleTimeMax);
				State = "Idle";
			}
		}
		else if (State == "Shoot")
		{
			var pl = ChaseTarget.GetComponent<Transform>();
			if (Vector3.Distance(transform.position, pl.position) < ChaseInnerRadius)
			{
				input = Vector3.zero;
				StartTime = Time.time;
				transform.LookAt(pl.position);
				player.isFiring = true;
			}
			else
			{
				player.isFiring = false;
				State = "Chase";
			}
		}
		if (State == "Idle" || State == "Wander")
		{
			for (int i = 0; i < playerTransforms.Length; i++)
			{
				var pl = playerTransforms[i];
				if (Vector3.Distance(transform.position, pl.position) < ChaseRadius)
				{
					ChaseTarget = OtherPlayers[i];
					State = "Chase";
				}
			}
		}
		player.SetInput(input.normalized);
	}
}
