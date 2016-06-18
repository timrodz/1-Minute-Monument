using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

public class PlayerMovement : MonoBehaviour {

	/// MEMBER VARIABLES ///

	//public BoardManager boardScript;
	public XboxController controller;

	// Accesing the collision check object
	public BoxCollider2D collisionCheck;

	// Accesing physics
	public Rigidbody2D rb;

	// Animation controller
	public Animator anim;

	// Physics related variables //

	// The speed with which the player will move
	private float maxSpeed = 12f;
	public float fSpeedMod = 0.3f;

	// The force that will be applied to the speed
	float moveForce = 300f;

	float fX;
	float fY;
	float fSpeedH;
	float fSpeedV;
	bool bIsFacingRight;

	Vector3 newPosition;

	// Crate holding variables //
	public GameObject[] crateHolder;
	public int iCratesHeld = 0;
	public int iTotalCrates = 0;
	bool bIsDroppingCrates = false;

	/// MEMBER FUNCTIONS ///

	// This happens before spawning the object
	void Awake() {

		collisionCheck = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

	}

	// Invoked after the object spawns
	void Start() {

		iTotalCrates = 0;
		iCratesHeld = 0;

		bIsFacingRight = true;

		// We don't flip to the right because that's the default direction
		if (transform.localPosition.x > 13)
			Flip();

	}

	// Update is called once per frame
	// Moving non-physics objects
	// Simple timers
	// Receiving input
	void Update() {

		// Getting updated xy position
		fX = transform.position.x;
		fY = transform.position.y;

		print(rb.velocity);

	}

	// TODO: FIX THE MOVEMENT

	// Called every phyics step
	// Adjusting rigidbody objects
	void FixedUpdate() {

		bIsDroppingCrates = XCI.GetButtonUp(XboxButton.B, controller);

		// Left stick movement
		newPosition = transform.position;

		float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
		float axisY = XCI.GetAxis(XboxAxis.LeftStickY, controller);

		float newPosX = newPosition.x + (axisX * maxSpeed * Time.deltaTime);
		//float newPosZ = newPosition.z + (axisY * maxSpeed * Time.deltaTime);
		float newPosY = newPosition.y + (axisY * maxSpeed * Time.deltaTime);
		newPosition = new Vector3(newPosX, newPosY, -1);
		transform.position = newPosition;

		//// Obtain left stick values
		//fSpeedH = XCI.GetAxis(XboxAxis.LeftStickX, controller);
		//fSpeedV = XCI.GetAxis(XboxAxis.LeftStickY, controller);

		//// Add the horizontal movement force to the player
		//if ((fSpeedH * rb.velocity.x < maxSpeed)) {
		//	rb.AddForce(new Vector2(fSpeedH * moveForce, 0f));
		//}

		//// Add the vertical movement force to the player
		//if ((fSpeedV * rb.velocity.y < maxSpeed)) {
		//	rb.AddForce(new Vector2(0f, fSpeedV * moveForce));
		//}

		//// Accelerate horizontally until running at maximum speed
		//if (Mathf.Abs(rb.velocity.x) > maxSpeed) {
		//	rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
		//}

		//// Accelerate vertically until running at maximum speed
		//if (Mathf.Abs(rb.velocity.y) > maxSpeed) {
		//	rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * maxSpeed);
		//}

		//// Stop the horizontal sliding
		//if (rb.velocity.x != 0 && Mathf.Abs(fSpeedH) == 0) {
		//	rb.velocity = new Vector2(0f, rb.velocity.y);
		//}

		//// Stop the vertical sliding
		//if (rb.velocity.y != 0 && Mathf.Abs(fSpeedV) == 0) {
		//	rb.velocity = new Vector2(rb.velocity.x, 0f);
		//}

		// Flip the player's position according to their speed and direction
		if (fSpeedH < 0 && bIsFacingRight)
			Flip();
		else if (fSpeedH > 0 && !bIsFacingRight)
			Flip();

		// Dropping a crate
		if (bIsDroppingCrates && iCratesHeld > 0) {

			Instantiate(crateHolder[iCratesHeld], new Vector3(fX, fY, 0.0f), Quaternion.identity);
			maxSpeed += fSpeedMod;
			iCratesHeld--;

		}

	}

	// Reverse the facing position
	private void Flip() {

		bIsFacingRight = !bIsFacingRight;

		// We use a 3D vector because the world has 3D objects
		// Using a 2D vector would make this objects disappear
		Vector3 tempScale = transform.localScale;
		tempScale.x *= -1;
		transform.localScale = tempScale;

	}

	// Triggering events with either resources or bases
	private void OnTriggerEnter2D(Collider2D other) {

		if ((other.tag == "Resource") && (iCratesHeld < 5)) {

			// Destroy the collided crate
			Destroy(other.gameObject);

			// Add it to the current crateHolding vector

			iCratesHeld++;
			maxSpeed -= fSpeedMod;

		}

		// Dropping off crates - just for player one at the moment - will need to add base 1, 2, 3, 4 tags etc
		if ((other.tag == "Base1") && (XCI.GetButtonUp(XboxButton.B, controller))) {

			iTotalCrates += iCratesHeld;
			iCratesHeld = 0;
			maxSpeed = 5;

		}

		if ((other.tag == "Base2") && (XCI.GetButtonUp(XboxButton.B, controller))) {

			iTotalCrates += iCratesHeld;
			iCratesHeld = 0;
			maxSpeed = 5;

		}

		if ((other.tag == "Base3") && (XCI.GetButtonUp(XboxButton.B, controller))) {

			iTotalCrates += iCratesHeld;
			iCratesHeld = 0;
			maxSpeed = 5;

		}

		if ((other.tag == "Base4") && (XCI.GetButtonUp(XboxButton.B, controller))) {

			iTotalCrates += iCratesHeld;
			iCratesHeld = 0;
			maxSpeed = 5;

		}

	}

}