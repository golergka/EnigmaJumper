using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	public float time { get; private set; }

	float startTime;

	void OnGameReset() {

		startTime = Time.time;

	}

	void OnGUI() {

		string timeText;

		int totalSeconds = Mathf.FloorToInt(time);
		int seconds = totalSeconds % 60;
        int minutes = totalSeconds / 60;

		timeText = string.Format("{0}:{1:00}", minutes, seconds);

		GUI.Label(new Rect(10,10,150,100), timeText);

	}

	// Use this for initialization
	void Start () {

		GameController.instance.GameReset += OnGameReset;

		OnGameReset();
	
	}
	
	// Update is called once per frame
	void Update () {

		if (GameController.instance.gameState == GameController.GameState.Running)
			time = Time.time - startTime;
	
	}
}
