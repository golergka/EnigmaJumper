using UnityEngine;
using System.Collections;

public class JumpController : MonoBehaviour {

#region Setup

	MovementController movementController;
	CharacterController characterController;

	float defaultHeight;
	float defaultY;

	void Awake() {

		movementController = GetComponent<MovementController>();

		if (!movementController)
			movementController = gameObject.AddComponent<MovementController>();

		characterController = GetComponent<CharacterController>();

		if (!characterController) {
			Debug.LogWarning("Please attach character controller to the character!");
			enabled = false;
			return;
		}

		defaultHeight = characterController.height;
		defaultY      = transform.position.y;

	}

	void Start() {

		GameController.instance.GameReset += OnGameReset;

	}

#endregion

#region Game state

	void OnGameOver() {

	}

	void OnGameReset() {

		Reset();
		jumpState = JumpState.Idle;

	}

#endregion

#region Designer configuration

	public AnimationCurve jumpCurve;
	public AnimationCurve crouchCurve;

#endregion

#region Public interface

	public void Jump() {

		if (jumpState == JumpState.Jumping)
			return;

		Reset();
		maneuverStart = Time.time;
		jumpState = JumpState.Jumping;

	}

	public void Crouch() {

		if (jumpState == JumpState.Crouching)
			return;

		Reset();
		maneuverStart = Time.time;
		jumpState = JumpState.Crouching;

	}

	public void Abort() {

		Reset();
		jumpState = JumpState.Idle;

	}

#endregion

#region Private state

	enum JumpState {
		Idle,
		Jumping,
		Crouching,
	}

	JumpState jumpState = JumpState.Idle;

	float maneuverStart = 0f;

#endregion

#region Performing movement

	void Reset() {

		movementController.motion.y = defaultY - transform.position.y;
		characterController.height  = defaultHeight;

	}

	void Update() {

		float elapsedTime = Time.time - maneuverStart;

		switch(jumpState) {

			case JumpState.Idle:
				break;

			case JumpState.Jumping:
				
				movementController.motion.y = jumpCurve.Evaluate(elapsedTime) - transform.position.y;

				if ( jumpCurve[jumpCurve.length-1].time < elapsedTime ) {
					Reset();
					jumpState = JumpState.Idle;
				}

				break;

			case JumpState.Crouching:

				float oldHeight = characterController.height;

				characterController.height = crouchCurve.Evaluate(elapsedTime);

				float newHeight = characterController.height;

				movementController.motion.y = (newHeight - oldHeight)/2;

				if ( crouchCurve[crouchCurve.length-1].time < elapsedTime ) {
					Reset();
					jumpState = JumpState.Idle;
				}

				break;

			default:
				Debug.LogWarning("Unknown jumping state!");
				break;

		}

	}

#endregion

}
