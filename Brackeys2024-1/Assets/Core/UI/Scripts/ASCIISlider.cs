using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ASCIISlider : MonoBehaviour {

	[SerializeField] private TMP_Text text;

	[Space]

	[SerializeField] private int length;
	[SerializeField] private float min;
	[SerializeField] private float max;
	[SerializeField] private float step;
	[SerializeField] private float value;

	private void Start() {
		SetValue(value);
	}

	public void SetValue(float value) {
		this.value = Mathf.Clamp(value, min, max);

		float t = Mathf.InverseLerp(min, max, value);
		int n = Mathf.RoundToInt(t * length);

		System.Text.StringBuilder sb = new System.Text.StringBuilder();

		if(n > 0) sb.Append('=', n);
		sb.Append('+');
		if(n < length) sb.Append('-', (length - n));

		text.text = sb.ToString();
	}

	public void Add() {
		SetValue(value + step);
	}

	public void Sub() {
		SetValue(value - step);
	}

}
