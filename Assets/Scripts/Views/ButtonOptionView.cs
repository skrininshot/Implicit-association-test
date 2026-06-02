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

        public void Initialize(AnswerOptionModel answerOption)
        {
            Image.color = answerOption.ViewColor;
            Text.text = answerOption.Content;
        }
    }
}