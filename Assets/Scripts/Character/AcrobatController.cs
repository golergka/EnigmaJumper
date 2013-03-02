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

		verticalVelocity = 0f;
		scaleVelocity    = 0f;
		totalRotation    = 0f;

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
		Jumping,
		Crouching,
		LeftTurn,
		RightTurn,
		UTurn,
		GameOver,
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

	// Set up in Reset()
	float verticalVelocity;
	float scaleVelocity;
	float totalRotation;

	public float gravity = 9f;
	
	public float scaleGravity = 1f;
	public float minScale = 0.5f;

	public float rotationSpeed = 10f;
	const float ROTATION_LIMIT = 90f;

	/*

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

	*/

#endregion

#region Detecting collisions

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

#endregion

}
