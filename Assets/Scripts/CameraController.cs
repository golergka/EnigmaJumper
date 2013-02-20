using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	
	public float distance     = 1f;
	public float height       = 1f;
	public float targetOffset = 1f;

	// Use this for initialization
	void Start () {

		if(!target) {
			Debug.LogWarning("Please configure target for the camera");
			enabled = false;
		}		
	
	}

	// Needs to be public for sitations like game reset, player teleportation, etc
	public void ApplyPosition() {

		Vector3 groundedTarget = target.position;
		groundedTarget.y = 0;

		Vector3 groundedTargetForward = target.forward;
		groundedTargetForward.y = 0;

		Vector3 targetCameraPosition = groundedTarget - groundedTargetForward * distance;
		targetCameraPosition.y = height;

		Vector3 targetLookPosition = target.position;
		targetLookPosition.y += targetOffset;

		Quaternion targetCameraRotation = Quaternion.LookRotation(targetLookPosition - targetCameraPosition);

		transform.position = targetCameraPosition;
		transform.rotation = targetCameraRotation;

	}
	
	// Update is called once per frame
	void LateUpdate () {

		ApplyPosition();
	
	}
}
