using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

	public new Camera camera;

	public float ShakeDuration = 1;
	public float ShakeFactor = 0.7f;
	public float DecreaseShakeFactor = 1.0f;
	public float cameraSizeFactor = 0.5f;
	public bool ResetCameraSize = false;

	private CameraResize crScript;
	private Vector3 originalCameraPosition;
	private float fOriginalCameraWidth;

	void Awake() {

		crScript = camera.gameObject.GetComponent<CameraResize>();
		originalCameraPosition = camera.gameObject.GetComponent<Camera>().transform.localPosition;
		fOriginalCameraWidth = crScript.fWidth;

	}

	// Update is called once per frame
	void Update() {

		if (ShakeDuration > 0) {

			crScript.fWidth -= cameraSizeFactor;
			GetComponent<Camera>().transform.localPosition = GetComponent<Camera>().transform.localPosition + Random.insideUnitSphere / 2 * ShakeFactor;
			ShakeDuration -= (DecreaseShakeFactor * Time.deltaTime);

		}
		else {

			if (ResetCameraSize)
				crScript.fWidth = fOriginalCameraWidth;

			GetComponent<Camera>().transform.localPosition = originalCameraPosition;
			ShakeDuration = 0.0f;

		}

	}

}