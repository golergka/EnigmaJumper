using UnityEngine;
using System.Collections;

public class LineStabilizer : MonoBehaviour {

#region Setup

	MovementController movementController;

	void Awake() {

		movementController = GetComponent<MovementController>();

		if (!movementController)
			movementController = gameObject.AddComponent<MovementController>();

	}

#endregion

	public float grid = 10f;

	public float stabilizationSpeed = 3f;
	public float stabilizationSnap  = 0.1f;
	
	// Update is called once per frame
	void Update () {

		Vector3 target = transform.position;

		if ( Mathf.Abs(transform.forward.z) > Mathf.Abs(transform.forward.x) ) { // character is moving alongside x

			target.x = Mathf.Round( ( transform.position.x - grid/2 ) / grid ) * grid + grid/2;

		} else { // character is moving alongside z

			target.z = Mathf.Round( ( transform.position.z - grid/2 ) / grid ) * grid + grid/2;

		}

		Vector3 motion;

		if ( (target - transform.position).magnitude < stabilizationSnap ) {

			motion = target - transform.position;

		} else {

			motion = (target - transform.position).normalized * stabilizationSpeed * Time.deltaTime;

		}

		movementController.motion += motion;
	
	}
}
