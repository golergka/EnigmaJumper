using UnityEngine;
using System.Collections;

public class KeyboardInput : MonoBehaviour {

	const string BUTTON_JUMP   = "Jump";
	const string BUTTON_CROUCH = "Crouch";
	const string BUTTON_RIGHT  = "Right";
	const string BUTTON_LEFT   = "Left";

	JumpController     jumpController;
	MovementController movementController;

	void Awake() {

		jumpController = GetComponent<JumpController>();

		if (!jumpController) {

			Debug.LogWarning("Please attach jump controller to the character!");
			enabled = false;

		}

		movementController = GetComponent<MovementController>();

		if (!movementController) {

			Debug.LogWarning("Please attach movement controller to the character!");
			enabled = false;
			
		}

	}
	
	// Update is called once per frame
	void Update () {

		if ( Input.GetButton(BUTTON_JUMP) ) {
			jumpController.Jump();
		}

		if ( Input.GetButton(BUTTON_CROUCH) ) {
			jumpController.Crouch();
		}

		if ( Input.GetButton(BUTTON_LEFT) ) {
			movementController.TurnLeft();
		}

		if ( Input.GetButton(BUTTON_RIGHT) ) {
			movementController.TurnRight();
		}
	
	}
	
}
