using UnityEngine;
using System.Collections;

public class TurnController : MonoBehaviour {

#region Setup

	TurnEnabler turnEnabler;

	void Awake() {

		turnEnabler = GetComponent<TurnEnabler>();

		if (!turnEnabler)
			turnEnabler = gameObject.AddComponent<TurnEnabler>();

	}

	void Start() {

		GameController.instance.GameReset += OnGameReset;

	}

#endregion

#region Game states

	void OnGameReset() {

		turnState = TurnState.Idle;

	}

#endregion

#region Public interface

	public void TurnLeft() {

		if (turnState != TurnState.Idle)
			return;

		if (!turnEnabler.CanTurnLeft())
			return;

		rotationTarget.x = - transform.forward.z;
		rotationTarget.z =   transform.forward.x;

		turnState = TurnState.Left;
		turnEnabler.Turned();

	}

	public void TurnRight() {

		if (turnState != TurnState.Idle)
			return;

		if (!turnEnabler.CanTurnRight())
			return;

		rotationTarget.x =   transform.forward.z;
		rotationTarget.z = - transform.forward.x;


		turnState = TurnState.Right;
		turnEnabler.Turned();

	}

#endregion

#region Private state

	enum TurnState {
		Idle,
		Right,
		Left,
	}

	TurnState turnState = TurnState.Idle;

	Vector3 rotationTarget = new Vector3();

#endregion

	public float rotationSpeed = 1f;

	public float rotationSnap = 0.3f;

	void Update() {

		if (turnState == TurnState.Idle)
			return;

		transform.forward = Vector3.Slerp(transform.forward, rotationTarget, rotationSpeed * Time.deltaTime);

		if ( (transform.forward - rotationTarget).magnitude < rotationSnap ) {

			transform.forward = rotationTarget;
			turnState = TurnState.Idle;

		}

	}

}
