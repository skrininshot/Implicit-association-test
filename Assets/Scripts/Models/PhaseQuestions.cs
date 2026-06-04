namespace Models
{
    [System.Serializable]
    public class PhaseQuestions
    {
        public readonly PhaseModel Phase;
        public readonly QuestionModel[] Questions;

        public PhaseQuestions(PhaseModel phase, QuestionModel[] questions)
        {
            Phase = phase;
            Questions = questions;
        }
    }
}