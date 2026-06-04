using Models;
using Services;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Views; // Если используете TextMeshPro, иначе using UnityEngine.UI;

public class RaceQuestionnaireResultsView : View
{
    [field: SerializeField] public Button ButtonAccept { get; private set; }

    [SerializeField] private TMP_Text dScoreText;
    [SerializeField] private TMP_Text latencyDiffText;
    [SerializeField] private TMP_Text accuracyPhase0Text;
    [SerializeField] private TMP_Text accuracyPhase1Text;
    [SerializeField] private TMP_Text interpretationText;

    public void SetResults(IQuestionnaireResultsModel results)
    {
        var raceResults = results as RaceQuestionnaireResultsModel;

        if (raceResults == null)
        {
            Debug.LogWarning("Results is not RaceQuestionnaireResultsModel");
            return;
        }

        // D-score
        if (!double.IsNaN(raceResults.DScore))
        {
            dScoreText.text = $"{LocalizationService.GetValue("race_result_dscore")}: {raceResults.DScore:F2}";
        }
        else
        {
            dScoreText.text = LocalizationService.GetValue("race_result_dscore_not_enought");
        }

        // Average latency
        var latencyValue = "—";
        if (!double.IsNaN(raceResults.AvgLatencyDiffMs))
        {
            latencyValue = $"{raceResults.AvgLatencyDiffMs:F1}";
        }
        
        latencyDiffText.text = $"{LocalizationService.GetValue("race_result_average_latency")}: {latencyValue} {LocalizationService.GetValue("milliseconds_short")}";

        // Accuracy of phase 1
        accuracyPhase0Text.text = $"{LocalizationService.GetValue("race_result_accuracy_phase_0")}: {raceResults.AccuracyPhase0 * 100:F1}%";

        // Accuracy of phase 2
        accuracyPhase1Text.text = $"{LocalizationService.GetValue("race_result_accuracy_phase_1")}: {raceResults.AccuracyPhase1 * 100:F1}%";

        // Interpretation
        interpretationText.text = LocalizationService.GetValue(raceResults.Interpretation);
    }
}