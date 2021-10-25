using System.Collections.Generic;
using ProBuilder2.Common;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	public GameObject player;

	public GameObject Player
	{
		get { return player; }
		set
		{
			player = value; 
			targetLerp = player.GetComponent<Transform>().position;
		}
	}
	public float distance;
	public Vector2 angle;
	private Vector3 offset;
	public float cameraRadiusLim;
	public float smoothness;
	public float alphaSmoothness;
	public float alphaStart;
	public float alphaTarget;
	private Vector3 targetLerp;
	//private GameObject lastGameObject;
	private List<GameObject> lastGameObjects;
	private float lastAlpha;
	
	// Update is called once per frame
	private void Start()
	{
		offset = Vector3.zero;
		//lastGameObject = null;
		lastGameObjects = new List<GameObject>();
		
	}

	/*void CameraRaycast(float maxRange)
	{
		RaycastHit hit;
		if (Physics.Raycast(player.transform.position, (transform.position - player.transform.position), out hit, maxRange))
		{
			if (hit.transform == transform)
			{
				if (lastGameObject != null)
				{
					var ren = lastGameObject.GetComponent<Renderer>();
					var color = ren.material.color;
					color.a = 1;
					lastGameObject.GetComponent<Renderer>().material.color = color;
					StandardShaderUtils.ChangeRenderMode(ren.material, StandardShaderUtils.BlendMode.Opaque);		
					lastGameObject = null;
				}
				
			}
			else if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Fade Out"))
			{
				lastGameObject = hit.transform.gameObject;
				var ren = lastGameObject.GetComponent<Renderer>();
				StandardShaderUtils.ChangeRenderMode(ren.material, StandardShaderUtils.BlendMode.Fade);
				var color = ren.material.color;
				if (color.a == 1)
				{
					color.a = alphaStart;
				}
				color.a = Mathf.Lerp(color.a, alphaTarget, alphaSmoothness);
				lastGameObject.GetComponent<Renderer>().material.color = color;
			}
		}
	}*/

	void CameraRaycastAll(float maxRange)
	{
		
		RaycastHit[] hits;
		hits = Physics.RaycastAll(player.transform.position, (transform.position - player.transform.position),maxRange);
		if (hits.Length <= 1) //Has no object, or has only camera
		{
			if(lastGameObjects.Count > 0) 
			{
				//Has objects that were processed, so make them all opaque and remove them all from the list
				for (int i = 0; i < lastGameObjects.Count; i++)
				{
					RemoveFromStack(i);
				}
			}
		}
		else
		{
			List<int> common = new List<int>();
			foreach (var hit in hits)
			{
				GameObject hitObj = hit.transform.gameObject;
				
				//Check that object is not camera and is in layer Fade out
				if (hitObj != gameObject && (hit.transform.gameObject.layer == LayerMask.NameToLayer("Fade Out")))
				{
					//Check if object is already in the object stack. If it's not, add it.
					var stackPlace = IsInLastGameObjects(hitObj);
					if (stackPlace != -1)
					{
						common.Add(stackPlace);
					}
					else
					{
						common.Add(lastGameObjects.Count);
						lastGameObjects.Add(hitObj);
					}
					
					//Process object
					var ren = hitObj.GetComponent<Renderer>();
					if (ren)
					{
						StandardShaderUtils.ChangeRenderMode(ren.material, StandardShaderUtils.BlendMode.Fade);
						var color = ren.material.color;
						if (color.a == 1)
						{
							color.a = alphaStart;
						}
						color.a = Mathf.Lerp(color.a, alphaTarget, alphaSmoothness);
						hitObj.GetComponent<Renderer>().material.color = color;
					}
				}
			}
			//Clear lastgameobjects from objects that are not in the list anymore
			ClearFromObjects(common);
		}
	}

	void ClearFromObjects(List<int> common)
	{
		common.Sort();
		int i = 0;
		while (common.Count > 0)
		{
			int cur = common[0];
			if (i == cur)
			{
				i++;
				common.RemoveAt(0);
			}
			else
			{
				RemoveFromStack(i);
				i++;
			}
		}
	}

	int IsInLastGameObjects(GameObject other)
	{
		for (int i = 0; i < lastGameObjects.Count; i++)
		{
			var obj = lastGameObjects[i];
			if (obj == other)
			{
				return i;
			}
		}
		return -1;
	}

	void RemoveFromStack(int id)
	{
		GameObject obj = lastGameObjects[id];
		var ren = obj.GetComponent<Renderer>();
		var color = ren.material.color;
		color.a = 1;
		obj.GetComponent<Renderer>().material.color = color;
		StandardShaderUtils.ChangeRenderMode(ren.material, StandardShaderUtils.BlendMode.Opaque);
		lastGameObjects.RemoveAt(id);
	}

	void Update ()
	{
		if (!player || player.GetComponent<PlayerController>().isDead) return;
		float dist = distance;
		float maxRange = Vector3.Distance(player.transform.position, transform.position);
		
		//CameraRaycast(maxRange);
		CameraRaycastAll(maxRange);

		offset = new Vector3(
			dist * Mathf.Sin(Mathf.Deg2Rad * angle.x) * Mathf.Cos(Mathf.Deg2Rad * angle.y), 
			dist * Mathf.Cos(Mathf.Deg2Rad * angle.x), 
			dist * Mathf.Sin(Mathf.Deg2Rad * angle.x) * Mathf.Sin(Mathf.Deg2Rad * angle.y));
		Vector3 playerP = player.GetComponent<Transform>().position;
		Vector3 mousePos = player.GetComponent<PlayerController>().mousPos;
		Vector3 target = middle(playerP, mousePos);
		if (Vector3.Distance(playerP,mousePos) > cameraRadiusLim)
		{
			Vector3 clamp = (mousePos-playerP).normalized * cameraRadiusLim;
			target = middle(playerP, playerP + clamp);
		}
		targetLerp = Vector3.Lerp(targetLerp, target , smoothness);
		transform.position = targetLerp + offset;
		transform.LookAt(targetLerp);
	}

	Vector3 middle(Vector3 A, Vector3 B)
	{
		return Vector3.Lerp(A, B, 0.5f);
	}
}
