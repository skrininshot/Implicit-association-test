using Models;

namespace Controllers
{
    public interface IAnswersCollectController
    {
        public void RegisterAnswer(QuestionModel questionModel, AnswerOptionModel answerOptionModel, double time);
        public void Reset();
    }
}