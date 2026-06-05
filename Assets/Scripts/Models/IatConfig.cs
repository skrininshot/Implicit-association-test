using UnityEngine;
using Views;

namespace Models
{
    [CreateAssetMenu(fileName = "IAT Config", menuName = "IAT/New Config")]
    public class IatConfig : ScriptableObject
    {
        public string TestId;
        public StimulusCategory[] Categories;
        public PhaseConfig[] Phases;
        public int QuestionsPerPhase = 40;
        public int MaxConsecutiveSameType = 3;
        public string InterpretationKeyPositive;
        public string InterpretationKeyNegative;
        public string InterpretationKeyNeutral;
        public ResultLocalizationKeys ResultKeys;
        public SelectIATButtonView ButtonPreview;
    }

    [System.Serializable]
    public class StimulusCategory
    {
        public string Name;
        public StimulusType Type;
        public string AssetPrefix;
        public int AssetCount;
        public string Polarity;
    }

    public enum StimulusType { Image, Text }

    [System.Serializable]
    public class PhaseConfig
    {
        public string PhaseName;
        public string InstructionLocalizedKey;
        public string[] LeftCategories;
        public string[] RightCategories;
    }
    
    [System.Serializable]
    public class ResultLocalizationKeys
    {
        public string DScoreTitle;
        public string DScoreDescription;
        public string DScoreNotEnoughData;
        public string AvgLatencyTitle;
        public string AvgLatencyDescription;
        public string MillisecondsShort;
        public string AccuracyPhaseTitle0;
        public string AccuracyPhaseTitle1;
        public string AccuracyDescription;
        public string InterpretationDescription;
    }
}