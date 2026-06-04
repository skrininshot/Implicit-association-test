using Models;

namespace Services
{
    public interface ICorrectAnswerChecker
    {
        public bool IsCorrectAnswer(string phaseName, QuestionModel questionModel, AnswerOptionModel answerModel);
    }
}