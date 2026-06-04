using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Services;

public class RaceQuestionnaireResultsHandler : IQuestionnaireResultsHandler
{
    public IQuestionnaireResultsModel GetResults(PhasesQuestionsAnswersModel phases)
    {
        if (phases.PhasesAnswers.Count != 2)
            throw new ArgumentException("Must provide two number of phases");

        var congruentPhase = phases.PhasesAnswers.FirstOrDefault(p => p.Phase.Name == "Congruent");
        var incongruentPhase = phases.PhasesAnswers.FirstOrDefault(p => p.Phase.Name == "Incongruent");

        if (congruentPhase == null || incongruentPhase == null)
            throw new InvalidOperationException("Не найдены обе фазы IAT");

        var phase0 = congruentPhase.QuestionsAnswers;
        var phase1 = incongruentPhase.QuestionsAnswers;
        
        const double minTime = 0.3;
        const double maxTime = 3.0;
        
        var correctTimesPhase0 = phase0
            .Where(q => q.IsCorrect && q.Time >= minTime && q.Time <= maxTime)
            .Select(q => q.Time)
            .ToList();

        var correctTimesPhase1 = phase1
            .Where(q => q.IsCorrect && q.Time >= minTime && q.Time <= maxTime)
            .Select(q => q.Time)
            .ToList();
        
        if (correctTimesPhase0.Count < 4 || correctTimesPhase1.Count < 4)
            return new RaceQuestionnaireResultsModel(
                dScore: double.NaN,
                avgLatencyDiff: double.NaN,
                accuracyPhase0: double.NaN,
                accuracyPhase1: double.NaN,
                interpretation: "Not enough values"
            );
        
        double mean0 = correctTimesPhase0.Average();
        double mean1 = correctTimesPhase1.Average();
        
        var allCorrectTimes = correctTimesPhase0.Concat(correctTimesPhase1).ToList();
        double meanAll = allCorrectTimes.Average();
        double sumSquaredDeviations = allCorrectTimes.Sum(t => (t - meanAll) * (t - meanAll));
        double sd = Math.Sqrt(sumSquaredDeviations / allCorrectTimes.Count);
        
        double dScore = (mean1 - mean0) / sd;
        
        double avgLatencyDiffMs = (mean1 - mean0) * 1000.0;
        
        double accuracy0 = (double)correctTimesPhase0.Count / 
                           phase0.Count(q => q.Time >= minTime && q.Time <= maxTime);
        double accuracy1 = (double)correctTimesPhase1.Count / 
                           phase1.Count(q => q.Time >= minTime && q.Time <= maxTime);
        
        string interpretation;
        if (dScore > 0.15)
            interpretation = "race_result_interpretation_white";
        else if (dScore < -0.15)
            interpretation = "race_result_interpretation_black";
        else
            interpretation = "race_result_interpretation_neutral";

        return new RaceQuestionnaireResultsModel(
            dScore: dScore,
            avgLatencyDiff: avgLatencyDiffMs,
            accuracyPhase0: accuracy0,
            accuracyPhase1: accuracy1,
            interpretation: interpretation
        );
    }
}