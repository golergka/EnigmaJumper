using UnityEngine;
using System.Collections;

public class AcrobatController : MonoBehaviour {

	const string JUMP_BUTTON_NAME   = "Jump";
	const string CROUCH_BUTTON_NAME = "Crouch";
	const string RIGHT_BUTTON_NAME  = "Right";
	const string LEFT_BUTTON_NAME   = "Left";

#region Setup

	CharacterController characterController;

	void Awake () {

		characterController = GetComponent<CharacterController>();
		if (!characterController) {

			Debug.LogWarning("Please attach CharacterController component!");
			enabled = false;

		}

	}

	float startTime;

	void Start() {

		startTime = Time.time;

	}	

#endregion

#region Movement states

	enum AcrobatState {
		Idle,
		Jumping,
		Crouching,
		LeftTurn,
		RightTurn,
	}

	AcrobatState state = AcrobatState.Idle;

	public float jumpVelocity = 5f;

	void Jump() {

		state = AcrobatState.Jumping;
		verticalVelocity += jumpVelocity;

	}

	public float crouchVelocity = 5f;

	void Crouch() {

		state = AcrobatState.Crouching;
		scaleVelocity -= crouchVelocity;

	}

	void LeftTurn() {

		state = AcrobatState.LeftTurn;

	}

	void RightTurn() {

		state = AcrobatState.RightTurn;

	}

#endregion

#region Input
	
	// Update is called once per frame
	void Update () {

		if (state != AcrobatState.Idle)
			return;

		if (Input.GetButton(JUMP_BUTTON_NAME))
		{
			Jump();
		} 
		else if (Input.GetButton(CROUCH_BUTTON_NAME))
		{
			Crouch();
		}
		else if (Input.GetButton(LEFT_BUTTON_NAME))
		{
			LeftTurn();
		}
		else if (Input.GetButton(RIGHT_BUTTON_NAME))
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

	float verticalVelocity = 0f;
	public float gravity = 9f;

	float scaleVelocity = 0f;
	public float scaleGravity = 1f;
	public float minScale = 0.5f;

	public float rotationSpeed = 10f;
	float totalRotation = 0;
	const float ROTATION_LIMIT = 90f;

	void LateUpdate() {

		// Rotating

		if (state == AcrobatState.LeftTurn ||
			state == AcrobatState.RightTurn) {

			bool sign = (state == AcrobatState.RightTurn);
			float rotation = rotationSpeed * Time.deltaTime;

			transform.Rotate(0, ( sign ? 1f : -1f ) * rotation, 0);
			totalRotation += rotation;

			if (totalRotation >= ROTATION_LIMIT) {

				transform.Rotate(0, ( sign ? 1f : -1f ) * (ROTATION_LIMIT - totalRotation), 0);
				totalRotation = 0;
				state = AcrobatState.Idle;

			}	

		}

		// Scale

		Vector3 scale = transform.localScale;

		if (scale.y >= 1f && scaleVelocity >= -0.0001 ) {

			if (state == AcrobatState.Crouching) {

				state = AcrobatState.Idle;

			}

			scaleVelocity = 0f;			
			scale.y = 1f;

		} else {

			scaleVelocity += scaleGravity * Time.deltaTime;

		}

		scale.y += scaleVelocity * Time.deltaTime;
		scale.y = Mathf.Max(scale.y, minScale);

		transform.localScale = scale;

		// Vertical movement & gravity

		Vector3 velocity = HorizontalVelocity();

		// Floating point stuff. Not really a magic number
		if (characterController.isGrounded && verticalVelocity <= 0.00001 ) {

			if (state == AcrobatState.Jumping) {

				state = AcrobatState.Idle;

			}

			verticalVelocity = 0f;

		} else {

			verticalVelocity -= gravity * Time.deltaTime;

		}

		velocity.y = verticalVelocity;

		characterController.Move(velocity * Time.deltaTime);

	}

#endregion

}
