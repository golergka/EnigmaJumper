using UnityEngine;
using System.Collections;

public class VisitGridDetector : MonoBehaviour {

	public VisitGridMarker marker;

	const string PLAYER_TAG = "Player";

	void Awake() {

		if (!marker) {

			marker = GetComponentInChildren<VisitGridMarker>();

			if (!marker) {

				Debug.LogWarning("My marker is not set up!");
				enabled = false;

			}

		}

	}

	void Start() {

		GameController.instance.GameReset += OnGameReset;

	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == PLAYER_TAG)
			marker.SetVisited(true);

	}

	void OnGameReset() {

		marker.SetVisited(false);

	}

}
