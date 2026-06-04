using Models;
using Services;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Views;

public class RaceQuestionnaireResultsView : View
{
    [field: SerializeField] public Button ButtonAccept { get; private set; }

    [SerializeField] private TMP_Text dScoreText;
    [SerializeField] private TMP_Text latencyDiffText;
    [SerializeField] private TMP_Text accuracyPhase0Text;
    [SerializeField] private TMP_Text accuracyPhase1Text;
    [SerializeField] private TMP_Text interpretationText;

    private RaceQuestionnaireResultsModel _raceResults;

    private void OnEnable()
    {
        LocalizationService.OnLanguageChanged += UpdateAllTexts;
    }

    private void OnDisable()
    {
        LocalizationService.OnLanguageChanged -= UpdateAllTexts;
    }

    public void SetResults(IQuestionnaireResultsModel results)
    {
        _raceResults = results as RaceQuestionnaireResultsModel;
        UpdateAllTexts();
    }

    private void UpdateAllTexts()
    {
        if (_raceResults == null) return;

        // D-score
        if (!double.IsNaN(_raceResults.DScore))
        {
            dScoreText.text = $"{LocalizationService.GetValue("race_result_dscore")}: {_raceResults.DScore:F2}";
        }
        else
        {
            dScoreText.text = LocalizationService.GetValue("race_result_dscore_not_enought");
        }
        AppendDescription(dScoreText, "race_result_dscore_description");

        // Average latency difference
        string latencyValue = double.IsNaN(_raceResults.AvgLatencyDiffMs)
            ? "—"
            : $"{_raceResults.AvgLatencyDiffMs:F1}";
        latencyDiffText.text =
            $"{LocalizationService.GetValue("race_result_average_latency")}: {latencyValue} {LocalizationService.GetValue("milliseconds_short")}";
        AppendDescription(latencyDiffText, "race_result_avg_latency_description");

        // Accuracy phase 0 (easy)
        accuracyPhase0Text.text =
            $"{LocalizationService.GetValue("race_result_accuracy_phase_0")}: {_raceResults.AccuracyPhase0 * 100:F1}%";
        AppendDescription(accuracyPhase0Text, "race_result_accuracy_description");

        // Accuracy phase 1 (hard)
        accuracyPhase1Text.text =
            $"{LocalizationService.GetValue("race_result_accuracy_phase_1")}: {_raceResults.AccuracyPhase1 * 100:F1}%";
        AppendDescription(accuracyPhase1Text, "race_result_accuracy_description");

        // Interpretation
        interpretationText.text = LocalizationService.GetValue(_raceResults.Interpretation);
        AppendDescription(interpretationText, "race_result_interpretation_description");
    }

    private void AppendDescription(TMP_Text textField, string localizationKey)
    {
        string description = LocalizationService.GetValue(localizationKey);
        if (!string.IsNullOrEmpty(description))
        {
            textField.text += $"\n<size=85%><color=#AAAAAA>{description}</color></size>";
        }
    }
}