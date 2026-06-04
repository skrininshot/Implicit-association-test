using Models;
using UnityEngine;

namespace Services
{
    public class RaceQuestionnaireGenerator : IQuestionnaireGenerator
    {
        private static readonly System.Random Random = new();
        
        public QuestionnaireModel Generate(IQuestionnaireGenerationSettings settings)
        {
            var raceSettings = settings as RaceQuestionnaireGenerationSettingsModel;
            
            var phases = new QuestionnairePhaseModel[2];

            for (int i = 0; i < 2; i++)
            {
                var questions = GeneratePhaseQuestions(i, raceSettings.questionsPerPhase, raceSettings.maxContinuousSameCharacteristicType);
                var phaseName = i == 0 ? "Congruent" : "Incongruent";
                phases[i] = new QuestionnairePhaseModel(
                    phaseName,
                    $"race_phase{i}_instruction",
                    questions
                );
            }

            var answerOptions = new[]
            {
                new AnswerOptionModel("left", Color.magenta, string.Empty),
                new AnswerOptionModel("right", Color.cyan, string.Empty)
            };

            return new QuestionnaireModel("race", answerOptions, phases);
        }

        private QuestionModel[] GeneratePhaseQuestions(int phaseIndex, int questionsPerPhase, int maxContinuousSameType)
        {
            var questions = new QuestionModel[questionsPerPhase];

            bool prevIsWord = false;
            int consecutiveSameType = 0;
            string lastWordKey = null;
            string lastImagePath = null;

            for (int j = 0; j < questionsPerPhase; j++)
            {
                bool isWord = Random.Next(2) == 1;
                
                if (isWord == prevIsWord)
                    consecutiveSameType++;
                else
                    consecutiveSameType = 1;

                if (consecutiveSameType > maxContinuousSameType)
                {
                    isWord = !isWord;
                    consecutiveSameType = 1;
                }

                CharacteristicModel characteristic;

                if (isWord)
                {
                    string key, polarity;
                    do
                    {
                        (key, polarity) = GetRandomWord();
                    } while (key == lastWordKey);

                    lastWordKey = key;
                    lastImagePath = null;
                    characteristic = new CharacteristicModel();
                    characteristic.Set("word", polarity, key, CharacteristicType.Text);
                }
                else
                {
                    string path, race;
                    do
                    {
                        (path, race) = GetRandomFace();
                    } while (path == lastImagePath);

                    lastImagePath = path;
                    lastWordKey = null;
                    characteristic = new CharacteristicModel();
                    characteristic.Set("race", race, path, CharacteristicType.Image);
                }

                questions[j] = new QuestionModel(new[] { characteristic });
                prevIsWord = isWord;
            }

            return questions;
        }

        private (string key, string polarity) GetRandomWord()
        {
            bool positive = Random.Next(2) == 1;
            int index = Random.Next(1, 51);
            string polarity = positive ? "positive" : "negative";
            string key = $"ambivalent_word_{polarity}_{index}";
            return (key, polarity);
        }

        private (string path, string race) GetRandomFace()
        {
            bool isWhite = Random.Next(2) == 1;
            int index = Random.Next(1, 51);
            string race = isWhite ? "white" : "black";
            string path = $"hm_{race}_{index}";
            return (path, race);
        }
    }
}