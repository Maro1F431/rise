using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

	public GameEvent Event;
	private bool canOpen;
	public bool open;
	private Animator anim;
	private PhotonView view;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			var player = other.gameObject.GetComponent<PlayerController>();

			if (player.IsPlayer())
			{
				canOpen = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			var player = other.gameObject.GetComponent<PlayerController>();

			if (player.IsPlayer())
			{
				canOpen = false;
			}
		}
	}
	
	[PunRPC]
	void Open()
	{
		open = true;
		anim.SetBool("open", true);
		anim.Play("Open");
		GetComponent<AudioManagerInstance>().Play("open");
		var itemList = Event.itemList;
		var coneAngle = 90;
		var coneRadius = 7;
		var offset = coneAngle / itemList.Count;
		for (int i = 0; i < itemList.Count; i++)
		{
			var itemSpawnAngle = 90 + transform.eulerAngles.y -coneAngle/2 + (i+1)*offset - offset/2;
			var itemObject = Resources.Load("ItemHolder", typeof(GameObject)) as GameObject;
			var itemPos = transform.position + new Vector3(Mathf.Cos(itemSpawnAngle * Mathf.Deg2Rad) * coneRadius, + itemObject.transform.localScale.y, Mathf.Sin(itemSpawnAngle * Mathf.Deg2Rad) * coneRadius);
			GameObject ItemHolder = Instantiate(itemObject, itemPos, Quaternion.identity);
			ItemHolder.GetComponent<ItemHolder>().SetItem(itemList[i]);
		}
	}
	// Use this for initialization
	void Start ()
	{
		canOpen = false;
		anim = GetComponent<Animator>();
		view = GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () {
		// TODO
		// Opens the chest.
		if (canOpen && !open)
		{
			view.RPC("Open", PhotonTargets.All);
		}
	}
}
