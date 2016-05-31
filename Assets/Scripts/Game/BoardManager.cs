using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count {

		public Count(int _min, int _max) {

			min = _min;
			max = _max;

		}

		public int GetMin() {
			return min;
		}

		public int GetMax() {
			return max;
		}

		private int min, max;

	}

	// Variables
	private Transform boardHolder;

	public int index = 1;
	public int columns = 8;
	public int rows = 8;

	public Count wallCount = new Count(5, 9);
	public Count resourceCount = new Count(1, 5);

	public GameObject[] outerWallTiles;
	public GameObject[] resourceTiles;
	public GameObject[] floorTiles;
	public GameObject[] players;
	public GameObject[] bases;

	private List<Vector3> gridPosition = new List<Vector3>();

	public void SetIndex(int i) {
		index = i;
	}

	public int GetIndex() {
		return index;
	}

	// Creating the board
	private void BoardSetup() {

		// We create an empty object called "Room"
		boardHolder = new GameObject("Room").transform;

		// This loop iterates all of the map's grid positions
		// We extend the limits so that it creates the outer walls
		for (int x = -1; x < columns + 1; x++) {

			for (int y = -1; y < rows + 1; y++) {

				// This object grabs random floor tiles to instantiate
				GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

				// In this one we set up the borders of the map (so that players can't go out of bounds
				if (x == -1 || y == -1 || x == columns || y == rows)
					toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

				// We now call the Instantiate function to add our previous objects to the room
				GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;

				// For order's sake, we group them as childs of our previously created empty "Room" object
				instance.transform.SetParent(boardHolder);

			}
			// Player 1 base
			GameObject base1 = Instantiate(bases[0], new Vector3(1, 1, 0.0f), Quaternion.identity) as GameObject;
			base1.transform.SetParent(boardHolder);
			base1.tag = "Base1";

			// Player 2 base
			GameObject base2 = Instantiate(bases[0], new Vector3(columns - 2, rows - 2, 0.0f), Quaternion.identity) as GameObject;
			base2.transform.SetParent(boardHolder);
			base2.tag = "Base2";

			// Player 3 base
			GameObject base3 = Instantiate(bases[0], new Vector3(columns - 2, 1, 0.0f), Quaternion.identity) as GameObject;
			base3.transform.SetParent(boardHolder);
			base3.tag = "Base3";

			// Player 4 base
			GameObject base4 = Instantiate(bases[0], new Vector3(1, rows - 2, 0.0f), Quaternion.identity) as GameObject;
			base4.transform.SetParent(boardHolder);
			base4.tag = "Base4";
		}
	}

	// Initializing the list of positions
	private void InitializeList() {

		gridPosition.Clear();

		for (int x = 1; x < columns - 1; x++) {

			for (int y = 1; y < rows - 1; y++) {

				gridPosition.Add(new Vector3(x, y, 0.0f));

			}

		}

	}

	// Spawning instances at random positions
	private Vector3 RandomPosition() {

		// Get a random number (returns 2 values, in this case x and y)
		int randomIndex = Random.Range(0, gridPosition.Count);
		// Create a vector based on the retrieved position
		Vector3 randomPos = gridPosition[randomIndex];
		// Remove that position because it's being now used
		gridPosition.RemoveAt(randomIndex);
		return randomPos;

	}

	// Instances at random position
	private void SpawnObjectRandomly(GameObject[] _obj, int _min, int _max) {

		int ammountOfObjectsToSpawn = Random.Range(_min, _max);

		for (int i = 0; i < ammountOfObjectsToSpawn; i++) {

			Vector3 position = RandomPosition();
			GameObject instance = _obj[Random.Range(0, _obj.Length)];
			Instantiate(instance, position, Quaternion.identity);

		}

	}

	private void SpawnObjectAt(GameObject[] _obj, int _index, int _x, int _y) {

		Vector3 position = new Vector3(_x, _y, -5);
		GameObject instance = _obj[_index];
		Instantiate(instance, position, Quaternion.identity);

	}

	// Setting up the scene
	public void SetupScene() {

		for (int i = 0; i < players.Length; i++) {

			if (0 == i)
				SpawnObjectAt(players, i, 2, 2);
			if (1 == i)
				SpawnObjectAt(players, i, columns - 3, rows - 3);
			if (2 == i)
				SpawnObjectAt(players, i, columns - 3, i);
			if (3 == i)
				SpawnObjectAt(players, i, 2, rows - 3);

		}

		InitializeList();

		// Spawning the entities
		//SpawnObjectAt(wallTiles, wallCount.GetMin(), wallCount.GetMax());

		//	SpawnObjectRandomly(resourceTiles, resourceCount.GetMin(), resourceCount.GetMax());

//		SpawnObjectRandomly(resourceTiles, 6, 15);

		// Spawning the players
		BoardSetup();


	}

}