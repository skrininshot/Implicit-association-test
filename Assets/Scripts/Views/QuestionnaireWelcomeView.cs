using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class QuestionnaireWelcomeView : ScreenView
    {
        [field: SerializeField] public Button ButtonAccept { get; private set; }
    }
}