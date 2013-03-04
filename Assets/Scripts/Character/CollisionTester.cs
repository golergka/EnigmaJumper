using UnityEngine;
using System.Collections;

public class CollisionTester : MonoBehaviour {

	void OnCollisionEnter(Collision collisionInfo) {

		Debug.Log("OnCollisionEnter " + collisionInfo.ToString() );

	}

	void OnTriggerEnter(Collider other) {

		Debug.Log("OnTriggerEnter " + other.ToString() );

	}

	void OnControllerColliderHit(ControllerColliderHit hit) {

		Debug.Log("OnControllerColliderHit " + hit.ToString() );

	}

}
