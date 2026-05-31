using Models;
using UnityEngine;

namespace Services
{
    public static class QuestionnaireGenerator
    {
        public static QuestionnaireModel Generate()
        {
            QuestionModel[] questions = new[]
            {
                new QuestionModel
                (
                    new CharacteristicModel[] 
                    {
                        new CharacteristicModel("img_test_path", CharacteristicType.Image),
                        new CharacteristicModel("test_key", CharacteristicType.Text),
                    }
                )
            };
        
            AnswerOptionModel[] answerOptions = new[]
            {
                new AnswerOptionModel
                (
                    string.Empty, 
                    Color.blue
                ),
                new AnswerOptionModel
                (
                    string.Empty, 
                    Color.red
                )
            };

            return new (questions, answerOptions);
        }
    }
}