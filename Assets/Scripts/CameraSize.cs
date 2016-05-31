using UnityEngine;
using System.Collections;

public class CameraSize : MonoBehaviour {

	public float fWidth = 39.0f;

	public void LateUpdate() {
		// Orthographic size is 1/2 of the vertical size seen by the camera. 
		Camera.main.orthographicSize = fWidth * Screen.height / Screen.width * 0.5f;
	}

}