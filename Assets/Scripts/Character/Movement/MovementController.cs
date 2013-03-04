using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	[HideInInspector]
	public Vector3 motion;

	CharacterController characterController;

	void Awake() {

		characterController = GetComponent<CharacterController>();

		if (!characterController) {
			Debug.LogWarning("Please attach character controller to the character!");
			enabled = false;
		}

	}
	
	// Update is called once per frame
	void Update () {

		characterController.Move(motion);

		motion = new Vector3();
	
	}
}
