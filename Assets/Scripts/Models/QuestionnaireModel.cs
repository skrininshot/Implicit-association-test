using UnityEngine;

namespace Models
{
    [System.Serializable]   
    public class QuestionnaireModel
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public PhaseQuestions[] Phases { get; private set; }
        
        public AnswerOptionModel[] AnswerOptions { get; private set; }
        
        public QuestionnaireModel(string name, AnswerOptionModel[] answerOptions, PhaseQuestions[] phases)
        {
            Name = name;
            Phases = phases;
            AnswerOptions = answerOptions;
        }
    }
}