using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class PhaseTipView : View
    {
        [SerializeField] private TMP_Text text;
        [field: SerializeField] public Button AcceptButton { get; private set; }

        private string _tipLocalizationKey;
        
        public void SetTip(string localizationTextKey)
        {
            _tipLocalizationKey = localizationTextKey;
            UpdateTip();
        }
        
        private void OnEnable()
        {
            LocalizationService.OnLanguageChanged += UpdateTip;
        }

        private void OnDisable()
        {
            LocalizationService.OnLanguageChanged -= UpdateTip;
        }

        private void UpdateTip()
        {
            text.text = LocalizationService.GetValue(_tipLocalizationKey);

        }
    }
}