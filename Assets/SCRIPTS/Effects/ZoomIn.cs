using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ZoomIn : MonoBehaviour {

	public Image ImageToZoomIn;

	//public AudioClip[] clip;
	public float multiplier = 2;
	public float firstWidth, firstHeight;
	public float finalWidth, finalHeight;

	private RectTransform rt;
	private AudioSource source;

	[HideInInspector]
	public bool bHasFinishedZoomingIn = false;

	// Use this for initialization
	void Awake() {
		
		source = GetComponent<AudioSource>();
		rt = ImageToZoomIn.GetComponent<RectTransform>();
		rt.sizeDelta = new Vector2(firstWidth, firstHeight);

	}

	// Update is called once per frame
	void Update() {

		if (!bHasFinishedZoomingIn && finalWidth <= rt.rect.width && finalHeight <= rt.rect.height) {

			rt.sizeDelta -= new Vector2(firstWidth / multiplier * ( Time.deltaTime ), firstHeight / multiplier * ( Time.deltaTime ));

		}
		else {

			bHasFinishedZoomingIn = true;

		}

	}
}
