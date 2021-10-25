using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossTimer : MonoBehaviour
{
	public GameObject lost;
	public GameObject won;
	public static BossTimer Instance;
	private Text Timer;
	private double offset;
	private double time;
	
	// Use this for initialization
	void Start ()
	{
		Instance = this;
		offset = Time.time;
		Timer = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		time = Global.Instance.BossTime*60 - Time.time + offset;
		if (time < 0) time = 0;
		string minutes = ((int)(time / 60)).ToString();
		minutes = (minutes.Length > 1 ? minutes : "0" + minutes);
		string seconds = ((int)(time % 60)).ToString();
		seconds = (seconds.Length > 1 ? seconds : "0" + seconds);
		Timer.text = minutes + ":" + seconds;

		if (time == 0 && !EndOfGame.end)
		{
			EndGame(false);
		}
	}

	public void EndGame(bool won)
	{
		StartCoroutine(End(won));
	}

	IEnumerator End(bool won)
	{
		EndOfGame.End(won, InfoHolder.Instance.PlayerInfos.Find(info => info.isMe).Color);
		if (won)
		{
			this.won.SetActive(true);
		}
		else
		{
			lost.SetActive(true);
		}
		yield return new WaitForSeconds(3);
		PhotonNetwork.Disconnect();
		SceneManager.LoadScene("MainMenu");
	}
}
