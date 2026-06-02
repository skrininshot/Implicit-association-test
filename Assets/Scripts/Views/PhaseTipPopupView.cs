using System;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class PhaseTipPopupView : View
    {
        [SerializeField] private TMP_Text text;
        [field: SerializeField] public Button AcceptButton { get; private set; }

        public void SetTip(string localizationTextKey)
        {
            text.text = LocalizationService.GetValue(localizationTextKey);
        }
    }
}