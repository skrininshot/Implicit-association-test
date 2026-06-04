using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class AnswerOptionModel
    {
        public readonly string Name;
        public readonly Color ViewColor;
        public readonly string Content;
        public readonly KeyCode KeyCode;
        
        public AnswerOptionModel(string name, Color viewColor, string content, KeyCode keyCodeCode)
        {
            Name = name;
            ViewColor = viewColor;
            Content = content;
            KeyCode = keyCodeCode;
        }
    }
}