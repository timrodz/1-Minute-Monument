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
	private XboxController[] controller =
		{
		XboxController.First,
		XboxController.Second,
		XboxController.Third,
		XboxController.Fourth
	};

	private AudioSource source;
	private float waitTime;

	private bool bHasFinishedFadingIn = false;
	private bool bHasFinishedFadingOut = false;
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

			// The first fade-in
			if (cg.alpha < 1 && !bHasFinishedFadingIn)
				cg.alpha += ( 5 * Time.deltaTime );

			if (cg.alpha == 1)
				bHasFinishedFadingIn = true;

			// Process the start button
			if (bHasFinishedFadingIn) {

				bool bStart1 = XCI.GetButtonDown(XboxButton.Start, controller[0]);
				bool bStart2 = XCI.GetButtonDown(XboxButton.Start, controller[1]);
				bool bStart3 = XCI.GetButtonDown(XboxButton.Start, controller[2]);
				bool bStart4 = XCI.GetButtonDown(XboxButton.Start, controller[3]);

				if (bStart1 || bStart2 || bStart3 || bStart4) {

					source = GetComponent<AudioSource>();

					source.PlayOneShot(select);
					waitTime = select.length;
					bHasPressedStart = true;

				}

				if (bHasPressedStart) {

					GameObject.Find("Menu Manager").gameObject.GetComponent<AudioSource>().Stop();
					//source.Stop();

					rt.sizeDelta += new Vector2(400 * ( Time.deltaTime ), 400 * ( Time.deltaTime ));
					//rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y + (200 * Time.deltaTime), rt.localPosition.z);

					if (cg.alpha > 0 && !bHasFinishedFadingOut) {

						cg.alpha -= ( 3 * Time.deltaTime );

					}

					//if (rt.localPosition.y >= 100)
					if (rt.sizeDelta.x > 700)
						bHasFinishedFadingOut = true;

					if (bHasFinishedFadingOut) {

						ChangeToScene(1);

					}

				}

			}

		}

	}

	// Update is called once per frame
	private void ChangeToScene(int scene) {

		#if UNITY_5_3_OR_NEWER
		SceneManager.LoadScene(scene);
		#else
		    Application.LoadLevel(scene);
		#endif

	}

}