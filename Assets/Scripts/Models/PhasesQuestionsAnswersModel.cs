using System.Collections.Generic;

namespace Models
{
    public class PhasesQuestionsAnswersModel
    {
        private readonly List<QuestionsAnswersModel> _phases = new();

        public IReadOnlyList<QuestionsAnswersModel> Phases => _phases;

        public void Add(QuestionsAnswersModel phase) => _phases.Add(phase);
        
        public void Reset()
        {
            foreach (var phase in _phases)
            {
                phase.Reset();
            }
            
            _phases.Clear();
        }
    }
}