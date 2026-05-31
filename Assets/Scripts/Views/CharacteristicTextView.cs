using Services;
using TMPro;
using UnityEngine;

namespace Views
{
    public class CharacteristicTextView : CharacteristicView
    {
        [SerializeField] private TMP_Text text;
        
        public override void SetData(string data)
        {
            text.text = LocalizationService.GetValue(data);
        }
    }
}