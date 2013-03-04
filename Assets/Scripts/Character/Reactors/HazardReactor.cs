using UnityEngine;
using System.Collections;

public class HazardReactor : MonoBehaviour {

	const int LAYER_HAZARD = 13;

	void OnControllerColliderHit(ControllerColliderHit hit) {

		if ( hit.gameObject.layer == LAYER_HAZARD) {

			GameController.instance.Hit();

		}

	}

}
