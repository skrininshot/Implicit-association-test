namespace Models
{
    [System.Serializable]
    public class CharacteristicModel
    {
        public readonly string Name;
        public readonly string Type;
        public readonly string Data;
        public readonly CharacteristicType DataType;

        public CharacteristicModel(string name, string type, string data, CharacteristicType dataType)
        {
            Name = name;
            Type = type;
            Data = data;
            DataType = dataType;
        }
    }
}