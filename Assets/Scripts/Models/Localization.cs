using System;

namespace Models
{
    [Serializable]
    public class LocalizationItem
    {
        public string key;
        public string value;
    }

    [Serializable]
    public class LocalizationData
    {
        public LocalizationItem[] items;
    }
}