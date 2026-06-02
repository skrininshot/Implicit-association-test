namespace Models
{
    public class RaceQuestionnaireResultsModel : IQuestionnaireResultsModel
    {
        public readonly double DScore;
        public readonly double AvgLatencyDiffMs;
        public readonly double AccuracyPhase0;
        public readonly double AccuracyPhase1;
        public readonly string Interpretation;

        public RaceQuestionnaireResultsModel(double dScore, double avgLatencyDiff, double accuracyPhase0, double accuracyPhase1, string interpretation)
        {
            DScore = dScore;
            AvgLatencyDiffMs = avgLatencyDiff;
            AccuracyPhase0 = accuracyPhase0;
            AccuracyPhase1 = accuracyPhase1;
            Interpretation = interpretation;
        }
    }
}