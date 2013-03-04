using UnityEngine;
using System.Collections;

public class ScaleDelegate : MonoBehaviour {

	CharacterController characterController;

	// Use this for initialization
	void Start () {

		characterController = transform.parent.GetComponent<CharacterController>();

		if (!characterController) {

			Debug.LogWarning("Delegate's parent should have character controller!");
			enabled = false;

		}
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 scale = transform.localScale;

		scale.y = characterController.height / 2;

		transform.localScale = scale;
	
	}
}
