using Models;
using UnityEngine;

namespace Services
{
    public class RaceCorrectAnswerChecker : ICorrectAnswerChecker
    {
        public bool IsCorrectAnswer(int phase, QuestionModel questionModel, AnswerOptionModel answerModel)
        {
            switch (phase)
            {
                case 0: // congruent phase
                    return ((answerModel.Name == "left" && questionModel.Characteristics[0].Type == "white" || questionModel.Characteristics[1].Type == "positive") ||
                            answerModel.Name == "right" && questionModel.Characteristics[0].Type == "black" || questionModel.Characteristics[1].Type == "negative");
                
                case 1: // incongruent phase
                    return ((answerModel.Name == "left" && questionModel.Characteristics[0].Type == "black" || questionModel.Characteristics[1].Type == "positive") || 
                            (answerModel.Name == "right" && questionModel.Characteristics[0].Type == "white" || questionModel.Characteristics[1].Type == "negative"));
                default:
                    Debug.LogError("Wrong phase number");
                    return false;
            }
        }
    }
}