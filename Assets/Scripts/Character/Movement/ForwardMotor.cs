using UnityEngine;
using System.Collections;

public class ForwardMotor : MonoBehaviour {

#region Setup

	MovementController movementController;

	void Awake() {

		movementController = GetComponent<MovementController>();

		if (!movementController)
			movementController = gameObject.AddComponent<MovementController>();

	}

#endregion

	public float speed = 5f;
	
	// Update is called once per frame
	void Update () {

		movementController.motion += transform.forward * speed * Time.deltaTime;
	
	}
}
