using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menu {

	public void OnResumePressed() => Game.Instance.Pause(false);

	public void OnOptionsPressed() => UIManager.Show<OptionsMenu>(true);

	public void OnQuitPressed() => Application.Quit();

}
