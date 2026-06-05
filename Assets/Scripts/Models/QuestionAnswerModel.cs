namespace Models
{
    [System.Serializable]
    public class QuestionAnswerModel
    {
        public readonly QuestionModel Question;
        public readonly AnswerOptionModel AnswerOption;
        public readonly bool IsCorrect;
        public readonly double Time;
        
        public QuestionAnswerModel(QuestionModel question, AnswerOptionModel answerOption, bool isCorrect, double time)
        {
            Question = question;
            AnswerOption = answerOption;
            IsCorrect = isCorrect;
            Time = time;
        }
    }
}