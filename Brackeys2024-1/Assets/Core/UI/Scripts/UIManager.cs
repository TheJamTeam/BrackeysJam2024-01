using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager> {

	public static Menu Current {
		get {
			if(Instance.stack.Count > 0)
				return Instance.stack[Instance.stack.Count - 1];
			else
				return null;
		}
	}

	public static Camera Camera => Instance.camera;
	public static EventSystem EventSystem => Instance.eventSystem;

	//----------------------------------------------------------------------------------------------------------

	[SerializeField] private EventSystem eventSystem;
	[SerializeField] private new AudioListener audio;
	[SerializeField] private new Camera camera;
	[SerializeField] private Menu[] menus;

	//----------------------------------------------------------------------------------------------------------

	private Dictionary<string, Menu> menuTable = new Dictionary<string, Menu>();
	private List<Menu> stack = new List<Menu>();
	private Canvas canvas;

	//----------------------------------------------------------------------------------------------------------

	protected override void Awake() {
		base.Awake();

		canvas = GetComponent<Canvas>();

		for(int i = 0; i < menus.Length; i++) {
			if(menuTable.ContainsKey(menus[i].ID)) {
				Debug.LogError("Two or more menus contain the same ID! '" + menus[i].ID + "'");
				continue;
			}
			menuTable.Add(menus[i].ID, menus[i]);
			if(menus[i].gameObject.activeSelf)
				menus[i].gameObject.SetActive(false);
		}
	}

	//----------------------------------------------------------------------------------------------------------

	private void Update() {
		bool pausePress = false;
		bool backPress = false;

		if(Current.GetType() != typeof(HUD)) {
			// TODO: pausePress = Input.Actions.UI.Pause.WasPressedThisFrame();
		} else {
			// TODO: pausePress = Input.Actions.Player.Pause.WasPressedThisFrame();
		}

		if(pausePress) {
			if(stack.Count > 1)
				Back();
			else if(Context.Exists)
				Game.Instance.Pause(!Game.Instance.IsPaused);
		} else if(backPress) {
			if(stack.Count > 1)
				Back();
		}
	}

	//----------------------------------------------------------------------------------------------------------

	public static void Back() {
		if(Instance.stack.Count < 2)
			return;

		Current.Hide();

		Instance.stack.RemoveAt(Instance.stack.Count - 1);

		Current.Show();
	}

	//----------------------------------------------------------------------------------------------------------

	public static void Show(string name, bool pushToStack = false) {
		if(Instance == null)
			return;

		if(Current != null)
			Current.Hide();

		if(!pushToStack)
			Instance.stack.Clear();

		if(Instance.menuTable.ContainsKey(name)) {
			Menu menu = GetMenu(name);

			Instance.stack.Add(menu);

			menu.Show();
		}
	}

	//----------------------------------------------------------------------------------------------------------

	public static void Show<T>(bool pushToStack = false) where T : Menu {
		if(Instance == null)
			return;

		if(Current != null)
			Current.Hide();

		if(!pushToStack)
			Instance.stack.Clear();

		Menu menu = GetMenu<T>();

		if(menu != null) {
			Instance.stack.Add(menu);
			menu.Show();
		}
	}

	//----------------------------------------------------------------------------------------------------------

	public static T GetMenu<T>(string name) where T : Menu => (T)Instance.menuTable[name];

	//----------------------------------------------------------------------------------------------------------

	public static Menu GetMenu(string name) => Instance.menuTable[name];

	//----------------------------------------------------------------------------------------------------------

	public static T GetMenu<T>() where T : Menu {
		foreach(Menu menu in Instance.menus)
			if(typeof(T) == menu.GetType())
				return (T)menu;
		return null;
	}

	//----------------------------------------------------------------------------------------------------------

	public static void ShowCursor(bool show) {
		Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = show;
	}

	//----------------------------------------------------------------------------------------------------------

	public static AudioListener GetAudioListener() => Instance.audio;

}
