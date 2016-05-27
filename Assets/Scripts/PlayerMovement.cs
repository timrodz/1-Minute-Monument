using UnityEngine;
using System.Collections;

#if UNITY_5_3_3
using UnityEngine.SceneManagement;
#endif

public class PlayerMovement : MonoBehaviour {

	public BoardManager boardScript;

	private float xPosition;
	private float yPosition;

	// Accesing the collision check object
	public BoxCollider2D collisionCheck;

	public Rigidbody2D rb;
	// Accesing physics
	public Animator anim;
	// Animation controller

	public float maxSpeed = 5f;
	// The speed with which the player will move
	public float moveForce = 300f;
	// The force that will be applied to the speed

	private bool isColliding;
	// Determining collision

	// Input
	private float speedH;
	private float speedV;

	// To distinguish between player 1 and 2 movement
	private string horiztonalCtrl = "";
	// defaults to player 1
	private string verticalCtrl = "";
	// defaults to player 1

	[HideInInspector]
	public string facingDirection;

	// For resource collecting
	public int CratesHeld;

	//public int P1CratesHeld;
	//public int P2CratesHeld;
	//public int P3CratesHeld;
	//public int P4CratesHeld;

	// This happens before spawning the object
	void Awake() {
		
		collisionCheck = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

	}

	// Invoked after the object spawns
	void Start() {

		// P1
		if (transform.localPosition.x == 2 && transform.localPosition.y == 2) {
			horiztonalCtrl = "Horizontal_P1";
			verticalCtrl = "Vertical_P1";
		}
		// P2
		else if (transform.localPosition.x == 13 && transform.localPosition.y == 13) {
			horiztonalCtrl = "Horizontal_P2";
			verticalCtrl = "Vertical_P2";
		}
		// P3
		else if (transform.localPosition.x == 13 && transform.localPosition.y == 2) {
			horiztonalCtrl = "Horizontal_P3";
			verticalCtrl = "Vertical_P3";
		}
		// P4
		else if (transform.localPosition.x == 2 && transform.localPosition.y == 13) {
			horiztonalCtrl = "Horizontal_P4";
			verticalCtrl = "Vertical_P4";
		}

		facingDirection = "right";

		// We don't flip to the right because that's the default direction
//		if (transform.localPosition.x == 13) {
//		}
		Flip();

	}

	// Update is called once per frame
	// Moving non-physics objects
	// Simple timers
	// Receiving input
	void Update() {

		// THIS DOESN'T WORK YET
		// Just helps deciding what to do after the player collides (can be deprecated)
		isColliding = Physics2D.Linecast(transform.position,
			collisionCheck.transform.position,
			1 << LayerMask.NameToLayer("Collision")); // Bitwise shift to check if there's an intersection with the "Collision" layer

		if (isColliding == true) {
			print("Colliding!");
		}

		// Getting updated xy position
		xPosition = transform.position.x;
		yPosition = transform.position.y;


	}

	// Called every phyics step
	// Adjusting rigidbody objects
	void FixedUpdate() {

		// Receiving both inputs
		speedH = Input.GetAxisRaw(horiztonalCtrl);
		speedV = Input.GetAxisRaw(verticalCtrl);

		// Add the horizontal movement force to the player
		if (( speedH * rb.velocity.x < maxSpeed )) {
			rb.AddForce(new Vector2(speedH * moveForce, 0f));
		}

		// Add the vertical movement force to the player
		if (( speedV * rb.velocity.y < maxSpeed )) {
			rb.AddForce(new Vector2(0f, speedV * moveForce));
		}

		// Accelerate horizontally until running at maximum speed
		if (Mathf.Abs(rb.velocity.x) > maxSpeed) {
			rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
		}

		// Accelerate vertically until running at maximum speed
		if (Mathf.Abs(rb.velocity.y) > maxSpeed) {
			rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * maxSpeed);
		}

		// Stop the horizontal sliding
		if (rb.velocity.x != 0 && Mathf.Abs(speedH) == 0) {
			rb.velocity = new Vector2(0f, rb.velocity.y);
		}

		// Stop the vertical sliding
		if (rb.velocity.y != 0 && Mathf.Abs(speedV) == 0) {
			rb.velocity = new Vector2(rb.velocity.x, 0f);
		}

		// Flip the player's position according to their speed and direction
		if (speedH < 0 && facingDirection == "right")
			Flip();
		else if (speedH > 0 && facingDirection == "left")
			Flip();

	}

	// Reverse the facing position
	private void Flip() {
		
		if (facingDirection == "right")
			facingDirection = "left";
		else
			facingDirection = "right";

		// We use a 3D vector because the world has 3D objects
		// Using a 2D vector would make this objects disappear
		Vector3 tempScale = transform.localScale;
		tempScale.x *= -1;
		transform.localScale = tempScale;

	}

	// Resource collecting
	private void OnTriggerEnter2D(Collider2D other) {
        
		if (other.tag == "Resource") {
			Destroy(other.gameObject);

			CratesHeld++;
			maxSpeed -= 0.5f;
  
		}

	}

	// For player 1 only, as they spawn at 2,2
	private void DropOffCrates() {
		
		if (( ( xPosition > 1.8f ) && ( xPosition < 2.2f ) ) &&
		    ( ( yPosition > 1.8f ) && ( yPosition < 2.2f ) )) {

			CratesHeld = 0;
			maxSpeed = 5;
		
		}

	}

}