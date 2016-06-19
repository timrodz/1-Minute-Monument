using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

	[HideInInspector]
	public bool bCanBePickedUp = false;

	private float fTimeAfterSpawn = 0.0f;

	// Update is called once per frame
	void Update() {

		if (fTimeAfterSpawn < 1) {
			fTimeAfterSpawn += Time.fixedDeltaTime;
		}
		else {
			bCanBePickedUp = true;
		}

	}

}