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
        [SerializeField] private Color transparentColor = Color.clear;

        private string _currentData;
        private AsyncOperationHandle<Sprite>? _currentHandle;

        private void OnDisable()
        {
            image.sprite = null;
            image.color = transparentColor;
            
            if (_currentHandle.HasValue && _currentHandle.Value.IsValid())
            {
                _currentHandle.Value.Completed -= OnSpriteLoaded;
                Addressables.Release(_currentHandle.Value);
                _currentHandle = null;
            }
            
            _currentData = null;
        }

        public override void SetData(string data)
        {
            if (_currentData == data) return;
            
            if (_currentHandle.HasValue && _currentHandle.Value.IsValid())
            {
                _currentHandle.Value.Completed -= OnSpriteLoaded;
                Addressables.Release(_currentHandle.Value);
                _currentHandle = null;
            }

            _currentData = data;
            
            if (!isActiveAndEnabled)
            {
                image.sprite = null;
                image.color = transparentColor;
                return;
            }
            
            image.sprite = null;
            image.color = transparentColor;

            var handle = Addressables.LoadAssetAsync<Sprite>(data);
            _currentHandle = handle;
            handle.Completed += OnSpriteLoaded;
        }

        private void OnSpriteLoaded(AsyncOperationHandle<Sprite> handle)
        {
            if (!_currentHandle.HasValue || !_currentHandle.Value.Equals(handle))
            {
                if (handle.IsValid())
                    Addressables.Release(handle);
                return;
            }
            
            handle.Completed -= OnSpriteLoaded;
            _currentHandle = null;
            
            if (!isActiveAndEnabled || _currentData == null)
            {
                if (handle.IsValid())
                    Addressables.Release(handle);
                return;
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                image.sprite = handle.Result;
                image.color = Color.white;
            }
            else
            {
                Debug.LogError($"Cannot load image: {_currentData}. Exception: {handle.OperationException}");
            }
        }
    }
}