using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class SelectIATView : View
    {
        [SerializeField] private Transform selectIatPreviewsContainer;

        public IReadOnlyList<Button> Buttons => _buttons;
        
        private readonly List<Button> _buttons = new(10);
        
        public Button AddPreviewButton(SelectIATButtonView iatButton)
        {
            var button = Instantiate(iatButton, selectIatPreviewsContainer).Button;
            _buttons.Add(button);
            
            return button;
        }
    }
}
