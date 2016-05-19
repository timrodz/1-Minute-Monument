using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
	
	// Update is called once per frame
	public void ChangeToScene(string scene) {

		SceneManager.LoadScene(scene);

	}

	public void Quit() {
		Application.Quit();
	}

}