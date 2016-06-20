using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class CrateSpawner : MonoBehaviour {

	private Transform resourceHolder;

	public Crate crate;

	public float waitTime = 1.0f;

	public int Rows, Columns;

	public float StartingPositionX, StartingPositionY;


	private List<Vector3> gridPosition = new List<Vector3>();
	private int gridIndex;

	// Use this for initialization
	void Start() {

		InitializeList();

		resourceHolder = new GameObject("Crates").transform;

		StartCoroutine(Spawner());

	}

	private IEnumerator Spawner() {

		while (true) {

			yield return new WaitForSeconds(waitTime);

			if (gridPosition.Count > 0) {

				Crate instance = Instantiate(crate, RandomPosition(), Quaternion.identity) as Crate;
				instance.transform.SetParent(resourceHolder);

			}
			else {

				//InitializeList();

			}

		}

	}

	// Initializing the list of positions
	private void InitializeList() {

		gridPosition.Clear();

		for (int x = 0; x < Columns; x++) {

			for (int y = 0; y < Rows; y++) {

				gridPosition.Add(new Vector3(x + StartingPositionX, y + StartingPositionY, -1f));

			}

		}

	}

	// Spawning instances at random positions
	private Vector3 RandomPosition() {

		// Get a random number (returns 2 values, in this case x and y)
		gridIndex = Random.Range(0, gridPosition.Count);
		// Create a vector based on the retrieved position
		Vector3 randomPos = gridPosition[gridIndex];
		// Remove that position because it's being now used
		gridPosition.RemoveAt(gridIndex);
		return randomPos;

	}

	// Reset the empty positions once they've been taken
	public void ResetEmptyPositions() {

		for (int i = 0; i < gridPosition.Count; i++) {
			
			for (int x = 0; x < Columns; x++) {

				for (int y = 0; y < Rows; y++) {

					if (gridPosition.IndexOf(new Vector3(x + StartingPositionX, y + StartingPositionY, -1f)) == -1) {

						// Position is clear
						print("Clear position at " + x + ", " + y);

					}

				}

			}
			
		}

	}

}