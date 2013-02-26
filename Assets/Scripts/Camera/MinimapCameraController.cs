using UnityEngine;
using System.Collections;

public class MinimapCameraController : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {

		// TODO: Create abstract camera controller and move initialisation there

		if(!target) {
			Debug.LogWarning("Please configure target for the camera");
			enabled = false;
		}
	
	}
	
	void LateUpdate() {

		Vector3 targetPosition = target.position;
		targetPosition.y = transform.position.y;

		transform.position = targetPosition;

	}

}
