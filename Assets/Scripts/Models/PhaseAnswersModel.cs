using System.Collections.Generic;

namespace Models
{
    public class PhaseAnswersModel
    {
        public readonly PhaseModel Phase;
        
        public PhaseAnswersModel(PhaseModel phaseModel)
        {
            Phase = phaseModel;
        }
        
        private readonly List<QuestionAnswerModel> _questionsAnswers = new();

        public IReadOnlyCollection<QuestionAnswerModel> QuestionsAnswers => _questionsAnswers;

        public void Add(QuestionAnswerModel phase) => _questionsAnswers.Add(phase);
        
        public void Reset() => _questionsAnswers.Clear();
    }
}