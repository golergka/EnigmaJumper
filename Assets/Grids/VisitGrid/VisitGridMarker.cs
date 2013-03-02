using UnityEngine;
using System.Collections;

public class VisitGridMarker : MonoBehaviour {

	public Material unvisitedMaterial;
	public Material visitedMaterial;

	public void SetVisited(bool visited) {

		if (visited) {
			renderer.material = visitedMaterial;
		} else {
			renderer.material = unvisitedMaterial;
		}

	}

}
