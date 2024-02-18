using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu {

	[SerializeField] private Button quitButton;

	private void Start() {
		if(Application.platform == RuntimePlatform.WebGLPlayer) {
			quitButton.gameObject.SetActive(false);
		}
	}

	public void OnStartPressed() => Game.Instance.StartNewGame();

	public void OnOptionsPressed() => UIManager.Show<OptionsMenu>(true);

	public void OnCreditsPressed() => UIManager.Show<CreditsMenu>(true);

	public void OnQuitPressed() => Application.Quit();

}
