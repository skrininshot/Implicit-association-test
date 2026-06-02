using Models;
using UnityEngine;
using Random = System.Random;

namespace Services
{
    public class RaceQuestionnaireGenerator : IQuestionnaireGenerator
    {
        public QuestionnaireModel Generate()
        {
            int phaseCount = 2;
            int questionPerPhaseCount = 25;
                
            var phases = new QuestionnairePhaseModel[2];

            for (int i = 0; i < phaseCount; i++)
            {
                QuestionModel[] questions = new QuestionModel[questionPerPhaseCount];
            
                for (int j = 0; j < questionPerPhaseCount; j++)
                {
                    bool isWhite = new Random().Next(1) == 1;
                    int humanIndex = new Random().Next(1, 50);
                    
                    bool isPositiveWord = new Random().Next(1) == 1;
                    int wordIndex = new Random().Next(1, 50);

                    var raceName = isWhite ? "white" : "black";
                    var wordPolarity = isPositiveWord ? "positive" : "negative";
                    
                    var imagePath = $"hm_{raceName}_{humanIndex}";
                    var textPath = $"ambivalent_word_{wordPolarity}_{wordIndex}";

                    CharacteristicModel[] characteristicModel =
                    {
                        new ("race", raceName, imagePath, CharacteristicType.Image),
                        new ("word", wordPolarity, textPath, CharacteristicType.Text)
                    };
                
                    var questionModel = new QuestionModel(characteristicModel);
                    questions[j] = questionModel;
                }

                var phaseName = (i + 1).ToString();
                phases[i] = new QuestionnairePhaseModel(phaseName, $"race_phase{i}_instruction", questions);
            }
        
            AnswerOptionModel[] answerOptions =
            {
                new 
                (
                    "left", 
                    Color.magenta,
                    string.Empty
                ),
                new 
                (
                    "right", 
                    Color.cyan,
                    string.Empty
                )
            };
            
            return new ("race", answerOptions, phases);
        }
    }
}