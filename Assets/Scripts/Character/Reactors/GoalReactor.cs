using UnityEngine;
using System.Collections;

public class GoalReactor : MonoBehaviour {

	const int LAYER_GOAL = 14;

	void OnControllerColliderHit(ControllerColliderHit hit) {

		if ( hit.gameObject.layer == LAYER_GOAL) {

			GameController.instance.Win();

		}

	}

}
