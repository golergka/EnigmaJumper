using UnityEngine;
using System.Collections;

public class TurnEnabler : MonoBehaviour {

	TurnGridElement currentGridElement;

	void OnTriggerEnter(Collider other) {

		TurnGridElement newGridElement = other.GetComponent<TurnGridElement>();

		if (newGridElement)
			currentGridElement = newGridElement;

	}

	public bool CanTurnRight() {

		return CanTurn(true);

	}

	public bool CanTurnLeft() {

		return CanTurn(false);

	}

	const float MINIMUM_FLOAT = 0.01f;

	bool CanTurn(bool right) {

		Vector3 forward = transform.forward;

		if (forward.x > MINIMUM_FLOAT) {

			return right ? currentGridElement.zNegative : currentGridElement.zPositive;

		} else if (forward.x < -MINIMUM_FLOAT) {

			return right ? currentGridElement.zPositive : currentGridElement.zNegative;

		} else if (forward.z > MINIMUM_FLOAT) {

			return right ? currentGridElement.xPositive : currentGridElement.xNegative;

		} else if (forward.z < -MINIMUM_FLOAT) {

			return right ? currentGridElement.xNegative : currentGridElement.xPositive;

		} else {

			Debug.LogError("Cannot determine character direction!");
			return false;

		}

	}

}