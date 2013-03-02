using UnityEngine;
using System.Collections;

public class AcrobatController : MonoBehaviour {

	const string JUMP_BUTTON_NAME   = "Jump";
	const string CROUCH_BUTTON_NAME = "Crouch";
	const string RIGHT_BUTTON_NAME  = "Right";
	const string LEFT_BUTTON_NAME   = "Left";

#region Setup

	float      startTime;
	Vector3    startPosition;
	Vector3    startScale;
	Quaternion startRotation;

	void OnGameReset() {

		state = AcrobatState.Idle;

		startTime            = Time.time;
		transform.position   = startPosition;
		transform.localScale = startScale;
		transform.rotation   = startRotation;

	}

	void OnGameOver() {

		state = AcrobatState.GameOver;

	}

	void Start() {

		GameController.instance.GameReset += OnGameReset;
		GameController.instance.GameOver  += OnGameOver;

		startPosition = transform.position;
		startScale    = transform.localScale;
		startRotation = transform.rotation;

		OnGameReset();

	}	

#endregion

#region Movement states

	enum AcrobatState {

		Idle,

		LeftTurn,
		RightTurn,

		GameOver,

	}

	AcrobatState state = AcrobatState.Idle;

	const string ITWEEN_ROTATE_LEFT  = "RotateLeft";
	const string ITWEEN_ROTATE_RIGHT = "RotateRight";
	const string ITWEEN_UTURN        = "UTurn";
	const string ITWEEN_FORWARD      = "MoveForward";

	void LeftTurn() {

		if (state != AcrobatState.Idle)
			return;

		state = AcrobatState.LeftTurn;

		

	}

	void RightTurn() {

		if (state != AcrobatState.Idle)
			return;

		state = AcrobatState.RightTurn;

		

	}

	void UTurn() {

		iTweenEvent.GetEvent(gameObject, ITWEEN_FORWARD).Stop();
		iTweenEvent.GetEvent(gameObject, ITWEEN_UTURN).Play();

	}

	void MoveForward() {

		iTweenEvent.GetEvent(gameObject, ITWEEN_FORWARD).Play();

	}

#endregion

#region iTween callbacks

	void OnRotateLeftComplete() {

		state = AcrobatState.Idle;
		MoveForward();

	}

	void OnRotateRightComplete() {

		state = AcrobatState.Idle;
		MoveForward();

	}

	void OnUTurnComplete() {

		MoveForward();

	}

	void OnMoveForwardComplete() {

		switch(state) {

			case AcrobatState.Idle:
				MoveForward();
				break;

			case AcrobatState.LeftTurn:
				iTweenEvent.GetEvent(gameObject, ITWEEN_ROTATE_LEFT).Play();
				break;

			case AcrobatState.RightTurn:
				iTweenEvent.GetEvent(gameObject, ITWEEN_ROTATE_RIGHT).Play();
				break;

			default:
				break;

		}

	}

#endregion

#region Input
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetButton(LEFT_BUTTON_NAME))
		{
			LeftTurn();
		}
		
		if (Input.GetButton(RIGHT_BUTTON_NAME))
		{
			RightTurn();
		}
	
	}

#endregion

#region Movement

	public float baseVelocity = 1f;
	public float acceleration = 0.1f;

	float Speed() {

		return baseVelocity + acceleration * (Time.time - startTime);

	}

	Vector3 HorizontalVelocity() {

		Vector3 result = transform.forward;
		result *= Speed();
		result.y = 0;
		return result;

	}

#endregion

#region Detecting collisions

	const int LAYER_OBSTACLE = 8;
	const int LAYER_FLOOR    = 11;

	void Meet(GameObject other) {

		if (other.layer == LAYER_OBSTACLE)
			UTurn();

	}

	// Whatever method of collision I'll use in the future, this will support them all

	void OnCollisionEnter(Collision collisionInfo) {

		Meet(collisionInfo.gameObject);

	}

	void OnControllerColliderHit(ControllerColliderHit hit) {

		Meet(hit.gameObject);

	}

	void OnTriggerEnter(Collider other) {

		Meet(other.gameObject);

	}

#endregion

}
