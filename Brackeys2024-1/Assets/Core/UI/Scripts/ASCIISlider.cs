using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ASCIISlider : MonoBehaviour {

	[SerializeField] private TMP_Text text;

	[Space]

	[SerializeField] private int length;
	[SerializeField] private float min;
	[SerializeField] private float max;
	[SerializeField] private float step;
	[SerializeField] private float value;

	[Space]

	public UnityEvent<float> OnValueChanged;

	private void Start() {
		SetValue(value);
	}

	public void SetValue(float value) {
		value = Mathf.Clamp(value, min, max);
		this.value = value;

		float t = Mathf.InverseLerp(min, max, value);
		int n = Mathf.RoundToInt(t * length);

		System.Text.StringBuilder sb = new System.Text.StringBuilder();

		if(n > 0) sb.Append('=', n);
		sb.Append('+');
		if(n < length) sb.Append('-', (length - n));

		text.text = sb.ToString();

		OnValueChanged?.Invoke(value);
	}

	public void Add() {
		SetValue(value + step);
	}

	public void Sub() {
		SetValue(value - step);
	}

}
