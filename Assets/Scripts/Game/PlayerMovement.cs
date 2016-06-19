using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

public class PlayerMovement : MonoBehaviour {

	/// MEMBER VARIABLES ///
	//public BoardManager boardScript;
	public XboxController controller;

	// Animation controller
	private Animator anim;

	// Physics variables //

	// The speed with which the player will move
	private float fSpeed = 12f;
	private float fMaxSpeed;
	private float fSpeedMod;

	// Knowing when to flip the player
	private bool bIsFacingRight;

	// Crate holding variables //
	public Transform crate;
	public AudioClip[] Clip;
	private int iCratesHeld = 0;
	private bool bIsDroppingCrates = false;

	// Others
	private AudioSource source;

	/// MEMBER FUNCTIONS ///

	// This happens before spawning the object
	void Awake() {

		source = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();

	}

	// Invoked after the object spawns
	void Start() {

		fMaxSpeed = fSpeed;

		fSpeedMod = fMaxSpeed / 14;

		bIsFacingRight = true;

		// We don't flip to the right because that's the default direction
		if (transform.localPosition.x > 20) {
			Flip();
		}

	}

	// Update is called once per frame
	// Moving non-physics objects
	// Simple timers
	// Receiving input
	void Update() {

		bIsDroppingCrates = XCI.GetButtonDown(XboxButton.B, controller) || XCI.GetButtonDown(XboxButton.X, controller);

	}

	// Called every phyics step
	// Adjusting rigidbody objects
	void FixedUpdate() {

		// Left stick movement

		float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
		float axisY = XCI.GetAxis(XboxAxis.LeftStickY, controller);

		float newPosX = transform.position.x + (axisX * fSpeed * Time.deltaTime);
		float newPosY = transform.position.y + (axisY * fSpeed * Time.deltaTime);

		transform.position = new Vector3(newPosX, newPosY, -2.0f);

		// Flip the player's position according to their speed and direction
		if (axisX < 0 && bIsFacingRight)
			Flip();
		else if (axisX > 0 && !bIsFacingRight)
			Flip();

		// Dropping a crate
		if (bIsDroppingCrates && iCratesHeld > 0) {

			Instantiate(crate, new Vector3(transform.position.x, transform.position.y, -1.0f), Quaternion.identity);
			fSpeed += fSpeedMod;
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

		// Receiving a crate
		if ((other.tag == "Resource") && (iCratesHeld < 5)) {

			// Get the components of the crate instance
			Crate crateInstance = other.gameObject.GetComponent<Crate>();

			if (crateInstance.bCanBePickedUp) {

				source.PlayOneShot(Clip[0]);
				Destroy(other.gameObject);
				fSpeed -= fSpeedMod;
				iCratesHeld++;

			}

		}

		// Dropping off crates - just for player one at the moment - will need to add base 1, 2, 3, 4 tags etc
		if (other.tag == "Base1" && this.tag == "P1") {

			MonumentScript monumentInstance = other.gameObject.GetComponent<MonumentScript>();
			source.PlayOneShot(Clip[1]);
			monumentInstance.iTotalCrates += iCratesHeld;
			iCratesHeld = 0;
			fSpeed = fMaxSpeed;

		}
		else if (other.tag == "Base2" && this.tag == "P2") {

			MonumentScript monumentInstance = other.gameObject.GetComponent<MonumentScript>();
			source.PlayOneShot(Clip[1]);
			monumentInstance.iTotalCrates += iCratesHeld;
			iCratesHeld = 0;
			fSpeed = fMaxSpeed;

		}
		else if (other.tag == "Base3" && this.tag == "P3") {

			MonumentScript monumentInstance = other.gameObject.GetComponent<MonumentScript>();
			source.PlayOneShot(Clip[1]);
			monumentInstance.iTotalCrates += iCratesHeld;
			iCratesHeld = 0;
			fSpeed = fMaxSpeed;

		}
		else if (other.tag == "Base4" && this.tag == "P4") {

			MonumentScript monumentInstance = other.gameObject.GetComponent<MonumentScript>();
			source.PlayOneShot(Clip[1]);
			monumentInstance.iTotalCrates += iCratesHeld;
			iCratesHeld = 0;
			fSpeed = fMaxSpeed;

		}

	}

}