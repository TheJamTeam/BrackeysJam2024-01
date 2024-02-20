namespace CustomScripts.Core.UI.Scripts
{
	public class PauseMenu : Menu {

		public void OnResumePressed() => Game.Instance.Pause(false);

		public void OnOptionsPressed() => UIManager.Show<OptionsMenu>(true);

		public void OnQuitPressed() => Game.Instance.RestartGame();

	}
}
