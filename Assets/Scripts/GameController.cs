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

	}

#endregion

#region Game state

	public event Action GameOver;
	public event Action GameReset;

	public enum GameState {
		Running,
		GameOver,
		Won,
	}

	public GameState gameState { get; private set; }

	public void Start() {

		gameState = GameState.Running;

	}

	public void Hit() {

		// TODO: implement actual hit points

		if (GameOver != null) // TODO: make thread-safe
			GameOver();

		gameState = GameState.GameOver;

	}

	public void Win() {

		gameState = GameState.Won;

	}

	public void Reset() {

		if (GameReset != null) // TODO: make thread-safe
			GameReset();

		gameState = GameState.Running;

	}

#endregion

#region Debug GUI

	public void OnGUI() {

		if (gameState == GameState.Running)
			return;

		string labelText = "";

		if (gameState == GameState.GameOver) {

			labelText = "You lost.";

		} else if (gameState == GameState.Won) {

			labelText = "You won!";

		}

		GUI.Label( new Rect(Screen.width/2 - 100, Screen.height/2 - 50, 100, 20), labelText );

		if ( GUI.Button( new Rect(Screen.width/2 - 100, Screen.height/2 - 20, 100, 20), "Reset" ) )
				Reset();

	}

#endregion

}