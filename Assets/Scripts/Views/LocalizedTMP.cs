using Services;
using TMPro;
using UnityEngine;

public class LocalizedTMP : MonoBehaviour
{
    [SerializeField] private string localizationKey;
    [SerializeField] private TMP_Text text;
    
    private void OnEnable()
    {
        UpdateText();
        LocalizationService.OnLanguageChanged += UpdateText;
    }

    private void OnDisable()
    {
        LocalizationService.OnLanguageChanged -= UpdateText;
    }

    private void UpdateText()
    {
        text.text = LocalizationService.GetValue(localizationKey);
    }
}
