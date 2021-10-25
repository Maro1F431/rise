using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{

	public float Lifetime;

	private float speed;

	private Vector3 startpos;

	private int damage = 100;

	private float color;

	public Renderer TrailRenderer;

	public Renderer BulletRenderer;

	private double timeOfCreation;

	private int ownerID;

	public GameObject prefabToCreate;

	private bool isBoss;



	// Use this for initialization
	void Start()
	{
		if (PhotonNetwork.connected)
		{
			timeOfCreation = PhotonNetwork.time;
		}
		else
		{
			timeOfCreation = Time.time;
		}
		startpos = transform.position;
	}

	public void Setup(double timestamp, int damage, Color color, float speed, int id)
	{
		this.damage = damage;
		if(BulletRenderer)
			BulletRenderer.material.color = color;
		if(TrailRenderer)
			TrailRenderer.material.color = color;
		this.speed = speed;
		timeOfCreation = timestamp;
		ownerID = id;
		isBoss = false;

	}

	public void Setup(int damage, float speed) // for the boss
	{
		this.damage = damage;
		this.speed = speed;
		isBoss = true;

	}


	// Update is called once per frame
	void Update()
	{
//problem here, it doesnt work offline
		if (PhotonNetwork.connected)
		{
			if ((PhotonNetwork.time - timeOfCreation) >= Lifetime)
            		{
            			End();
            		}
			else
			{
	
				float timepassed = (float) (PhotonNetwork.time - timeOfCreation);
				Vector3 newPos = startpos + transform.forward * speed * timepassed;
				RaycastHit hit;
				if (Physics.Raycast(transform.position, newPos, out hit, Vector3.Distance(transform.position, newPos)))
				{
					if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Bullet"))
					{
						transform.position = newPos;
					}
					else if (hit.transform.gameObject.tag == "Player")
					{
						var player = hit.transform.gameObject.GetComponent<PlayerController>();
						if (!player.isDead && !player.Dashing && !player.SameID(ownerID))
						{
							player.Damage(damage); //only work locally , for the one being touched
                            End();		
						}
						else
						{
							transform.position = newPos;
						}
						// The function Damage(int damage) is a Method of PlayerController, it has to be implemented 
					}
					else if (hit.transform.gameObject.tag == "Boss")
					{
						var Boss = hit.transform.gameObject.GetComponent<Boss_Script>();
						if (!Boss.IsDead && !isBoss)
						{
							Boss.Damage(damage); //only work locally , for the one being touched
							End();
						}
						else
						{
							transform.position = newPos;
						}
					}
					else
					{
						End();
					}
				}
				else
				{
					transform.position = newPos;
				}
			}

		}
		else
		{
			if ((Time.time - timeOfCreation) >= Lifetime)
			{
				End();
			}
			else
			{
	
				float timepassed = (float) (Time.time - timeOfCreation);
				Vector3 newPos = startpos + transform.forward * speed * timepassed;
				RaycastHit hit;
				if (Physics.Raycast(transform.position, newPos, out hit, Vector3.Distance(transform.position, newPos)))
				{
					if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Bullet"))
					{
						transform.position = newPos;
					}
					else if (hit.transform.gameObject.tag == "Player")
					{
						var player = hit.transform.gameObject.GetComponent<PlayerController>();
						if (!player.isDead && !player.Dashing )
						{
							player.Damage(damage); //only work locally , for the one being touched
                            End();
						}
						else
						{
							transform.position = newPos;
						}
						// The function Damage(int damage) is a Method of PlayerController, it has to be implemented 
					}
					else if (hit.transform.gameObject.tag == "Boss")
					{
						var Boss = hit.transform.gameObject.GetComponent<Boss_Script>();
						if (!Boss.IsDead && !isBoss)
						{
							Boss.Damage(damage); //only work locally , for the one being touched
							End();
						}
						else
						{
							transform.position = newPos;
						}
					}
					else
					{
						End();
					}
				}
				else
				{
					transform.position = newPos;
				}
			}
		}
		

}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			var player = other.gameObject.GetComponent<PlayerController>();
			if (!player.isDead && !player.Dashing && !player.SameID(ownerID))
			{
				player.Damage(damage); //only work locally , for the one being touched
				End();
			}
			/* The function Damage(int damage) is a Method of PlayerController, it has to be implemented */
			
		}
		else if (other.gameObject.tag == "Boss")
		{
			var Boss = other.transform.parent.GetComponent<Boss_Script>();
			if (!Boss.IsDead && !isBoss)
			{
				Boss.Damage(damage); //only work locally , for the one being touched
				End();
			}
		}
		else
		{
			End();
		}
		
	}

	private void End()
	{
		Instantiate(prefabToCreate, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
