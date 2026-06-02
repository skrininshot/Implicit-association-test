namespace Models
{
    [System.Serializable]
    public class QuestionModel
    {
        public readonly CharacteristicModel[] Characteristics;

        public QuestionModel(CharacteristicModel[] characteristics)
        {
            Characteristics = characteristics;
        }
    }
}