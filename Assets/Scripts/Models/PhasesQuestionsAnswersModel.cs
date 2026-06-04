using System.Collections.Generic;

namespace Models
{
    public class PhasesQuestionsAnswersModel
    {
        private readonly List<PhaseAnswersModel> _phasesAnswers = new();

        public IReadOnlyList<PhaseAnswersModel> PhasesAnswers => _phasesAnswers;

        public void Add(PhaseAnswersModel phase) => _phasesAnswers.Add(phase);
        
        public void Reset()
        {
            foreach (var phase in _phasesAnswers)
            {
                phase.Reset();
            }
            
            _phasesAnswers.Clear();
        }
    }
}