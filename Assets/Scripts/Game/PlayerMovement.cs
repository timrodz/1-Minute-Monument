using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public BoardManager boardScript;

	private float xPosition;
	private float yPosition;

	// Accesing the collision check object
	public BoxCollider2D collisionCheck;

	// Accesing physics
	public Rigidbody2D rb;

	// Animation controller
	public Animator anim;

	// The speed with which the player will move
	public float maxSpeed = 5f;

	// The force that will be applied to the speed
	public float moveForce = 300f;

	// Input
	private float speedH;
	private float speedV;

	// To distinguish between player 1 and 2 movement
	private string horiztonalCtrl = "";
	// defaults to player 1
	private string verticalCtrl = "";
	// defaults to player 1
	private string drop = "";
	// defaults to player 1

	[HideInInspector]
	public string facingDirection;

	// For resource collecting
	public GameObject[] crates;
	public int CratesHeld = 0;
	public int TotalCrates = 0;

	// Accesing the board's size
	int rows, cols;

	// This happens before spawning the object
	void Awake() {

		collisionCheck = GetComponent<BoxCollider2D>();
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		GameObject obj = GameObject.Find("GameManager");
		boardScript = obj.GetComponent<BoardManager>();
//		boardScript = obj;

		rows = boardScript.rows;
		cols = boardScript.columns;

	}

	// Invoked after the object spawns
	void Start() {
		
		TotalCrates = 0;
		CratesHeld = 0;

		// P1
		if (transform.localPosition.x == 2 && transform.localPosition.y == 2) {
			horiztonalCtrl = "Horizontal_P1";
			verticalCtrl = "Vertical_P1";
			drop = "Drop1";

		}
		// P2
		else if (transform.localPosition.x == cols - 3 && transform.localPosition.y == rows - 3) {
			horiztonalCtrl = "Horizontal_P2";
			verticalCtrl = "Vertical_P2";
			drop = "Drop2";
		}
		// P3
		else if (transform.localPosition.x == cols - 3 && transform.localPosition.y == 2) {
			horiztonalCtrl = "Horizontal_P3";
			verticalCtrl = "Vertical_P3";
			drop = "Drop3";
		}
		// P4
		else if (transform.localPosition.x == 2 && transform.localPosition.y == rows - 3) {
			horiztonalCtrl = "Horizontal_P4";
			verticalCtrl = "Vertical_P4";
			drop = "Drop4";
		}

		facingDirection = "right";

		// We don't flip to the right because that's the default direction
		if (transform.localPosition.x == 13)
			Flip();

	}

	// Update is called once per frame
	// Moving non-physics objects
	// Simple timers
	// Receiving input
	void Update() {

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

		if (speedV >= 0.75) {
			speedV = 1;
		}
		else if (speedV <= -0.75) {
			speedV = -1;
		}
		else {
			speedV = 0;
		}

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

		// Dropping crates
		if (Input.GetButtonDown(drop) && CratesHeld > 0) {
			CratesHeld--;
			maxSpeed -= 0.3f;
			Instantiate(crates[0], new Vector3(xPosition - 2, yPosition, 0.0f), Quaternion.identity);
		}

	}

	// Tyring to delay the trigger so the crate is not immediately picked back up
	//IEnumerator OnTriggerEnter(Collider collider)
	//{
	//    if (collider.tag == "Player")
	//    {
	//        collider.enabled = false;

	//        yield return new WaitForSeconds(2);

	//        collider.enabled = true;
	//    }
	//}


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
		if (( other.tag == "Resource" ) && ( CratesHeld < 5 )) {
			Destroy(other.gameObject);

			CratesHeld++;
			maxSpeed -= 0.3f;
		}

		// Dropping off crates - just for player one at the moment - will need to add base 1, 2, 3, 4 tags etc
		if (( other.tag == "Base1" ) && ( drop == "Drop1" )) { 
//			print("Player 1 dropping crates");
			TotalCrates += CratesHeld;
			CratesHeld = 0;
			maxSpeed = 5;
		}

		if (( other.tag == "Base2" ) && ( drop == "Drop2" )) {
//			print("Player 2 dropping crates");
			TotalCrates += CratesHeld;
			CratesHeld = 0;
			maxSpeed = 5;
		}

		if (( other.tag == "Base3" ) && ( drop == "Drop3" )) {
//			print("Player 3 dropping crates");
			TotalCrates += CratesHeld;
			CratesHeld = 0;
			maxSpeed = 5;
		}

		if (( other.tag == "Base4" ) && ( drop == "Drop4" )) {
//			print("Player 4 dropping crates");
			TotalCrates += CratesHeld;
			CratesHeld = 0;
			maxSpeed = 5;
		}

	}
}