using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace MonsterVox.UI
{
    /// <summary>
    /// Settings popup with volume sliders for BGM, SFX, and Vocal channels.
    /// Persists values in PlayerPrefs. Binds directly to AudioMixer exposed parameters.
    /// </summary>
    public class SettingsPopupUI : MonoBehaviour
    {
        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer audioMixer;

        [Header("Sliders")]
        [SerializeField] private Slider sliderBGM;
        [SerializeField] private Slider sliderSFX;
        [SerializeField] private Slider sliderVocal;

        [Header("Buttons")]
        [SerializeField] private Button btnClose;

        // These must match the exposed parameter names in the AudioMixer
        private const string PARAM_BGM = "BGMVolume";
        private const string PARAM_SFX = "SFXVolume";
        private const string PARAM_VOCAL = "VocalVolume";

        private const string PREF_BGM = "Settings_BGM";
        private const string PREF_SFX = "Settings_SFX";
        private const string PREF_VOCAL = "Settings_Vocal";

        private void Start()
        {
            if (btnClose != null) btnClose.onClick.AddListener(Close);

            // Load saved values (default 0.75 = 75% volume)
            float bgmVal = PlayerPrefs.GetFloat(PREF_BGM, 0.75f);
            float sfxVal = PlayerPrefs.GetFloat(PREF_SFX, 0.75f);
            float vocalVal = PlayerPrefs.GetFloat(PREF_VOCAL, 0.75f);

            SetupSlider(sliderBGM, bgmVal, PARAM_BGM, PREF_BGM);
            SetupSlider(sliderSFX, sfxVal, PARAM_SFX, PREF_SFX);
            SetupSlider(sliderVocal, vocalVal, PARAM_VOCAL, PREF_VOCAL);
        }

        private void SetupSlider(Slider slider, float initialValue, string mixerParam, string prefKey)
        {
            if (slider == null) return;

            slider.minValue = 0.001f; // Avoid Log10(0)
            slider.maxValue = 1f;
            slider.value = initialValue;

            ApplyVolume(mixerParam, initialValue);

            slider.onValueChanged.AddListener((value) =>
            {
                ApplyVolume(mixerParam, value);
                PlayerPrefs.SetFloat(prefKey, value);
            });
        }

        private void ApplyVolume(string mixerParam, float linearValue)
        {
            if (audioMixer == null) return;
            // Convert linear 0-1 to dB (-80 to 0)
            float dB = Mathf.Log10(Mathf.Max(linearValue, 0.001f)) * 20f;
            audioMixer.SetFloat(mixerParam, dB);
        }

        private void Close()
        {
            PlayerPrefs.Save();
            gameObject.SetActive(false);
        }
    }
}
