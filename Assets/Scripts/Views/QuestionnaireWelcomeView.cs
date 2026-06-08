using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class QuestionnaireWelcomeView : View
    {
        [field: SerializeField] public Button ButtonAccept { get; private set; }
        [field: SerializeField] public Button ButtonBack { get; private set; }
    }
}