﻿using UnityEngine;
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

	private List<Vector3> gridPosition = new List<Vector3>();

	// Setting up the scene
	public void SetupScene() {

		for (int i = 0; i < players.Length; i++) {

			if (0 == i)
				SpawnObjectAt(players, i, 2, 2);
			if (1 == i)
				SpawnObjectAt(players, i, rows - 3, columns - 3);
			if (2 == i)
				SpawnObjectAt(players, i, rows - 3, i);
			if (3 == i)
				SpawnObjectAt(players, i, 2, columns - 3);

		}

		InitializeList();

		BoardSetup();

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



	private void SpawnObjectAt(GameObject[] _obj, int _index, int _x, int _y) {

		Vector3 position = new Vector3(_x, _y, 0);
		GameObject instance = _obj[_index];
		Instantiate(instance, position, Quaternion.identity);

	}

}