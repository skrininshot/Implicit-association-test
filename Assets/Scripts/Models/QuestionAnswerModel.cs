namespace Models
{
    [System.Serializable]
    public class QuestionAnswerModel
    {
        public readonly QuestionModel Question;
        public readonly AnswerOptionModel AnswerOption;
        public readonly double Time;
        
        public QuestionAnswerModel(QuestionModel question, AnswerOptionModel answerOption, double time)
        {
            Question = question;
            AnswerOption = answerOption;
            Time = time;
        }

    }
}