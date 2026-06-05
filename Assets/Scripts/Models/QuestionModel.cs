namespace Models
{
    [System.Serializable]
    public class QuestionModel
    {
        public readonly Stimulus Category;

        public QuestionModel(Stimulus category)
        {
            Category = category;
        }
    }
}