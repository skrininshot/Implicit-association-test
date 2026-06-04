namespace Models
{
    [System.Serializable]
    public class CharacteristicModel
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Data { get; private set; }
        public CharacteristicType DataType { get; private set; }
        
        public void Set(string name, string type, string data, CharacteristicType dataType)
        {
            Name = name;
            Type = type;
            Data = data;
            DataType = dataType;
        }
    }
}