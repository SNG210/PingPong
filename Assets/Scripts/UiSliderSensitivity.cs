using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiSliderSensitivity : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityValueText;
    [SerializeField] private Player player;

    [Header("Sensitivity Settings")]
    [SerializeField] private float minSensitivity = 50f;
    [SerializeField] private float maxSensitivity = 300f;

    private const string SensitivityPrefKey = "PlayerSensitivity";

    private void Start()
    {
        sensitivitySlider.minValue = minSensitivity;
        sensitivitySlider.maxValue = maxSensitivity;

        float savedSensitivity = PlayerPrefs.HasKey(SensitivityPrefKey) ? PlayerPrefs.GetFloat(SensitivityPrefKey) : 100f;

        sensitivitySlider.value = savedSensitivity;
        player.SetSensitivity(savedSensitivity);

        UpdateSensitivityValueText(savedSensitivity);

        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    }

    private void OnSensitivityChanged(float value)
    {
        player.SetSensitivity(value);

        UpdateSensitivityValueText(value);
    }
    private void UpdateSensitivityValueText(float value)
    {
        sensitivityValueText.text = $"{value:F0}"; 
    }
}
