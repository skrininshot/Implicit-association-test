using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Services
{
    public class IatQuestionnaireGenerator : IQuestionnaireGenerator
{
    private readonly IatConfig _config;
    private static readonly System.Random Rng = new();

    public IatQuestionnaireGenerator(IatConfig config)
    {
        _config = config;
    }

    public QuestionnaireModel Generate()
    {
        var phaseConfigs = new[] { _config.Phases[0], _config.Phases[1] };
        if (Rng.Next(2) == 1) (phaseConfigs[0], phaseConfigs[1]) = (phaseConfigs[1], phaseConfigs[0]);

        var phases = new PhaseQuestions[2];
        for (int i = 0; i < 2; i++)
        {
            var pc = phaseConfigs[i];
            var questions = GeneratePhaseQuestions(pc);
            phases[i] = new PhaseQuestions(
                new PhaseModel(pc.PhaseName, pc.InstructionLocalizedKey),
                questions
            );
        }

        var answerOptions = new[]
        {
            new AnswerOptionModel("left", Color.magenta, string.Empty, KeyCode.Q),
            new AnswerOptionModel("right", Color.cyan, string.Empty, KeyCode.E)
        };

        return new QuestionnaireModel(_config.TestId, answerOptions, phases);
    }

    private QuestionModel[] GeneratePhaseQuestions(PhaseConfig phaseConfig)
    {
        int totalCategories = _config.Categories.Length;
        int perCategory = _config.QuestionsPerPhase / totalCategories;
        if (_config.QuestionsPerPhase % totalCategories != 0)
            throw new ArgumentException("QuestionsPerPhase must be divisible by number of categories");
        
        var categoryList = new List<StimulusCategory>(_config.QuestionsPerPhase);
        foreach (var cat in _config.Categories)
            for (int i = 0; i < perCategory; i++)
                categoryList.Add(cat);

        bool valid = false;
        int attempts = 0;
        do
        {
            Shuffle(categoryList);
            valid = CheckMaxConsecutiveType(categoryList, _config.MaxConsecutiveSameType);
            attempts++;
        } while (!valid && attempts < 1000);
        if (!valid) throw new InvalidOperationException("Cannot satisfy consecutive type constraint");
        
        var questions = new QuestionModel[_config.QuestionsPerPhase];
        var lastUsedAsset = new Dictionary<string, string>();

        for (int i = 0; i < categoryList.Count; i++)
        {
            var cat = categoryList[i];
            string assetKey = GetRandomAsset(cat, lastUsedAsset.ContainsKey(cat.Name) ? lastUsedAsset[cat.Name] : null);
            lastUsedAsset[cat.Name] = assetKey;

            var stimulus = new Stimulus();
            if (cat.Type == StimulusType.Image)
                stimulus.Set("race", cat.Name, assetKey, StimulusType.Image);
            else
                stimulus.Set("word", cat.Polarity, assetKey, StimulusType.Text);

            questions[i] = new QuestionModel(stimulus);
        }

        return questions;
    }

    private string GetRandomAsset(StimulusCategory category, string exclude)
    {
        int index;
        string asset;
        do
        {
            index = Rng.Next(1, category.AssetCount + 1);
            asset = $"{category.AssetPrefix}{index}";
        } while (asset == exclude);
        return asset;
    }

    private bool CheckMaxConsecutiveType(List<StimulusCategory> list, int max)
    {
        int streak = 1;
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].Type == list[i - 1].Type)
            {
                streak++;
                if (streak > max) return false;
            }
            else streak = 1;
        }
        return true;
    }

    private void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}
}