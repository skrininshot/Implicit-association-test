using Models;

namespace Services
{
    public interface IQuestionnaireGenerator
    {
        public QuestionnaireModel Generate();
    }
}