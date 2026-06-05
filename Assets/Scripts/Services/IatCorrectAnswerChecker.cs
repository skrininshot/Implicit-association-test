using System.Linq;
using Models;

namespace Services
{
    public class IatCorrectAnswerChecker : ICorrectAnswerChecker
    {
        private readonly IatConfig _config;

        public IatCorrectAnswerChecker(IatConfig config)
        {
            _config = config;
        }

        public bool IsCorrectAnswer(string phaseName, QuestionModel question, AnswerOptionModel answer)
        {
            var phaseConfig = _config.Phases.FirstOrDefault(p => p.PhaseName == phaseName);
            if (phaseConfig == null) return false;

            string stimulusCategory = question.Category.Type;
            if (answer.Name == "left")
                return phaseConfig.LeftCategories.Contains(stimulusCategory);
            else if (answer.Name == "right")
                return phaseConfig.RightCategories.Contains(stimulusCategory);
            return false;
        }
    }
}