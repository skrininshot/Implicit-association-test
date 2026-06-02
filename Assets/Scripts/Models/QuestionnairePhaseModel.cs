using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class QuestionnairePhaseModel
    {
        public readonly string Name;
        public readonly QuestionModel[] Questions;

        public QuestionnairePhaseModel(string name, QuestionModel[] questions)
        {
            Name = name;
            Questions = questions;
        }
    }
}