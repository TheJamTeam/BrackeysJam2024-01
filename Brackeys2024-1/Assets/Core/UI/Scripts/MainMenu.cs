using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu {
    
	public void OnStartPressed() => Game.Instance.StartNewGame();

	public void OnOptionsPressed() => UIManager.Show<OptionsMenu>(true);

	public void OnCreditsPressed() => UIManager.Show<CreditsMenu>(true);

	public void OnQuitPressed() => Application.Quit();

}
