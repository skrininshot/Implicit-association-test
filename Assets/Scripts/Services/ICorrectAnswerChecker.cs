using Models;

namespace Services
{
    public interface ICorrectAnswerChecker
    {
        public bool IsCorrectAnswer(int phase, QuestionModel questionModel, AnswerOptionModel answerModel);
    }
}