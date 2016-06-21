using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XboxCtrlrInput;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public class GameManager : MonoBehaviour {

	// Make this class a singleton
	public static GameManager instance = null;

	// Player related variables
	public GameObject[] playerPrefabs;
	public GameObject[] playerMonuments;
	public GameObject[] playerScoreBoards;
	public Text[] PlayerScoreText;

	// Parent placeholders
	private Transform PlayerHolder;
	private Transform MonumentHolder;
	private Transform ScoreBoardHolder;

	// Accesing the controllers
	private XboxController[] controller =
		{
			XboxController.First,
			XboxController.Second,
			XboxController.Third,
			XboxController.Fourth
		};

	// Others //
	private AudioSource source;
	public Image ControlImage;
	public Image WinnerImage;
	public Text WinnerText;
	public Text CountdownText;
	public AudioClip[] clip;
	public Font font;

	public float TimeToShowControls = 5f;
	public float TimeBetweenCountdowns = 0.8f;

	private bool bCanPause = false;
	private bool bIsPaused = false;
	private int iNumControllers;

	private float fSoundWaitTime = 1.0f;
	private bool bHasChangedWinnerText = false;

	// Use this for initialization
	void Awake() {

		if (instance == null) {

			instance = this;

		}
		else if (instance != this) {

			Destroy(gameObject);

		}

		//DontDestroyOnLoad(gameObject);
		source = GetComponent<AudioSource>();

		iNumControllers = XCI.GetNumPluggedCtrlrs();
		Debug.Log("There are " + iNumControllers + " Xbox controllers plugged in.");

		// Determine how many controllers are connected and create the players based on this ammount.
		playerSetup();

		StartCoroutine(StartingScreen(TimeToShowControls));

	}

	void Update() {

		// Pause the game
		bool bPause1 = XCI.GetButtonDown(XboxButton.Start, controller[0]);
		bool bPause2 = XCI.GetButtonDown(XboxButton.Start, controller[1]);
		bool bPause3 = XCI.GetButtonDown(XboxButton.Start, controller[2]);
		bool bPause4 = XCI.GetButtonDown(XboxButton.Start, controller[3]);
		if (bCanPause && ( bPause1 || bPause2 || bPause3 || bPause4 )) {

			bIsPaused = !bIsPaused;

			source.PlayOneShot(clip[0]);

			if (bIsPaused == true) {

				GameObject.Find("Pause Screen").GetComponent<CanvasGroup>().alpha = 1;
				Time.timeScale = 0;

			}
			else {
				GameObject.Find("Pause Screen").GetComponent<CanvasGroup>().alpha = 0;
				Time.timeScale = 1;
			}

		}

		// Restart the match
		if (Input.GetKeyDown(KeyCode.R)) {

			#if UNITY_5_3_OR_NEWER
			SceneManager.LoadScene(1);
			#else
				Application.LoadLevel(1);
			#endif
		
		}

		UpdateScores();

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

	private void UpdateScores() {

		int index = 0;

		foreach (Transform child in MonumentHolder) {

			MonumentScript ms = child.gameObject.GetComponent<MonumentScript>();

			string value = "0";
			if (ms.iTotalCrates < 10)
				value += Mathf.Ceil(ms.iTotalCrates).ToString();
			else
				value = Mathf.Ceil(ms.iTotalCrates).ToString();

			PlayerScoreText[index].text = value;
			index++;

			if (ms.iTotalCrates >= 15) {

				if (!bHasChangedWinnerText) {

					string tempName = ms.sprites[0].name;
					if (tempName[0] == 'R') {

						WinnerText.text += "Rodrigo";

					}
					else if (tempName[0] == 'A') {

						WinnerText.text += "A4";

					}
					else if (tempName[0] == 'J') {

						WinnerText.text += "J3FF";

					}
					else {

						WinnerText.text += "Bob The Blob";

					}

					bHasChangedWinnerText = true;

				}

				StartCoroutine(GameOver());

			}

		}

	}

	/// Countdowns ///
	private IEnumerator GameOver() {

		Time.timeScale = 0;

		source.Stop();

		yield return StartCoroutine(WaitForRealSeconds(1));

		WinnerImage.gameObject.SetActive(true);
		WinnerText.gameObject.SetActive(true);

		yield return StartCoroutine(WaitForRealSeconds(5));

		#if UNITY_5_3_OR_NEWER
		SceneManager.LoadScene(2);
		#else
		Application.LoadLevel(2);
		#endif


	}

	private IEnumerator StartCountdown(float delay) {

		CountdownText.gameObject.SetActive(true);

		source.PlayOneShot(clip[1]);
		CountdownText.GetComponent<Text>().text = "3";
		yield return StartCoroutine(WaitForRealSeconds(delay));

		source.PlayOneShot(clip[1]);
		CountdownText.GetComponent<Text>().text = "2";
		yield return StartCoroutine(WaitForRealSeconds(delay));

		source.PlayOneShot(clip[1]);
		CountdownText.GetComponent<Text>().text = "1";
		yield return StartCoroutine(WaitForRealSeconds(delay));

		source.PlayOneShot(clip[2]);
		CountdownText.GetComponent<Text>().text = "Go!";
		yield return StartCoroutine(WaitForRealSeconds(delay));

		CountdownText.GetComponent<Text>().text = "";

		CountdownText.gameObject.SetActive(false);

	}

	private IEnumerator StartingScreen(float delay) {

		Time.timeScale = 0;

		CountdownText.gameObject.SetActive(false);
		ControlImage.gameObject.SetActive(true);

		yield return StartCoroutine(WaitForRealSeconds(delay));

		ControlImage.gameObject.SetActive(false);

		yield return StartCoroutine(StartCountdown(TimeBetweenCountdowns));

		Time.timeScale = 1;

		source.Play();

		bCanPause = true;

	}

	IEnumerator WaitForRealSeconds(float delay) {

		float waitTime = Time.realtimeSinceStartup + delay;
		yield return new WaitWhile(() => Time.realtimeSinceStartup < waitTime);

	}

}
