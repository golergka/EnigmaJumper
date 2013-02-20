using UnityEngine;
using System.Collections;

public class ForwardMotor : MonoBehaviour {

	public float baseVelocity = 1f;
	public float acceleration = 0.1f;

	float startTime;

	void Start() {

		startTime = Time.time;

	}

	float Speed() {

		return baseVelocity + acceleration * (Time.time - startTime);

	}

	Vector3 Velocity() {

		Vector3 result = transform.forward;
		result *= Speed();
		return result;

	}
	
	// Update is called once per frame
	void Update () {

		transform.position += Velocity() * Time.deltaTime;
	
	}
}
