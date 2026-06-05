namespace Models
{
    [System.Serializable]
    public class Stimulus
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Data { get; private set; }
        public StimulusType StimulusType { get; private set; }
        
        public void Set(string name, string type, string data, StimulusType stimulusType)
        {
            Name = name;
            Type = type;
            Data = data;
            StimulusType = stimulusType;
        }
    }
}