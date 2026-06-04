using Models;
using UnityEngine;

namespace Services
{
    public class RaceCorrectAnswerChecker : ICorrectAnswerChecker
    {
        public bool IsCorrectAnswer(string phase, QuestionModel questionModel, AnswerOptionModel answerModel)
        {
            switch (phase)
            {
                case "Congruent": // congruent phase
                    return ((answerModel.Name == "left" && (questionModel.Characteristics[0].Type == "white" || questionModel.Characteristics[0].Type == "positive")) ||
                            (answerModel.Name == "right" && (questionModel.Characteristics[0].Type == "black" || questionModel.Characteristics[0].Type == "negative")));
                
                case "Incongruent": // incongruent phase
                    return ((answerModel.Name == "left" && (questionModel.Characteristics[0].Type == "black" || questionModel.Characteristics[0].Type == "positive")) || 
                            (answerModel.Name == "right" && (questionModel.Characteristics[0].Type == "white" || questionModel.Characteristics[0].Type == "negative")));
                default:
                    Debug.LogError("Wrong phase number");
                    return false;
            }
        }
    }
}