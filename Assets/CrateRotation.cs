using UnityEngine;
using System.Collections;

public class CrateRotation : MonoBehaviour {

	public Transform body;

	// Use this for initialization
	void Start() {

		body = GetComponent<Transform>();
	
	}
	
	// Update is called once per frame
	void Update() {

//		body.localRotation = new Vector3
		body.Rotate(new Vector3(1, -1, -1) * -2);
	
	}
}
