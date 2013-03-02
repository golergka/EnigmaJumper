using UnityEngine;
using System.Collections;

public class AcrobatController : MonoBehaviour {

	const string JUMP_BUTTON_NAME   = "Jump";
	const string CROUCH_BUTTON_NAME = "Crouch";
	const string RIGHT_BUTTON_NAME  = "Right";
	const string LEFT_BUTTON_NAME   = "Left";

#region Setup

	Vector3    startPosition;
	Vector3    startScale;
	Quaternion startRotation;

	void OnGameReset() {

		state = AcrobatState.Idle;

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

#endregion

#region Scheduling turns

	const string ITWEEN_UTURN        = "UTurn";
	const string ITWEEN_FORWARD      = "MoveForward";

	public float gridPointStickTime = 0.2f;

	bool scheduledTurn = false;

	void ScheduleLeftTurn() {

		if (scheduledTurn)
			return;

		if (state != AcrobatState.Idle)
			return;

		state = AcrobatState.LeftTurn;

		scheduledTurn = true;

	}

	void ScheduleRightTurn() {

		if (scheduledTurn)
			return;

		if (state != AcrobatState.Idle)
			return;

		state = AcrobatState.RightTurn;

		scheduledTurn = true;

	}

#endregion

#region Implemeting turns

	const float MINIMUM_FLOAT = 0.01f;

	Vector3 TargetPosition() {

		Vector3 targetPosition = rigidbody.position;
		targetPosition.x = gridPoint.x;
		targetPosition.z = gridPoint.z;

		return targetPosition;

	}

	Vector3 TargetLookPosition(bool right) {

		Vector3 delta;
		Vector3 forward = transform.forward;

		if (forward.x > MINIMUM_FLOAT) {

			delta = new Vector3(0,0, (right ? -1f : 1f));

		} else if (forward.x < -MINIMUM_FLOAT) {

			delta = new Vector3(0,0, right ? 1f : -1f);

		} else if (forward.z > MINIMUM_FLOAT) {

			delta = new Vector3(right ? 1f : -1f, 0, 0);

		} else if (forward.z < -MINIMUM_FLOAT) {

			delta = new Vector3(right ? -1f : 1f, 0, 0);

		} else {

			Debug.LogError("Cannot determine character direction!");
			delta = new Vector3();

		}

		return TargetPosition() + delta;

	}

	void ImplementTurn() {

		bool right;

		switch (state) {

			case (AcrobatState.LeftTurn):
				right = false;
				break;

			case (AcrobatState.RightTurn):
				right = true;
				break;

			default:
				return;

		}

		iTweenEvent.GetEvent(gameObject, ITWEEN_FORWARD).Stop();

		Hashtable args = new Hashtable();

		args.Add( "position"   , TargetPosition()          );
		args.Add( "time"       , gridPointStickTime        );
		args.Add( "oncomplete" , "OnStickComplete"         );
		args.Add( "looktarget" , TargetLookPosition(right) );
		
		iTween.MoveTo(gameObject, args);

		state = AcrobatState.Idle;

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

	void OnStickComplete() {

		MoveForward();

	}

	void OnUTurnComplete() {

		scheduledTurn = false;
		MoveForward();

	}

	void OnMoveForwardComplete() {

		MoveForward();

	}

#endregion

#region Turn directing

	TurnGridElement currentTurnGridElement;

	public float turnDistance = 3f;

	Vector3 gridPoint {

		get {

			return currentTurnGridElement.transform.position;

		}

	}

	bool CanImplementTurn() {

		return (transform.position - gridPoint).magnitude < turnDistance;

	}

	bool CanTurn(bool right) {

		Vector3 forward = transform.forward;

		if (forward.x > MINIMUM_FLOAT) {

			return right ? currentTurnGridElement.zNegative : currentTurnGridElement.zPositive;

		} else if (forward.x < -MINIMUM_FLOAT) {

			return right ? currentTurnGridElement.zPositive : currentTurnGridElement.zNegative;

		} else if (forward.z > MINIMUM_FLOAT) {

			return right ? currentTurnGridElement.xPositive : currentTurnGridElement.xNegative;

		} else if (forward.z < -MINIMUM_FLOAT) {

			return right ? currentTurnGridElement.xNegative : currentTurnGridElement.xPositive;

		} else {
			
			Debug.LogError("Cannot determine character direction!");
			return false;

		}

	}

	public bool CanTurnRight() {

		return CanTurn(true);

	}

	public bool CanTurnLeft() {

		return CanTurn(false);

	}

#endregion

#region Detecting collisions

	const int LAYER_OBSTACLE = 8;
	const int LAYER_FLOOR    = 11;
	const int LAYER_TURN     = 12;

	void Meet(GameObject other) {

		if (other.layer == LAYER_OBSTACLE)
			UTurn();

		if (other.layer == LAYER_TURN)
			currentTurnGridElement = other.GetComponent<TurnGridElement>();

	}

	void Leave(GameObject other) {

		if (other.layer == LAYER_TURN) {

			scheduledTurn = false;
			
			TurnGridElement exTurnDirector = other.GetComponent<TurnGridElement>();
			if (currentTurnGridElement == exTurnDirector)
				currentTurnGridElement = null;

		}

	}

	// Whatever method of collision I'll use in the future, this will support them all

	void OnCollisionEnter(Collision collisionInfo) {

		Meet(collisionInfo.gameObject);

	}

	void OnTriggerEnter(Collider other) {

		Meet(other.gameObject);

	}

	void OnCollisionExit(Collision collisionInfo) {

		Leave(collisionInfo.gameObject);

	}

	void OnTriggerExit(Collider other) {

		Leave(other.gameObject);

	}

#endregion
		
	// Update is called once per frame
	void Update () {
		
		if ( Input.GetButton(LEFT_BUTTON_NAME) && CanTurnLeft() ) {
			ScheduleLeftTurn();
		}
		
		if ( Input.GetButton(RIGHT_BUTTON_NAME) && CanTurnRight() ) {
			ScheduleRightTurn();
		}

		if (CanImplementTurn())
			ImplementTurn();
	
	}

}