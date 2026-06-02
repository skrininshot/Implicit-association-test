using Services;
using TMPro;
using UnityEngine;

public class LocalizedTMP : MonoBehaviour
{
    [SerializeField] private string localizationKey;
    [SerializeField] private TMP_Text text;

    private void Awake()
    {
        text.text = LocalizationService.GetValue(localizationKey);
    }
}
