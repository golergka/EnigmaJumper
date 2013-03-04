using UnityEngine;
using System.Collections;

public class Resetable : MonoBehaviour {

	Vector3 startPosition;
	Vector3 startScale;
	Quaternion startRotation;

	void Start() {

		startPosition = transform.position;
		startScale    = transform.localScale;
		startRotation = transform.rotation;

		GameController.instance.GameReset += OnGameReset;

	}

	void OnGameReset() {

		transform.position   = startPosition;
		transform.localScale = startScale;
		transform.rotation   = startRotation;

	}

}
