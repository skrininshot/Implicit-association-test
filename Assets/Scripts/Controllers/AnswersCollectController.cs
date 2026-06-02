using Models;

namespace Controllers
{
    public class AnswersCollectController : IAnswersCollectController
    {
        private readonly QuestionsAnswersDataSetModel _questionsAnswersDataSet;
        
        public AnswersCollectController(QuestionsAnswersDataSetModel questionAnswerModel)
        {
            _questionsAnswersDataSet = questionAnswerModel;
        }
        
        public void RegisterAnswer(QuestionModel questionModel, AnswerOptionModel answerOptionModel, double time)
        {
            var data = new QuestionAnswerModel(questionModel, answerOptionModel, time);
            _questionsAnswersDataSet.Add(data);
        }

        public void Reset()
        {
            _questionsAnswersDataSet.Reset();
        }
    }
}