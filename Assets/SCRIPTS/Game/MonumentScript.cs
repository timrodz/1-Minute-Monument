using UnityEngine;
using System.Collections;

public class MonumentScript : MonoBehaviour {

	public Sprite[] sprites;
	public SpriteRenderer sp;
	public BoxCollider2D bc;

	public int TotalCrates;

	// Use this for initialization
	void Start () {

		sp = GetComponent<SpriteRenderer>();
		bc = GetComponent<BoxCollider2D>();
	
	}
	
	// Update is called once per frame
	void Update () {

		if (TotalCrates < 5) {

			sp.sprite = sprites[0];
			Vector3 v = sp.bounds.size;
			bc.size = v;

		}
		else if (TotalCrates < 10) {

			sp.sprite = sprites[1];
			Vector3 v = sp.bounds.size;
			bc.size = v;

		}
		else if (TotalCrates == 15) {

			sp.sprite = sprites[2];
			Vector3 v = sp.bounds.size;
			bc.size = v;

		}
	
	}
}
