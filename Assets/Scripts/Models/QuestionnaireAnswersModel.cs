using System.Collections.Generic;

namespace Models
{
    [System.Serializable]
    public class QuestionnaireAnswersModel
    {
        private Dictionary<QuestionModel, AnswerOptionModel> _answers = new();

        public void RegisterAnswer(QuestionModel question, AnswerOptionModel answerOption)
        {
            _answers.Add(question, answerOption);
        }

        public IReadOnlyDictionary<QuestionModel, AnswerOptionModel> GetAnswers => _answers;
    }
}