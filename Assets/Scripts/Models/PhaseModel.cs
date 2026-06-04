namespace Models
{
    [System.Serializable]
    public class PhaseModel
    {
        public readonly string Name;
        public readonly string TipLocalizationKey;
        
        public PhaseModel(string name, string tipLocalizationKey)
        {
            Name = name;
            TipLocalizationKey = tipLocalizationKey;
        }
    }
}