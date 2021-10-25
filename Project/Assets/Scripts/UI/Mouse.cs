using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{
	public static Mouse Instance;
	public List<ProgressBar> Sliders;

	public ProgressBar RadialSlider;
	// Use this for initialization
	void Start ()
	{
		Instance = this;
		foreach (ProgressBar slider in Sliders)
		{
			slider.currentPercent = 100;
		}
		RadialSlider.currentPercent = 100;
	}

	private void Update()
	{
		transform.position = Input.mousePosition;
	}

	public void Roll(float duration)
	{
		RadialSlider.currentPercent = 0;
		RadialSlider.speed = 100 * Time.deltaTime / duration;
		//StartCoroutine(RefillRadial(duration));
	}

	/*IEnumerator RefillRadial(float duration)
	{
		while (RadialSlider.currentValue < 100)
		{
			RadialSlider.currentValue += duration * Time.deltaTime;
			yield return null;
		}
	}*/

	public void Shoot(float duration)
	{
		foreach (ProgressBar slider in Sliders)
		{
			slider.currentPercent = 0;
			slider.speed = 100 * Time.deltaTime / duration;
		}
		//StartCoroutine(RefillSlider(duration));
	}

	/*IEnumerator RefillSlider(float duration)
	{
		while (Sliders[0].currentPercent < 100)
		{
			foreach (ProgressBar slider in Sliders)
			{
				slider.currentPercent += duration * Time.deltaTime;
			}
			yield return null;
		}
	}*/
}
