using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

// Volume control for slider
public class VolumeControl : MonoBehaviour {

    // Setup
    public AudioMixer mixer;
    public string volume;

    // Set volume level
    public void SetLevel(float sliderValue) {
        mixer.SetFloat(volume, Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat(volume, sliderValue);
    }
}
