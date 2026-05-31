namespace Models
{
    [System.Serializable]
    public class CharacteristicModel
    {
        public string Data { get; private set; }
        public CharacteristicType Type { get; private set; }

        public CharacteristicModel(string data, CharacteristicType type)
        {
            Data = data;
            Type = type;
        }
    }
}