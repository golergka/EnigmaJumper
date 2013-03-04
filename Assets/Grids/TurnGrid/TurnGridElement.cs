using UnityEngine;
using System.Collections;

public class TurnGridElement : MonoBehaviour {

	public float linecastLength = 10f;

	public bool xPositive { get; private set; }
	public bool xNegative { get; private set; }
	public bool zPositive { get; private set; }
	public bool zNegative { get; private set; }

	Vector3 xPositiveLinecast;
	Vector3 xNegativeLinecast;
	Vector3 zPositiveLinecast;
	Vector3 zNegativeLinecast;

	// Use this for initialization
	void Start () {
	
		xPositiveLinecast = new Vector3(linecastLength,0,0);
		xNegativeLinecast = new Vector3(-linecastLength,0,0);
		zPositiveLinecast = new Vector3(0,0,linecastLength);
		zNegativeLinecast = new Vector3(0,0,-linecastLength);

		int layerMask = (1 << 8);

		xPositive = !Physics.Linecast(transform.position, transform.position + xPositiveLinecast, layerMask);
		xNegative = !Physics.Linecast(transform.position, transform.position + xNegativeLinecast, layerMask);
		zPositive = !Physics.Linecast(transform.position, transform.position + zPositiveLinecast, layerMask);
		zNegative = !Physics.Linecast(transform.position, transform.position + zNegativeLinecast, layerMask);

	}

	void OnDrawGizmosSelected() {

		Gizmos.color = Color.green;

		if (xPositive) {
			Gizmos.DrawLine(transform.position, transform.position + xPositiveLinecast);
		}

		if (xNegative) {
			Gizmos.DrawLine(transform.position, transform.position + xNegativeLinecast);
		}

		if (zPositive) {
			Gizmos.DrawLine(transform.position, transform.position + zPositiveLinecast);
		}

		if (zNegative) {
			Gizmos.DrawLine(transform.position, transform.position + zNegativeLinecast);
		}

	}
	
}
