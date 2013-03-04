using UnityEngine;
using System.Collections;

public class ObstacleReactor : MonoBehaviour {

#region Setup

	MovementController movementController;
	ForwardMotor forwardMotor;

	void Awake() {

		movementController = GetComponent<MovementController>();

		if (!movementController) {

			movementController = gameObject.AddComponent<MovementController>();

		}

		forwardMotor = GetComponent<ForwardMotor>();

		if (!forwardMotor) {

			Debug.LogWarning("Please attach forward motor to the character!");
			enabled = false;

		}

	}

#endregion

#region Designer configuration

	public AnimationCurve bounceSpeed;

#endregion

#region Private state
	
	bool acting = false;
	float bounceStart;

#endregion

	const int LAYER_OBSTACLE = 8;

	// Use this for initialization
	void OnControllerColliderHit(ControllerColliderHit hit) {

		if ( hit.gameObject.layer == LAYER_OBSTACLE && !acting) {

			acting = true;
			bounceStart = Time.time;
			forwardMotor.enabled = false;

		}

	}

	void Update() {

		if (!acting)
			return;

		float elapsedTime = Time.time - bounceStart;

		movementController.motion += transform.forward * bounceSpeed.Evaluate(elapsedTime) * Time.deltaTime;

		if ( bounceSpeed[bounceSpeed.length-1].time < elapsedTime ) {

			acting = false;
			forwardMotor.enabled = true;

		}


	}

}
