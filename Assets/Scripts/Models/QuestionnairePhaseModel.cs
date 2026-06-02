using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class QuestionnairePhaseModel
    {
        public readonly string Name;
        public readonly string TipLocalizationKey;
        public readonly QuestionModel[] Questions;

        public QuestionnairePhaseModel(string name, string tipLocalizationKey, QuestionModel[] questions)
        {
            Name = name;
            TipLocalizationKey = tipLocalizationKey;
            Questions = questions;
        }
    }
}