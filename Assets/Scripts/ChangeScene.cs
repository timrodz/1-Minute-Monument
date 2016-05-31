using UnityEngine;
using System.Collections;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public class ChangeScene : MonoBehaviour {
	
	// Update is called once per frame
	public void ChangeToScene(string scene) {

		#if UNITY_5_3_OR_NEWER
		SceneManager.LoadScene(scene);
		#else
    Application.LoadLevel(scene);
		#endif

	}

	public void Quit() {
		Application.Quit();
	}

}