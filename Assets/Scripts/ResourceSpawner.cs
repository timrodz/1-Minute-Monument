using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ResourceSpawner : MonoBehaviour {

	private Transform resourceHolder;

	public GameObject[] resourceTiles;

	public float waitTime;

	private float startTime;
	private float elapsedTime;

	int lastRow, lastCol;
	int currentRow, currentCol;
	int countRow, countCol;

	private List<Vector3> gridPosition = new List<Vector3>();

	// Use this for initialization
	void Start() {

		resourceHolder = new GameObject("Resources").transform;
		
		startTime = Time.unscaledTime;
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

			while (currentRow == lastRow && currentCol == lastCol) {

				print("WHOOPS");

				currentRow = Random.Range(countRow / 4, countRow / 2 + countRow / 4);
				currentCol = Random.Range(countCol / 4, countCol / 2 + countCol / 4);

			}

			lastRow = currentRow;
			lastCol = currentCol;

			SpawnObjectAt(resourceTiles, Random.Range(0, resourceTiles.Length), currentRow, currentCol);


		}



	}

	private void SpawnObjectAt(GameObject[] _obj, int _index, int _x, int _y) {

		Vector3 position = new Vector3(_x, _y, 0);
		GameObject instance = _obj[_index];
		GameObject ins = Instantiate(instance, position, Quaternion.identity) as GameObject;
		ins.transform.SetParent(resourceHolder);

	}

}