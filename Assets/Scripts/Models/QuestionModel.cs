namespace Models
{
    [System.Serializable]
    public class QuestionModel
    {
        public CharacteristicModel[] Characteristics { get; private set; }

        public QuestionModel(CharacteristicModel[] characteristics)
        {
            Characteristics = characteristics;
        }
    }
}