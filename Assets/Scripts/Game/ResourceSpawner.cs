using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ResourceSpawner : MonoBehaviour {

	private Transform resourceHolder;

	public GameObject[] resourceTiles;

	public float waitTime = 1;

	int lastRow, lastCol;
	int currentRow, currentCol;
	int countRow, countCol;

	// Use this for initialization
	void Start() {

		resourceHolder = new GameObject("Resources").transform;

		GameObject boardObject = GameObject.Find("GameManager");
		BoardManager boardScript = boardObject.GetComponent<BoardManager>();

		countRow = boardScript.rows;
		countCol = boardScript.columns;

		StartCoroutine(Spawner());

	}

	// Update is called once per frame
	void Update() {



	}

	private IEnumerator Spawner() {

		while (true) {

			yield return new WaitForSeconds(waitTime);

			currentRow = Random.Range(countRow / 4, countRow / 2 + countRow / 4);
			currentCol = Random.Range(countCol / 4, countCol / 2 + countCol / 4);

			print(Physics.CheckSphere(new Vector3(currentCol, currentRow, -1), 40));

			if (Physics.CheckSphere(new Vector3(currentCol, currentRow, -1), 4)) {

				print("Overlapping at " + currentCol + " - " + currentRow);

			}

			lastRow = currentRow;
			lastCol = currentCol;

			SpawnObjectAt(resourceTiles, Random.Range(0, resourceTiles.Length), currentCol, currentRow);

		}

	}

	private void SpawnObjectAt(GameObject[] _obj, int _index, int _x, int _y) {

		Vector3 position = new Vector3(_x, _y, -1);
		GameObject instance = _obj[_index];
		GameObject ins = Instantiate(instance, position, Quaternion.identity) as GameObject;
		ins.transform.SetParent(resourceHolder);

	}

}