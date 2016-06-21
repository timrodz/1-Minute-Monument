using UnityEngine;
using System.Collections;
using XboxCtrlrInput;
public class PlayerMovement : MonoBehaviour {

	/// MEMBER VARIABLES ///
	public XboxController controller;

	private Animator anim;

	// Attacking variables //
	private bool bIsPressingAttack = false;
	private bool bIsAttacking = false;
	private float fAttackZone = 2.5f;
	private float fAttackResetTimer = 0.5f;

	private Transform P1Position;
	private Transform P2Position;
	private Transform P3Position;
	private Transform P4Position;
	private string enemyString;

	// Physics variables //
	// The speed with which the player will move
	private float fSpeed = 10f;
	private float fMaxSpeed;
	private float fSpeedMod;

	// Knowing when to flip the player
	private bool bIsFacingRight;

	// Crate holding variables //
	public Transform crate;
	public AudioClip[] Clip;
	private int iCratesHeld = 0;
	private bool bIsDroppingCrates = false;

	// Others //
	private int iNumControllers;
	private AudioSource source;

	/// MEMBER FUNCTIONS ///
	// This happens before spawning the object
	void Awake() {

		source = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		iNumControllers = XCI.GetNumPluggedCtrlrs();

	}

	// Invoked after the object spawns
	void Start() {

		fMaxSpeed = fSpeed;
		fSpeedMod = fMaxSpeed / 10;
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
		bIsPressingAttack = XCI.GetButtonDown(XboxButton.A, controller);

		/// WORKING ON THIS
		// A timer to reset the attack
		if (bIsPressingAttack) {

			if (fAttackResetTimer <= 0) {

				print("Restting attack");
				fAttackResetTimer = 0.3f;

			}

		}

		if (iNumControllers > 0)
			P1Position = GameObject.FindGameObjectWithTag("P1").transform;
		if (iNumControllers > 1)
			P2Position = GameObject.FindGameObjectWithTag("P2").transform;
		if (iNumControllers > 2)
			P3Position = GameObject.FindGameObjectWithTag("P3").transform;
		if (iNumControllers > 3)
			P4Position = GameObject.FindGameObjectWithTag("P4").transform;

	}

	// Called every phyics step
	// Adjusting rigidbody objects
	void FixedUpdate() {

		// Left stick movement
		float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
		float axisY = XCI.GetAxis(XboxAxis.LeftStickY, controller);
		float newPosX = transform.position.x + (axisX * fSpeed * Time.deltaTime);
		float newPosY = transform.position.y + (axisY * fSpeed * Time.deltaTime);
		transform.position = new Vector3(newPosX, newPosY, transform.position.z);

		// Flip the player's position according to their speed and direction
		if (axisX < 0 && bIsFacingRight)
			Flip();
		else if (axisX > 0 && !bIsFacingRight)
			Flip();

		// Dropping a crate
		if (bIsDroppingCrates && iCratesHeld > 0) {

			print("Dropping");
			Instantiate(crate, new Vector3(transform.position.x, transform.position.y, -1.0f), Quaternion.identity);
			fSpeed += fSpeedMod;
			iCratesHeld--;

		}

		/// WORKING ON THIS
		if (bIsAttacking) {

			print("Currently attacking");
			fAttackResetTimer -= Time.deltaTime;

		}

		/// WORKING ON THIS
		if (fAttackResetTimer < 0) {

			fAttackResetTimer = 0.0f;
			print("Can attack now");
			bIsAttacking = false;

		}

		// Attacking
		if (!bIsAttacking && bIsPressingAttack && (iCratesHeld == 0)) {

			bIsAttacking = true;

			anim.SetBool("Attack", true);

			source.PlayOneShot(Clip[2]);

			// Determine which player you are next to
			if ((iNumControllers > 0) &&
				(Mathf.Abs(P1Position.position.x - transform.position.x) < fAttackZone) &&
				(Mathf.Abs(P1Position.position.y - transform.position.y) < fAttackZone) &&
				(this.tag != "P1")) {

				enemyString = "P1";
				print("Attacking P1");

			}

			if ((iNumControllers > 1) &&
				(Mathf.Abs(P2Position.position.x - transform.position.x) < fAttackZone) &&
				(Mathf.Abs(P2Position.position.y - transform.position.y) < fAttackZone) &&
				(this.tag != "P2")) {

				enemyString = "P2";
				print("Attacking P2");

			}

			if ((iNumControllers > 2) &&
				(Mathf.Abs(P3Position.position.x - transform.position.x) < fAttackZone) &&
				(Mathf.Abs(P3Position.position.y - transform.position.y) < fAttackZone) &&
				(this.tag != "P3")) {

				enemyString = "P3";
				print("Attacking P3");

			}

			if ((iNumControllers > 3) &&
				(Mathf.Abs(P4Position.position.x - transform.position.x) < fAttackZone) &&
				(Mathf.Abs(P4Position.position.y - transform.position.y) < fAttackZone) &&
				(this.tag != "P4")) {

				enemyString = "P4";
				print("Attacking P4");

			}

			if (enemyString != null) {

				source.PlayOneShot(Clip[3]);

				// Find the enemy object and instance of that object
				GameObject enemy = GameObject.FindGameObjectWithTag(enemyString);
				PlayerMovement enemyInstance = enemy.gameObject.GetComponent<PlayerMovement>();

				if (bIsFacingRight) {

					enemyInstance.transform.localPosition = new Vector3(enemyInstance.transform.localPosition.x + (30 * Time.deltaTime), enemyInstance.transform.localPosition.y, enemyInstance.transform.localPosition.z);

				}
				else {

					enemyInstance.transform.localPosition = new Vector3(enemyInstance.transform.localPosition.x - (30 * Time.deltaTime), enemyInstance.transform.localPosition.y, enemyInstance.transform.localPosition.z);

				}

				// If holding 5, lose 3
				if (enemyInstance.iCratesHeld == 5) {

					enemyInstance.iCratesHeld = 2;
					print("Enemy holding 5");
					// Spawn the 3 crates dropped
					Instantiate(crate, new Vector3(transform.position.x - 1, transform.position.y - 1, -1.0f), Quaternion.identity);
					enemyInstance.fSpeed += fSpeedMod;
					Instantiate(crate, new Vector3(transform.position.x, transform.position.y, -1.0f), Quaternion.identity);
					enemyInstance.fSpeed += fSpeedMod;
					Instantiate(crate, new Vector3(transform.position.x + 1, transform.position.y + 1, -1.0f), Quaternion.identity);
					enemyInstance.fSpeed += fSpeedMod;

				}
				// If holding 4, lose 2
				else if (enemyInstance.iCratesHeld == 4) {

					enemyInstance.iCratesHeld = 2;
					print("Enemy holding 4");
					// Spawn the 2 crates dropped
					Instantiate(crate, new Vector3(transform.position.x - 1, transform.position.y - 1, -1.0f), Quaternion.identity);
					enemyInstance.fSpeed += fSpeedMod;
					Instantiate(crate, new Vector3(transform.position.x + 1, transform.position.y + 1, -1.0f), Quaternion.identity);
					enemyInstance.fSpeed += fSpeedMod;

				}
				else if (enemyInstance.iCratesHeld > 0) // Else, lose 1  

				{
					enemyInstance.iCratesHeld -= 1;
					print("Enemy holding 3 or less");
					// Spawn the crate dropped
					Instantiate(crate, new Vector3(transform.position.x, transform.position.y, -1.0f), Quaternion.identity);
					enemyInstance.fSpeed += fSpeedMod;

				}

				enemyString = null;

			}

		}

		if (!bIsPressingAttack) {

			anim.SetBool("Attack", false);

		}

	}

	// Reverse the facing position
	private void Flip() {

		bIsFacingRight = !bIsFacingRight;
		Vector3 tempScale = transform.localScale;
		tempScale.x *= -1;
		transform.localScale = tempScale;

	}

	// Triggering events with either resources or bases or attacking
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

			if (iCratesHeld > 0)
				source.PlayOneShot(Clip[1]);

			monumentInstance.iTotalCrates += iCratesHeld;
			iCratesHeld = 0;
			fSpeed = fMaxSpeed;

		}
		else if (other.tag == "Base2" && this.tag == "P2") {

			MonumentScript monumentInstance = other.gameObject.GetComponent<MonumentScript>();

			if (iCratesHeld > 0)
				source.PlayOneShot(Clip[1]);

			monumentInstance.iTotalCrates += iCratesHeld;
			iCratesHeld = 0;
			fSpeed = fMaxSpeed;

		}
		else if (other.tag == "Base3" && this.tag == "P3") {

			MonumentScript monumentInstance = other.gameObject.GetComponent<MonumentScript>();

			if (iCratesHeld > 0)
				source.PlayOneShot(Clip[1]);

			monumentInstance.iTotalCrates += iCratesHeld;
			iCratesHeld = 0;
			fSpeed = fMaxSpeed;

		}
		else if (other.tag == "Base4" && this.tag == "P4") {

			MonumentScript monumentInstance = other.gameObject.GetComponent<MonumentScript>();

			if (iCratesHeld > 0)
				source.PlayOneShot(Clip[1]);

			monumentInstance.iTotalCrates += iCratesHeld;
			iCratesHeld = 0;
			fSpeed = fMaxSpeed;

		}

	}

}