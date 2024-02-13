using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Overarching gamee/app manager.
public class Game : Singleton<Game> {
    
	public static UIManager UI => Instance.ui;

	private UIManager ui;

	protected override void Awake() {
		base.Awake();

		ui = FindObjectOfType<UIManager>();
	}

	private void Start() {
		if(Application.isEditor && SceneManager.sceneCount > 1) {
			// If any additional scene is loaded in the editor, then automatically start game & create context.
			StartNewGame();
		}
	}

	public void StartNewGame() {
		Context.Create();
	}

	public void QuitGame() {
		Application.Quit();
	}

}