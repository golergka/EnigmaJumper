using UnityEngine;
using System.Collections;

public class VisitGridGenerator : MonoBehaviour {

	public Transform visitGridCell;

	void Awake() {

		if (!visitGridCell) {

			Debug.LogWarning("Visit grid cell is not set up!");
			enabled = false;

		}

	}

	public int gridSize = 10;
	public float cellSize = 10f;

	// Use this for initialization
	void Start () {

		for(int x=0; x < gridSize; x++) {
			for(int y=0; y < gridSize; y++) {

				Vector3 cellPosition = transform.position;

				cellPosition.x += cellSize * (x - gridSize/2);
				cellPosition.z += cellSize * (y - gridSize/2);

				Instantiate(visitGridCell, cellPosition, Quaternion.identity);

			}
		}
	
	}

}
