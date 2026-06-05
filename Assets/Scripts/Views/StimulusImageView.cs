using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Views
{
    public class StimulusImageView : StimulusView
    {
        [SerializeField] private Image image;
        [SerializeField] private Color transparentColor;
        private string _data;
        
        private void OnDisable()
        {
            image.color = transparentColor;
        }

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
                image.color = Color.white;
            }
            else
            {
                Debug.LogError($"Cannot load image: {_data}.");
            }
        }
    }
}