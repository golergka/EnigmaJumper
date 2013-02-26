using System;
using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

#region Singleton

	static public GameController instance;

	void Awake() {

		if (!instance) {
			instance = this;
		} else {
			Debug.LogWarning("GameController instance duplication detected!");
			enabled = false;
		}

		gameOver = false;

	}

#endregion

#region Game state

	public event Action GameOver;
	public event Action GameReset;

	public bool gameOver { get; private set; }

	public void Hit() {

		// TODO: implement actual hit points

		if (GameOver != null) // TODO: make thread-safe
			GameOver();

		gameOver = true;

	}

	public void Reset() {

		if (GameReset != null) // TODO: make thread-safe
			GameReset();

		gameOver = false;

	}

#endregion

#region Debug GUI

	public void OnGUI() {

		if (gameOver) {

			if ( GUI.Button( new Rect(Screen.width/2 - 100, Screen.height/2 - 20, 100, 20), "Reset" ) )
				Reset();

		}

	}

#endregion

}