using CustomScripts.Core.UI.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomScripts.Helpers;

// Essentially a world manager of room scenes, player, keys, objects.
namespace CustomScripts.Core
{
	public class Context : Singleton<Context> {

		private Scene[] roomScenes;

		protected override void Awake() {
			base.Awake();

			// Skip load scene at i = 0 because it should be the main scene.
			roomScenes = new Scene[SceneManager.sceneCountInBuildSettings - 1];
			bool[] roomsLoaded = new bool[roomScenes.Length];
		

			for(int i = 1; i < SceneManager.sceneCount; i++) {
				Scene scene = SceneManager.GetSceneAt(i);
				roomsLoaded[scene.buildIndex - 1] = true;
				roomScenes[scene.buildIndex - 1] = scene;
			}

			LoadSceneParameters parameters = new LoadSceneParameters(LoadSceneMode.Additive);
			for(int i = 0; i < roomScenes.Length; i++) {
				if(!roomsLoaded[i]) {
					roomScenes[i] = SceneManager.LoadScene(i + 1, parameters);
				}
			}
		}

		protected override void OnDestroy() {
			if(!Instance)
				return;

			base.OnDestroy();

			// Game.Instance.Pause(true);
			// UIManager.Camera.gameObject.SetActive(true);

			for(int i = 0; i < roomScenes.Length; i++) {
				SceneManager.UnloadSceneAsync(roomScenes[i]);
			}
		}

		public static void Destroy() {
			if(!Instance) {
				return;
			}

			GameObject obj = Instance.gameObject;
			Instance.OnDestroy();
			Destroy(obj);
		}

		public static void Create() {
			if(Instance) {
				Destroy();
			}

			new GameObject("Context", typeof(Context));

			UIManager.Camera.gameObject.SetActive(false);
			Game.Instance.Pause(false);
		}

	}
}