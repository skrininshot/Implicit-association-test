using Models;

namespace Controllers
{
    public interface IResultsHandlerController
    {
        public QuestionnaireResultsModel GetResults();
    }
}