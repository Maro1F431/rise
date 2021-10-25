using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeEvents : MonoBehaviour
{

	public List<EventAndLoc> EventList;

	public List<TimeStamp> StampList;

	private InGameUI ui;

	public SpawnManager SpawnManager;

	public GameObject EventPrefab;

	public Animator EventMessage;

	private GameObject lastEvent;
	
	public void SetList(List<Vector2> list)
	{
		EventList = new List<EventAndLoc>();
		for (int i = 0; i < list.Count; i++)
		{
			EventAndLoc enl = new EventAndLoc();
			enl.Event = Global.Instance.EventList[(int)list[i].x];
			enl.Trans = SpawnManager.EventSpawn.SpawnList[(int)list[i].y].transform;
			EventList.Add(enl);
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		EventList = new List<EventAndLoc>();
		ui = GetComponent<InGameUI>();
	}


	
	// Update is called once per frame
	void Update ()
	{
		TimeStamp stamp = new TimeStamp();
		if (StampList.Count > 0)
		{
			stamp = StampList[0];
		}
		if (ui.time <= 0 && StampList.Count == 0)
		{
			Debug.Log("Called Go to crafting");
			StartCoroutine(ui.GoToCrafting(2));
		}
		if (StampList.Count > 0 && EventList.Count > 0 && ui.time <= stamp.minute * 60 + stamp.second)
		{
			var Event = EventList[0];
			
			StampList.RemoveAt(0);
			EventList.RemoveAt(0);

			if (lastEvent)
			{
				Destroy(lastEvent);
				lastEvent = null;
			}
			lastEvent = Instantiate(EventPrefab, Event.Trans.position, Quaternion.Euler(Event.Trans.rotation.eulerAngles + new Vector3(0,-90,0)));
			lastEvent.GetComponent<EventPrefab>().chest.GetComponent<Chest>().Event = Event.Event;
			
			EventMessage.SetTrigger("anim");
		}
		
	}
	

}
