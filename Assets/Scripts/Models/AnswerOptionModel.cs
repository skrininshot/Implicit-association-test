using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class AnswerOptionModel
    {
        public string Name { get; private set; }
        public Color ViewColor { get; private set; }
        
        public AnswerOptionModel(string name, Color viewColor)
        {
            Name = name;
            ViewColor = viewColor;
        }
    }
}