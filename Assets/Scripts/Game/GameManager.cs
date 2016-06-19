using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XboxCtrlrInput;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif


public class GameManager : MonoBehaviour {

	// Singleton
	public static GameManager instance = null;

	public Font font;

	// Player related variables
	public GameObject[] playerPrefabs;
	public GameObject[] playerMonuments;
	public GameObject[] playerScoreBoards;
	public Text[] PlayerScoreText;

	private Transform PlayerHolder;
	private Transform MonumentHolder;
	private Transform ScoreBoardHolder;
	public Transform CanvasHolder;

	// Accesing the controllers
	private XboxController[] controller = {
		XboxController.First,
		XboxController.Second,
		XboxController.Third,
		XboxController.Fourth
	};

	// Others //
	private bool paused = false;
	private int iNumControllers;

	// Use this for initialization
	void Awake() {

		if (instance == null) {

			instance = this;

		}
		else if (instance != this) {

			Destroy(gameObject);

		}

		DontDestroyOnLoad(gameObject);

		iNumControllers = XCI.GetNumPluggedCtrlrs();
		Debug.Log("There are " + iNumControllers + " Xbox controllers plugged in.");

		// Determine how many controllers are connected and create the players based on this ammount.
		playerSetup();

	}

	private void playerSetup() {

		PlayerHolder = new GameObject("Players").transform;
		MonumentHolder = new GameObject("Monuments").transform;
		ScoreBoardHolder = new GameObject("Score Board").transform;

		for (int i = 0; i < iNumControllers; i++) {

			/// Player Prefabs ///
			// Spawn the players in the positions their prefabs are set to
			Vector3 playerPosition = new Vector3(playerPrefabs[i].gameObject.transform.position.x, playerPrefabs[i].gameObject.transform.position.y, playerPrefabs[i].gameObject.transform.position.z);

			// Instantiate the player prefabs
			GameObject playerObj = playerPrefabs[i];
			playerObj = Instantiate(playerPrefabs[i], playerPosition, Quaternion.identity) as GameObject;
			playerObj.transform.SetParent(PlayerHolder);

			/// Player Monuments ///
			// Spawn the monuments in the positions their prefabs are set to
			Vector3 monumentPosition = new Vector3(playerMonuments[i].gameObject.transform.position.x, playerMonuments[i].gameObject.transform.position.y, playerMonuments[i].gameObject.transform.position.z);

			// Instantiate the monument prefabs
			GameObject monumentObj = playerMonuments[i];
			monumentObj = Instantiate(playerMonuments[i], monumentPosition, Quaternion.identity) as GameObject;
			monumentObj.transform.SetParent(MonumentHolder);

			/// Player Score Board ///
			// Spawn the score board images in the positions their prefabs are set to
			Vector3 scoreBoardPosition = new Vector3(playerScoreBoards[i].gameObject.transform.position.x, playerScoreBoards[i].gameObject.transform.position.y, playerScoreBoards[i].gameObject.transform.position.z);

			// Instantiate the score board prefabs
			GameObject playerScoreBoardObj = playerScoreBoards[i];
			playerScoreBoardObj = Instantiate(playerScoreBoards[i], scoreBoardPosition, Quaternion.identity) as GameObject;
			playerScoreBoardObj.transform.SetParent(ScoreBoardHolder);

		}

		// Disable the score text based on the player number
		switch (iNumControllers) {
			case 1:
				GameObject.Find("A4 Score Text").SetActive(false);
				GameObject.Find("Bob The Blob Score Text").SetActive(false);
				GameObject.Find("J3FF Score Text").SetActive(false);
				break;
			case 2:
				GameObject.Find("Bob The Blob Score Text").SetActive(false);
				GameObject.Find("J3FF Score Text").SetActive(false);
				break;
			case 3:
				GameObject.Find("J3FF Score Text").SetActive(false);
				break;
			default:
				// 4 players
				break;
		}

	}

	void Update() {

		// Pause the game
		//bool bControllerPause = XCI.GetButtonDown(XboxButton.Start, controller);
		bool bPause1 = XCI.GetButtonDown(XboxButton.Start, controller[0]);
		bool bPause2 = XCI.GetButtonDown(XboxButton.Start, controller[1]);
		bool bPause3 = XCI.GetButtonDown(XboxButton.Start, controller[2]);
		bool bPause4 = XCI.GetButtonDown(XboxButton.Start, controller[3]);
		if (bPause1 || bPause2 || bPause3 || bPause4) {

			paused = !paused;

			if (paused == true)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;

		}

		// Update the text

		// Restart the match
		if (Input.GetKeyDown(KeyCode.R)) {

#if UNITY_5_3_OR_NEWER
			SceneManager.LoadScene("Game");
#else
			Application.LoadLevel("Game");
#endif
		}

	}

	// Function to display a GUI on runtime
	void OnGUI() {

		GUI.skin.font = font;

		if (paused) {

			GUILayout.BeginArea(new Rect(0, (Screen.height / 2) - 50, Screen.width, Screen.height));
			var centeredStyle = GUI.skin.GetStyle("Label");
			centeredStyle.alignment = TextAnchor.UpperCenter;
			GUILayout.Label(" Game is paused");
			GUILayout.Label(" Press START to unpause");
			GUILayout.EndArea();

		}

	}

	public void GameOver() {

		enabled = false;

	}



}