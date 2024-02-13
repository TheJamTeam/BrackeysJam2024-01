using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class ASCIIFrame : MonoBehaviour {

	[SerializeField] private int width;
	[SerializeField] private int height;

	private TMP_Text text;

	private void Awake() {
		text = GetComponent<TMP_Text>();
	}

	private void OnValidate() {
		if(width < 3) width = 3;
		if(height < 3) height = 3;

		System.Text.StringBuilder sb = new System.Text.StringBuilder();

		sb.Append('+');
		sb.Append('-', width - 2);
		sb.Append("+\n");

		for(int i = 0; i < height - 2; i++) {
			sb.Append('|');
			sb.Append(' ', width - 2);
			sb.Append("|\n");
		}

		sb.Append('+');
		sb.Append('-', width - 2);
		sb.Append('+');

		GetComponent<TMP_Text>().text = sb.ToString();
	}

}
