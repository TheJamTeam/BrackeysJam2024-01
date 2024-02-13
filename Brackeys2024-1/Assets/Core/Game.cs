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
			Pause(true);
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
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		} else if(!pause && isPaused) {
			Time.timeScale = 1.0F;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		isPaused = pause;

		if(Context.Exists) {
			if(isPaused) {
				UIManager.Show<PauseMenu>();
			} else {
				UIManager.Show<HUD>();
			}
		} else {
			UIManager.Show<MainMenu>();
		}
	}

}