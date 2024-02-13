using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Overarching gamee/app manager.
public class Game : Singleton<Game> {

	public bool IsPaused => isPaused;

	private bool isPaused = true;

	protected override void Awake() {
		base.Awake();
	}

	private void Start() {
		if(Application.isEditor && SceneManager.sceneCount > 1) {
			// If any additional scene is loaded in the editor, then automatically start game & create context.
			StartNewGame();
		} else {
			UIManager.Show<MainMenu>();
		}
	}

	public void StartNewGame() {
		Context.Create();
	}

	public void QuitGame() {
		Application.Quit();
	}

	public void Pause(bool pause) {
		if(pause && !isPaused) {
			Time.timeScale = 0.0F;
		} else if(!pause && isPaused) {
			Time.timeScale = 1.0F;
		}

		isPaused = pause;
	}

}