using Models;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public static class LocalizationService
    {
        private static Dictionary<string, string> _localizedText;
        
        public static void LoadLocalizedText(string langCode)
        {
            _localizedText = new Dictionary<string, string>();
            
            TextAsset textAsset = Resources.Load<TextAsset>(langCode);
        
            if (textAsset != null)
            {
                LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(textAsset.text);
            
                foreach (LocalizationItem item in loadedData.items)
                {
                    _localizedText[item.key] = item.value;
                }
            }
            else
            {
                Debug.LogError($"Localization file not found for language: {langCode}");
            }
        }

        public static string GetValue(string key)
        {
            if (_localizedText != null && _localizedText.ContainsKey(key))
            {
                return _localizedText[key];
            }
            return $"[{key}]";
        }
    }
}