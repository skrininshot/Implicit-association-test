using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class QuestionnaireResultsView : ScreenView
    {
        [field: SerializeField] public Button ButtonAccept { get; private set; }
    }
}