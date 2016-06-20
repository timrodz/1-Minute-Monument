using UnityEngine;
using System.Collections;

public class ShakeObject : MonoBehaviour {

	public float ShakeDuration = 0.5f;
	public float ShakeFactor = 0.7f;

	private float DecreaseShakeFactor = 1.0f;

	private RectTransform rt;
	private Vector3 originalPosition;
	private bool bHasFinishedShaking = false;

	// Use this for initialization
	void Awake () {

		rt = GetComponent<RectTransform>();
		originalPosition = rt.localPosition;

	}
	
	// Update is called once per frame
	void Update () {

		if (ShakeDuration > 0) {

			rt.localPosition = rt.localPosition + (Random.insideUnitSphere * ShakeFactor);
			ShakeDuration -= (DecreaseShakeFactor * Time.deltaTime);

		} 
		else {

			if (!bHasFinishedShaking) {

				rt.localPosition = originalPosition;
				bHasFinishedShaking = true;

			}

			ShakeDuration = 0.0f;

		}
	
	}

}