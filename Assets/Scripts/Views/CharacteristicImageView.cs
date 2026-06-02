using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Views
{
    public class CharacteristicImageView : CharacteristicView
    {
        [SerializeField] private Image image;
        private string _data;
        
        public override void SetData(string data)
        {
            _data = data;
            Addressables.LoadAssetAsync<Sprite>(_data).Completed += OnSpriteLoaded;
        }
        
        private void OnSpriteLoaded(AsyncOperationHandle<Sprite> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Sprite loadedSprite = handle.Result;
                image.sprite = loadedSprite;
            }
            else
            {
                Debug.LogError($"Cannot load image: {_data}.");
            }
        }
    }
}