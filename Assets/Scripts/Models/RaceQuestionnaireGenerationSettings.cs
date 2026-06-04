using UnityEngine;

namespace Models
{
    [CreateAssetMenu(fileName = "Race Questionnaire Generation Settings", menuName = "Questionnaire/New Race Questionnaire Generation Settings")]
    public class RaceQuestionnaireGenerationSettings : QuestionnaireGenerationSettings
    {
        [field: SerializeField] private RaceQuestionnaireGenerationSettingsModel settings;
        
        public override IQuestionnaireGenerationSettings Get() => settings;
    }

    [System.Serializable]
    public class RaceQuestionnaireGenerationSettingsModel : IQuestionnaireGenerationSettings
    {
        [field: SerializeField] public int questionsPerPhase = 25;
        [field: SerializeField] public int maxContinuousSameCharacteristicType = 3;
    }
}