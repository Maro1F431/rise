using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthOnlineSync : MonoBehaviour
{
	private Dictionary<int, float> BossHealth;

	public List<BossRadialHealthBar> HealthBars;

	public Boss_Script LocalBoss;

	private int LocalID;

	private Vector3 colorVector3;

	private PhotonView View;
	
	private void Start()
	{
		View = GetComponent<PhotonView>();
		LocalID = View.viewID;
		BossHealth = new Dictionary<int, float>();
		int i = 0;
		if (InfoHolder.Instance)
		{
			/*foreach (PlayerInfo instancePlayerInfo in InfoHolder.Instance.PlayerInfos)
			{
				if (instancePlayerInfo.viewID != GetComponent<PhotonView>().viewID)
				{
					if (!instancePlayerInfo.isBot)
					{
						HealthBars[i].ID = instancePlayerInfo.viewID;
						i++;
					}
				}
			}*/
		}
		Debug.Log("trashing useless, reached " + i + " players");
		while (i < HealthBars.Count)
		{
			Debug.Log("delete " + i + "'s bar");
			HealthBars[i].Useless();
			i++;
		}
	}

	private void Update()
	{
		Debug.Log("is updating");
		Debug.Log(LocalBoss);
		View.RPC("UpdateHealth", PhotonTargets.All, LocalID, LocalBoss.healthBar.value);
	}

	public float GetHealth(int ID)
	{
		return BossHealth[ID];
	}

	[PunRPC]
	public void UpdateHealth(int ID, float health)
	{
		Debug.Log(health);
		BossHealth[ID] = health;
		Debug.Log(BossHealth[ID]);
	}
}
