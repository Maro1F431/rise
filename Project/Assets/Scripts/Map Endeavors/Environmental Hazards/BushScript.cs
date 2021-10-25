using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushScript : MonoBehaviour {
	
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>().IsPlayer())
		{
			var ren = GetComponent<Renderer>();
			if (ren)
			{
				StandardShaderUtils.ChangeRenderMode(ren.material, StandardShaderUtils.BlendMode.Fade);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>().IsPlayer())
		{
			var ren = GetComponent<Renderer>();
			if (ren)
			{
				StandardShaderUtils.ChangeRenderMode(ren.material, StandardShaderUtils.BlendMode.Cutout);
			}
		}
	}
}
