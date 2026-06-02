using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class AnswerOptionModel
    {
        public readonly string Name;
        public readonly Color ViewColor;
        public readonly string Content;
        
        public AnswerOptionModel(string name, Color viewColor, string content)
        {
            Name = name;
            ViewColor = viewColor;
            Content = content;
        }
    }
}