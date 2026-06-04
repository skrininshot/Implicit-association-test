using System;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class ButtonOptionView : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public Image Image { get; private set; }
        [field: SerializeField] public TMP_Text Text { get; private set; }

        private KeyCode _keyCode;
        
        public void Initialize(AnswerOptionModel answerOption)
        {
            Image.color = answerOption.ViewColor;
            Text.text = (!string.IsNullOrEmpty(answerOption.Content) ? answerOption.Content + " " : string.Empty) + $"({answerOption.KeyCode})";
            _keyCode = answerOption.KeyCode;
        }

        private void Update()
        {
            if (Input.GetKeyDown(_keyCode)) // replace with new system input event
            {
                Button.onClick.Invoke();
            }
        }
    }
}