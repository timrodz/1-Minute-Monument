using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

	//[HideInInspector]
	public bool bCanBePickedUp = false;

	private float fTimeAfterSpawn;

	// Use this for initialization
	void Start() {


	}

	// Update is called once per frame
	void Update() {

		if (fTimeAfterSpawn < 1)
			fTimeAfterSpawn += Time.fixedDeltaTime;
		else
			bCanBePickedUp = true;

	}

}