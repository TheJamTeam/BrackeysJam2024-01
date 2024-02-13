using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

	public string ID => id;

	//----------------------------------------------------------------------------------------------------------

	[SerializeField] private string id;
	[SerializeField] private GameObject defaultSelectedElement;

	//----------------------------------------------------------------------------------------------------------

	public void Show() {
		gameObject.SetActive(true);
		OnShow();

		UIManager.EventSystem.SetSelectedGameObject(GetStartSelectedElement());
	}

	//----------------------------------------------------------------------------------------------------------

	public void Hide() {
		gameObject.SetActive(false);
		OnHide();
	}

	//----------------------------------------------------------------------------------------------------------

	protected virtual void OnShow() { }

	//----------------------------------------------------------------------------------------------------------

	protected virtual void OnHide() { }

	//----------------------------------------------------------------------------------------------------------

	public virtual GameObject GetStartSelectedElement() => defaultSelectedElement;

}
