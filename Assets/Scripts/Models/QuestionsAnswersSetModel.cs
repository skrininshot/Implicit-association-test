using System.Collections.Generic;

namespace Models
{
    public class QuestionsAnswersDataSetModel
    {
        private readonly List<QuestionAnswerModel> _data = new();

        public IReadOnlyCollection<QuestionAnswerModel> Data => _data;

        public void Add(QuestionAnswerModel questionAnswerModel) => _data.Add(questionAnswerModel);
        
        public void Reset() => _data.Clear();
    }
}