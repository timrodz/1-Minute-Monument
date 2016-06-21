using UnityEngine;
using System.Collections;

public class MonumentScript : MonoBehaviour {

	public Sprite[] sprites;

	private SpriteRenderer sp;
	private BoxCollider2D bc;

	[HideInInspector]
	public float iTotalCrates;

	private AudioSource source;
	private bool hasPlayedSound = false;

	private bool hasBeenUpgraded = false;

	// Use this for initialization
	void Awake() {

		source = GetComponent<AudioSource>();
		sp = GetComponent<SpriteRenderer>();
		bc = GetComponent<BoxCollider2D>();

	}

	void Start() {

		sp.sprite = sprites[0];

		ResetBoxCollider2D();

	}

	// Update is called once per frame
	void Update() {

		if (iTotalCrates >= 5 && iTotalCrates < 15) {

			if (!hasPlayedSound) {
				source.Play();
				hasPlayedSound = !hasPlayedSound;
			}

			if (!hasBeenUpgraded) {

				sp.sprite = sprites[1];
				ResetBoxCollider2D();
				hasBeenUpgraded = true;

			}

		}
		else if (iTotalCrates == 15) {

			if (hasPlayedSound) {
				source.Play();
				hasPlayedSound = !hasPlayedSound;
			}

			if (hasBeenUpgraded) {
				sp.sprite = sprites[2];
				ResetBoxCollider2D();
				hasBeenUpgraded = false;
			}

		}

	}

	private void ResetBoxCollider2D() {

		Destroy(bc);
		bc = gameObject.AddComponent<BoxCollider2D>();
		bc.isTrigger = true;

	}

}