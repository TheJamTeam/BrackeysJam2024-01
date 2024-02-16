using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : Menu {

	[SerializeField] private ASCIISlider masterVolumeSlider;
	[SerializeField] private ASCIISlider sfxVolumeSlider;
	[SerializeField] private ASCIISlider musicVolumeSlider;

	private void Start() {
		masterVolumeSlider.SetValue(AudioManager.Instance.masterVolume);
		sfxVolumeSlider.SetValue(AudioManager.Instance.sfxVolume);
		musicVolumeSlider.SetValue(AudioManager.Instance.musicVolume);
	}

	public void OnBackPressed() => UIManager.Back();

	public void OnMasterVolumeChanged(float value) => AudioManager.Instance.ChangeMasterVol(value);

	public void OnSFXVolumeChanged(float value) => AudioManager.Instance.ChangeSfxVol(value);

	public void OnMusicVolumeChanged(float value) => AudioManager.Instance.ChangeMusicVol(value);

}