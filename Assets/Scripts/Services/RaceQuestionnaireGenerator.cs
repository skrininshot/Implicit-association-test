using System;
using System.Collections.Generic;
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
            
            if (raceSettings.questionsPerPhase % 4 != 0)
                throw new ArgumentException("questionsPerPhase must be a multiple of 4 for equal proportions");

            var congruentPhase = CreatePhase(new PhaseModel("Congruent", "race_instruction_congruent"), raceSettings);
            var incongruentPhase = CreatePhase(new PhaseModel("Incongruent", "race_instruction_incongruent"), raceSettings);

            var phases = new PhaseQuestions[2];
            if (Random.Next(2) == 0)
            {
                phases[0] = congruentPhase;
                phases[1] = incongruentPhase;
            }
            else
            {
                phases[0] = incongruentPhase;
                phases[1] = congruentPhase;
            }

            var answerOptions = new[]
            {
                new AnswerOptionModel("left", Color.magenta, string.Empty, KeyCode.Q),
                new AnswerOptionModel("right", Color.cyan, string.Empty, KeyCode.E)
            };

            return new QuestionnaireModel("race", answerOptions, phases);
        }

        private PhaseQuestions CreatePhase(PhaseModel phase, RaceQuestionnaireGenerationSettingsModel settings)
        {
            var questions = GeneratePhaseQuestions(settings.questionsPerPhase, settings.maxContinuousSameCharacteristicType);
            return new PhaseQuestions(phase, questions);
        }
        
        private QuestionModel[] GeneratePhaseQuestions(int questionsPerPhase, int maxContinuousSameType)
        {
            int countPerCategory = questionsPerPhase / 4;
            
            var categoryList = new List<string>(questionsPerPhase);
            categoryList.AddRange(Repeat("white_face", countPerCategory));
            categoryList.AddRange(Repeat("black_face", countPerCategory));
            categoryList.AddRange(Repeat("positive_word", countPerCategory));
            categoryList.AddRange(Repeat("negative_word", countPerCategory));
            
            bool valid = false;
            int attempts = 0;
            const int maxAttempts = 1000;
            do
            {
                Shuffle(categoryList);
                valid = CheckMaxConsecutiveType(categoryList, maxContinuousSameType);
                attempts++;
            } while (!valid && attempts < maxAttempts);

            if (!valid)
                throw new InvalidOperationException("Failed to generate a sequence with the given consecutive type constraint");
            
            var questions = new QuestionModel[questionsPerPhase];
            string lastWordKey = null;
            string lastImagePath = null;

            for (int j = 0; j < questionsPerPhase; j++)
            {
                string cat = categoryList[j];
                CharacteristicModel characteristic;

                if (cat == "white_face" || cat == "black_face")
                {
                    string race = cat == "white_face" ? "white" : "black";
                    string path = GetRandomFace(race, lastImagePath);
                    lastImagePath = path;
                    lastWordKey = null;
                    characteristic = new CharacteristicModel();
                    characteristic.Set("race", race, path, CharacteristicType.Image);
                }
                else
                {
                    string polarity = cat == "positive_word" ? "positive" : "negative";
                    string key = GetRandomWord(polarity, lastWordKey);
                    lastWordKey = key;
                    lastImagePath = null;
                    characteristic = new CharacteristicModel();
                    characteristic.Set("word", polarity, key, CharacteristicType.Text);
                }

                questions[j] = new QuestionModel(new[] { characteristic });
            }

            return questions;
        }

        private string GetRandomWord(string polarity, string excludeKey)
        {
            int index;
            string key;
            do
            {
                index = Random.Next(1, 51);
                key = $"ambivalent_word_{polarity}_{index}";
            } while (key == excludeKey);
            return key;
        }

        private string GetRandomFace(string race, string excludePath)
        {
            int index;
            string path;
            do
            {
                index = Random.Next(1, 51);
                path = $"hm_{race}_{index}";
            } while (path == excludePath);
            return path;
        }
        
        private bool CheckMaxConsecutiveType(List<string> categoryList, int maxSameType)
        {
            int currentStreak = 1;
            for (int i = 1; i < categoryList.Count; i++)
            {
                bool prevIsImage = IsImageCategory(categoryList[i - 1]);
                bool currIsImage = IsImageCategory(categoryList[i]);
                if (prevIsImage == currIsImage)
                {
                    currentStreak++;
                    if (currentStreak > maxSameType)
                        return false;
                }
                else
                {
                    currentStreak = 1;
                }
            }
            return true;
        }

        private bool IsImageCategory(string cat) => cat == "white_face" || cat == "black_face";

        private List<string> Repeat(string value, int count)
        {
            var list = new List<string>(count);
            for (int i = 0; i < count; i++) list.Add(value);
            return list;
        }

        private void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}