using Services;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class SwitchLocalizationButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        private void OnEnable()
        {
            button.onClick.AddListener(SwitchToNextLanguage);
        }

        private void OnDisable()
        {
            button.onClick.RemoveAllListeners();
        }

        public static void SwitchToNextLanguage()
        {
            LocalizationService.SwitchToNextLanguage();
        }
    }
}