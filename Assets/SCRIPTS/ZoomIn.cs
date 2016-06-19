using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ZoomIn : MonoBehaviour {

	public Image PressStartImage;

	public AudioClip[] clip;
	public float multiplier = 2;
	public float firstWidth, firstHeight;
	public float finalWidth, finalHeight;

	private RectTransform rt;
	private AudioSource source;
	bool bHasFinishedZoomingIn = false;
	bool bHasFinishedPlayingZoomInClip = false;

	// Use this for initialization
	void Awake() {

		source = GetComponent<AudioSource>();
		rt = GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(firstWidth, firstHeight);

		source.PlayOneShot(clip[0]);
		source.PlayOneShot(clip[1]);
		source.PlayOneShot(clip[2]);
		source.PlayOneShot(clip[3]);

	}

	// Update is called once per frame
	void Update() {

		if (finalWidth + 30 <= rt.rect.width && finalHeight + 30 <= rt.rect.height) {

			rt.sizeDelta -= new Vector2(firstWidth / multiplier * (Time.deltaTime), firstHeight / multiplier * (Time.deltaTime));

		}
		else {
			bHasFinishedZoomingIn = true;
		}

		if (bHasFinishedZoomingIn && !bHasFinishedPlayingZoomInClip) {
			
			source.Play();

			//GameObject psi = GameObject.Find("Press Start");
			//PressStartImage = psi.gameObject.GetComponent<Image>();
			//PressStartImage
			//PressStartImage.SetA

			PressStartImage.gameObject.SetActive(true);

			bHasFinishedZoomingIn = false;
			bHasFinishedPlayingZoomInClip = true;

		}

	}
}
