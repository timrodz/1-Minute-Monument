using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

	public float ShakeDuration = 0;
	public float ShakeFactor = 0.7f;
	public float DecreaseShakeFactor = 1.0f;
	public float cameraSizeFactor = 0.5f;

	private CameraSize cameraSizeScript;

	private float originalCameraSize;
	private Vector3 originalCameraPosition;

	void Awake() {

		GameObject obj = GameObject.Find("Main Camera");
		cameraSizeScript = obj.gameObject.GetComponent<CameraSize>();

		originalCameraSize = cameraSizeScript.fWidth;
		originalCameraPosition = GetComponent<Camera>().transform.localPosition;

	}
	
	// Update is called once per frame
	void Update () {

		if (ShakeDuration > 0) {
			cameraSizeScript.fWidth -= cameraSizeFactor;
			GetComponent<Camera>().transform.localPosition = GetComponent<Camera>().transform.localPosition + Random.insideUnitSphere * ShakeFactor;
			ShakeDuration -= Time.deltaTime * DecreaseShakeFactor;
		}
		else {
			//cameraSizeScript.fWidth = originalCameraSize;
			GetComponent<Camera>().transform.localPosition = originalCameraPosition;
			ShakeDuration = 0.0f;
		}

	}

}