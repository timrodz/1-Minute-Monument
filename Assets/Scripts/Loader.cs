using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	public GameObject gameManager;
	//public GameObject player;

	// Use this for initialization
	void Awake() {

		if (GameManager.instance == null) {

			Instantiate(gameManager);

		} else {

			Instantiate(gameManager);

		}

	}

}