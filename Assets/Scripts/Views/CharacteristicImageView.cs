using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class CharacteristicImageView : CharacteristicView
    {
        [SerializeField] private Image image;
        
        public override void SetData(string data)
        {
            // image addressable loading
        }
    }
}