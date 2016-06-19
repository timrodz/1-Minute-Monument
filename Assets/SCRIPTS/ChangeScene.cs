using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public class ChangeScene : MonoBehaviour {

	public AudioClip select;
	private AudioSource source;
	float waitTime;
	bool checkSceneChange = false;

	XboxController controller = XboxController.All;
	
	// Update is called once per frame
	public void ChangeToScene(string scene) {

		#if UNITY_5_3_OR_NEWER
		SceneManager.LoadScene(scene);
		#else
    Application.LoadLevel(scene);
		#endif

	}

	void Update() {

		if (XCI.GetButtonDown(XboxButton.Start, controller)) {

			source = GetComponent<AudioSource>();
			source.PlayOneShot(select);
			source.volume = 1;
			waitTime = select.length;
			checkSceneChange = true;

		}

		if (checkSceneChange) {

			waitTime -= Time.deltaTime;

		}

		if (waitTime < 0) {

			ChangeToScene("Game");

		}

	}

}