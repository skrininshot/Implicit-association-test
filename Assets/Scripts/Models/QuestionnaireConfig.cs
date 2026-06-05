using UnityEngine;
using Views;

namespace Models
{
    [CreateAssetMenu(fileName = "Questionnaire Config", menuName = "IAT/New Questionnaire Config")]
    public class QuestionnaireConfig : ScriptableObject
    {
        [field: SerializeField] public IatConfig[] IatConfigs { get; private set; }
    }
}