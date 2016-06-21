using UnityEngine;
using System.Collections;

public class moveUp : MonoBehaviour {

	private RectTransform rt;

	// Use this for initialization
	void Awake() {

		rt = GetComponent<RectTransform>();
	
	}
	
	// Update is called once per frame
	void Update() {

		rt.localPosition = new Vector2(rt.localPosition.x, rt.localPosition.y + ( 50 * Time.deltaTime ));
	
	}

}