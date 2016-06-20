using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	public AudioClip[] Sounds;

	private ZoomIn zoomInObject;
	private AudioSource source;

	private bool bHasPlayedSoundtrack = false;

	void Awake() {
		source = GetComponent<AudioSource>();

		//GameObject logo = GameObject.Find("Logo");
		//rt = logo.gameObject.GetComponent<RectTransform>();
		zoomInObject = GameObject.Find("Logo").gameObject.GetComponent<ZoomIn>();

	}

	// Use this for initialization
	void Start () {

		for (int i = 0; i < 4; i++) {
			source.PlayOneShot(Sounds[i]);
		}
		
	}
	
	// Update is called once per frame
	void Update () {

		if (zoomInObject.bHasFinishedZoomingIn && !bHasPlayedSoundtrack) {

			source.Play();
			bHasPlayedSoundtrack = true;

		}
	
	}

}