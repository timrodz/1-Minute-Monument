using UnityEngine;
using System.Collections;

public class MonumentScript : MonoBehaviour {

	public Sprite[] sprites;
	public SpriteRenderer sp;
	public BoxCollider2D bc;

	[HideInInspector]
	public int iTotalCrates;

	private AudioSource source;
	private bool hasPlayedSound = false;

	// Use this for initialization
	void Awake() {

		source = GetComponent<AudioSource>();
		sp = GetComponent<SpriteRenderer>();
		bc = GetComponent<BoxCollider2D>();

	}

	// Update is called once per frame
	void Update() {

		if (iTotalCrates < 5) {

			sp.sprite = sprites[0];
			Vector3 v = sp.bounds.size;
			bc.size = v;

		}
		else if (iTotalCrates >= 5 && iTotalCrates < 15) {

			if (!hasPlayedSound) {
				source.Play();
				hasPlayedSound = !hasPlayedSound;
			}
			sp.sprite = sprites[1];
			Vector3 v = sp.bounds.size;
			bc.size = v;

		}
		else if (iTotalCrates == 15) {

			if (hasPlayedSound) {
				source.Play();
				hasPlayedSound = !hasPlayedSound;
			}
			sp.sprite = sprites[2];
			Vector3 v = sp.bounds.size;
			bc.size = v;

		}

	}
}
