using UnityEngine;
using UnityEngine.UI;

namespace CustomScripts.Core.UI.Scripts
{
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
}
