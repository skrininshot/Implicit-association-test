using Models;

namespace Services
{
    public interface IQuestionnaireResultsHandler
    {
        public IQuestionnaireResultsModel GetResults(PhasesQuestionsAnswersModel phases);
    }
}