using UnityEngine;
using System.Collections;

public class ForwardMotor : MonoBehaviour {

	public float speed = 1f;

	void FixedUpdate() {

		Vector3 movement = transform.forward * Time.deltaTime * speed;
		rigidbody.MovePosition(rigidbody.position + movement);

	}

}
