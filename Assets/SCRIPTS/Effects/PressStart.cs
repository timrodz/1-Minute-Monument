using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XboxCtrlrInput;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public class PressStart : MonoBehaviour {

	public AudioClip select;

	// Accesing the controllers
	private XboxController[] controller = {
		XboxController.First,
		XboxController.Second,
		XboxController.Third,
		XboxController.Fourth
	};

	private AudioSource source;
	private float waitTime;
	private bool bHasPressedStart = false;

	// Animation variables

	private CanvasGroup cg;
	private ZoomIn zoomInObject;
	private RectTransform rt;

	void Awake() {

		// Make the object invisible
		cg = GetComponent<CanvasGroup>();
		cg.alpha = 0;

		GameObject logo = GameObject.Find("Logo");
		rt = logo.gameObject.GetComponent<RectTransform>();
		zoomInObject = logo.gameObject.GetComponent<ZoomIn>();

	}

	void Update() {

		if (zoomInObject.bHasFinishedZoomingIn) {

			if (cg.alpha < 1) {

				cg.alpha += (3 * Time.deltaTime);

			} else {

				bool bStart1 = XCI.GetButtonDown(XboxButton.Start, controller[0]);
				bool bStart2 = XCI.GetButtonDown(XboxButton.Start, controller[1]);
				bool bStart3 = XCI.GetButtonDown(XboxButton.Start, controller[2]);
				bool bStart4 = XCI.GetButtonDown(XboxButton.Start, controller[3]);
				if (bStart1 || bStart2 || bStart3 || bStart4) {

					source = GetComponent<AudioSource>();
					source.PlayOneShot(select);
					source.volume = 1;
					waitTime = select.length;
					bHasPressedStart = true;

				}

				if (bHasPressedStart) {

					if (rt.localPosition.y <= 80)
						rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y + (100 * Time.deltaTime), rt.localPosition.z);

				}

				if (waitTime < 0) {



				}

			}

			

		}

	}

	// Update is called once per frame
	private void ChangeToScene(string scene) {

#if UNITY_5_3_OR_NEWER
		SceneManager.LoadScene(scene);
#else
    Application.LoadLevel(scene);
#endif

	}

}