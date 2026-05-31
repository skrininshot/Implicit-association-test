namespace Models
{
    [System.Serializable]   
    public class QuestionnaireModel
    {
        public QuestionModel[] Questions { get; private set; }
        public AnswerOptionModel[] AnswerOptions { get; private set; }
        
        public QuestionnaireModel(QuestionModel[] questions, AnswerOptionModel[] answerOptions)
        {
            Questions = questions;
            AnswerOptions = answerOptions;
        }
    }
}