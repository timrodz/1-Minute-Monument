using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// Singleton
	public static GameManager instance = null;

	public BoardManager boardScript;

	public Font font;

	private bool paused = false;

	// Use this for initialization
	void Awake() {

		if (instance == null) {

			instance = this;

		}
		else if (instance != this) {

			Destroy(gameObject);

		}
		
		DontDestroyOnLoad(gameObject);
		boardScript = GetComponent<BoardManager>();
		InitGame();	

	}

	void Update() {

		// Pause the game
		if (Input.GetButtonDown("Pause")) {

			paused = !paused;

			if (paused == true)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;

		}

		// Restart the match
		if (Input.GetKeyDown(KeyCode.R)) {

			SceneManager.LoadScene("Game");

		}

		// Return to main menu
		if (Input.GetKeyDown(KeyCode.X)) {

			Destroy(this);
			SceneManager.LoadScene("Menu");

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
			GUILayout.Label(" Press ESC / START to unpause");
			GUILayout.EndArea();

		} else {

			GUILayout.Label(" Press ESC /\n START to pause");
			GUILayout.Label("\n Press R to restart");
			GUILayout.Label("\n Press X to return");

		}

	}

	public void GameOver() {

		enabled = false;

	}

	private void InitGame() {
		
		boardScript.SetupScene();

	}

	

}