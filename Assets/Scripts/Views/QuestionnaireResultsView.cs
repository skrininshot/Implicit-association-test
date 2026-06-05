using Models;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Views
{
    public class QuestionnaireResultsView : View
    {
        [field: SerializeField] public Button ButtonAccept { get; private set; }

        [SerializeField] private TMP_Text dScoreText;
        [SerializeField] private TMP_Text latencyDiffText;
        [SerializeField] private TMP_Text accuracyPhase0Text;
        [SerializeField] private TMP_Text accuracyPhase1Text;
        [SerializeField] private TMP_Text interpretationText;

        private IatResultsModel _results;
        private IatConfig _config;

        private void OnEnable()
        {
            LocalizationService.OnLanguageChanged += UpdateAllTexts;
        }

        private void OnDisable()
        {
            LocalizationService.OnLanguageChanged -= UpdateAllTexts;
        }

        public void SetResults( IatConfig config, IQuestionnaireResultsModel results)
        {
            _config = config;
            _results = results as IatResultsModel;
            UpdateAllTexts();
        }

        private void UpdateAllTexts()
        {
            if (_results == null || _config == null || _config.ResultKeys == null) return;

            var keys = _config.ResultKeys;

            // D-score
            if (!double.IsNaN(_results.DScore))
                dScoreText.text = $"{LocalizationService.GetValue(keys.DScoreTitle)}: {_results.DScore:F2}";
            else
                dScoreText.text = LocalizationService.GetValue(keys.DScoreNotEnoughData);
            AppendDescription(dScoreText, keys.DScoreDescription);

            // Average latency
            string latencyValue = double.IsNaN(_results.AvgLatencyDiffMs) ? "—" : $"{_results.AvgLatencyDiffMs:F1}";
            latencyDiffText.text =
                $"{LocalizationService.GetValue(keys.AvgLatencyTitle)}: {latencyValue} {LocalizationService.GetValue(keys.MillisecondsShort)}";
            AppendDescription(latencyDiffText, keys.AvgLatencyDescription);

            // Accuracy phases
            accuracyPhase0Text.text =
                $"{LocalizationService.GetValue(keys.AccuracyPhaseTitle0)}: {_results.AccuracyPhase0 * 100:F1}%";
            AppendDescription(accuracyPhase0Text, keys.AccuracyDescription);

            accuracyPhase1Text.text =
                $"{LocalizationService.GetValue(keys.AccuracyPhaseTitle1)}: {_results.AccuracyPhase1 * 100:F1}%";
            AppendDescription(accuracyPhase1Text, keys.AccuracyDescription);

            // Interpretation
            interpretationText.text = LocalizationService.GetValue(_results.Interpretation);
            AppendDescription(interpretationText, keys.InterpretationDescription);
        }

        private void AppendDescription(TMP_Text textField, string localizationKey)
        {
            string description = LocalizationService.GetValue(localizationKey);
            if (!string.IsNullOrEmpty(description))
                textField.text += $"\n<size=85%><color=#AAAAAA>{description}</color></size>";
        }
    }
}