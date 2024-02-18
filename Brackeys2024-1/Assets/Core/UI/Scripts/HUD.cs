using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : Menu {

	private GameObject player;
	private PlayerInteractionComponent interaction;

	private InteractComponent lastFocus;
	private InteractComponent lastHolding;

	[SerializeField] private TMP_Text interactionText;

	private void OnEnable() {
		player = GameObject.FindGameObjectWithTag("Player");
		interaction = player.GetComponent<PlayerInteractionComponent>();
		lastFocus = null;
		lastHolding = null;
		interactionText.gameObject.SetActive(false);
	}

	private void Update() {
		if(!interaction) return;

		if(lastFocus == interaction.currentFocus && lastHolding == interaction.currentlyHoldingObject)
			return;

		if(interaction.currentFocus) {
			if(interaction.currentlyHoldingObject) {
				// Use X on Y
				interactionText.text = interaction.currentlyHoldingObject.InteractWhileHoldingVerb.Replace("[THIS]", interaction.currentlyHoldingObject.name).Replace("[OTHER]", interaction.currentFocus.name);
			} else {
				// Verb Y
				interactionText.text = interaction.currentFocus.InteractVerb.Replace("[THIS]", interaction.currentFocus.name);
			}
			interactionText.gameObject.SetActive(true);
		} else {
			interactionText.gameObject.SetActive(false);
		}

		lastFocus = interaction.currentFocus;
		lastHolding = interaction.currentlyHoldingObject;
	}

}