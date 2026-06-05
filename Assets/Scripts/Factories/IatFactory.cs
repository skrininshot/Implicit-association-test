using Models;
using Services;

namespace Factories
{
    public class IatFactory
    {
        public IQuestionnaireGenerator CreateGenerator(IatConfig config) =>
            new IatQuestionnaireGenerator(config);

        public ICorrectAnswerChecker CreateChecker(IatConfig config) =>
            new IatCorrectAnswerChecker(config);

        public IQuestionnaireResultsHandler CreateResultsHandler(IatConfig config) =>
            new IatResultsHandler(config);
    }
}