using Models;

namespace Controllers
{
    public interface IResultsHandlerController
    {
        public IQuestionnaireResultsModel GetResults();
    }
}