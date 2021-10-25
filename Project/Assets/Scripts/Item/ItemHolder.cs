using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{

	public Item item;
	public GameObject cube;
	public float waveMagnitude = 0.5f;
	public float waveSpeed = 1;
	public float rotateSpeed = 90;
	public float itemScale = 2f;
	private GameObject ItemObject;

	// Use this for initialization
	void Start () {
		if (item.Prefab)
		{
			if (cube)
			{
				Destroy(cube);
			}
			if (ItemObject)
			{
				Destroy(ItemObject);
			}
			ItemObject = Instantiate(item.Prefab, transform.position, transform.rotation);
			ItemObject.transform.parent = transform;
			var scale = ItemObject.transform.localScale;
			scale.x *= itemScale;
			scale.y *= itemScale;
			scale.z *= itemScale;
			ItemObject.transform.localScale = scale;
		}
	}

	public void SetItem(Item item)
	{
		this.item = item;
		if (item.Prefab)
		{
			if (cube)
			{
				Destroy(cube);
			}
			if (ItemObject)
			{
				Destroy(ItemObject);
			}
			
			ItemObject = Instantiate(item.Prefab, transform.position, transform.rotation);
			ItemObject.transform.parent = transform;
			var scale = ItemObject.transform.localScale;
			scale.x *= itemScale;
			scale.y *= itemScale;
			scale.z *= itemScale;
			ItemObject.transform.localScale = scale;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
		GameObject ObjectReference;

		if (item.Prefab)
		{
			ObjectReference = ItemObject;
		}
		else
		{
			ObjectReference = transform.GetChild(0).gameObject;
		}

		if (ObjectReference)
		{
			var temp = ObjectReference.transform.position;
			temp.y = Mathf.Sin(Time.time * waveSpeed) * waveMagnitude + transform.position.y;
			ObjectReference.transform.position = temp;
			ObjectReference.transform.Rotate(new Vector3(0,Time.deltaTime * rotateSpeed, 0));

		}
	}
	
	private void OnTriggerEnter(Collider other)
	{
        
		if (other.gameObject.tag == "Player")
		{
			PlayerController player = other.gameObject.GetComponent<PlayerController>();
			ItemCallbacks.Eval(player, item, item.FunctionToCall);
			if(item.IsResource)
			{
				player.AddItem(item);
			}
			Destroy(gameObject);
			
		}
	}
}
