using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Services
{
    public static class LocalizationService
    {
        private static readonly string[] availableLanguages = { "ru", "en" };

        private static Dictionary<string, string> _localizedText;
        public static event Action OnLanguageChanged;
        public static string CurrentLanguage { get; private set; }

        public static string[] GetAvailableLanguages() => availableLanguages;

        static LocalizationService()
        {
            string savedLang = PlayerPrefs.GetString("Language", "");
            if (!string.IsNullOrEmpty(savedLang) && Array.Exists(availableLanguages, l => l == savedLang))
                LoadLocalizedText(savedLang);
            else if (availableLanguages.Length > 0)
                LoadLocalizedText(availableLanguages[0]);
        }

        public static void LoadLocalizedText(string langCode)
        {
            if (!Array.Exists(availableLanguages, l => l == langCode))
            {
                Debug.LogError($"Language {langCode} is not available!");
                return;
            }

            _localizedText = new Dictionary<string, string>();
            TextAsset textAsset = Resources.Load<TextAsset>(langCode);

            if (textAsset != null)
            {
                LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(textAsset.text);
                foreach (LocalizationItem item in loadedData.items)
                {
                    _localizedText[item.key] = item.value;
                }
                CurrentLanguage = langCode;
                PlayerPrefs.SetString("Language", langCode);
                OnLanguageChanged?.Invoke();
            }
            else
            {
                Debug.LogError($"Localization file not found: {langCode}");
            }
        }

        public static void SwitchToNextLanguage()
        {
            if (availableLanguages.Length == 0) return;

            int currentIndex = Array.IndexOf(availableLanguages, CurrentLanguage);
            if (currentIndex < 0) currentIndex = 0;
            int nextIndex = (currentIndex + 1) % availableLanguages.Length;
            LoadLocalizedText(availableLanguages[nextIndex]);
        }

        public static string GetValue(string key)
        {
            if (_localizedText != null && _localizedText.ContainsKey(key))
                return _localizedText[key];
            return $"[{key}]";
        }
    }
}