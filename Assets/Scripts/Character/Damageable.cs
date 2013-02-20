using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour {

	const int OBSTACLE_LAYER = 8;

	void Meet(GameObject other) {

		if (other.layer == OBSTACLE_LAYER)
			GameController.instance.Hit();

	}

	void OnControllerColliderHit(ControllerColliderHit hit) {

		Meet(hit.gameObject);

	}

	void OnTriggerEnter(Collider other) {

		Meet(other.gameObject);

	}

}
