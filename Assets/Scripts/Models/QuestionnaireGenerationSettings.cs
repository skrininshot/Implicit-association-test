using UnityEngine;

namespace Models
{
    public abstract class QuestionnaireGenerationSettings : ScriptableObject
    {
        public abstract IQuestionnaireGenerationSettings Get();
    }
}