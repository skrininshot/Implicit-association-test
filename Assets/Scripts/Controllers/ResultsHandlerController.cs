using Models;

namespace Controllers
{
    public class ResultsHandlerController : IResultsHandlerController
    {
        private readonly IAnswersCollectController _answersCollectController;
        
        public QuestionnaireResultsModel GetResults()
        {
            throw new System.NotImplementedException();
        }
    }
}