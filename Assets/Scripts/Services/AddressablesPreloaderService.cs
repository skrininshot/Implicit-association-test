using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Services
{
    public interface IAddressablesPreloader
    {
        IEnumerator PreloadCoroutine(Action<float> onProgress, Action onComplete);
    }

    public class AddressablesPreloaderService : IAddressablesPreloader
    {
        private const string PreloadLabel = "default";

        private List<Sprite> _preloadedSprites = new();

        public IEnumerator PreloadCoroutine(Action<float> onProgress, Action onComplete)
        {
            var downloadHandle = Addressables.DownloadDependenciesAsync(PreloadLabel);
            while (!downloadHandle.IsDone)
            {
                onProgress?.Invoke(downloadHandle.PercentComplete);
                yield return null;
            }

            if (downloadHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to download dependencies for label '{PreloadLabel}'");
                onComplete?.Invoke();
                yield break;
            }

            Addressables.Release(downloadHandle);
            
            var locationsHandle = Addressables.LoadResourceLocationsAsync(PreloadLabel, typeof(Sprite));
            yield return locationsHandle;

            if (locationsHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to get locations for label '{PreloadLabel}'");
                onComplete?.Invoke();
                yield break;
            }

            var locations = locationsHandle.Result;
            if (locations.Count == 0)
            {
                Debug.LogWarning($"No Sprite locations with label '{PreloadLabel}'");
                onComplete?.Invoke();
                yield break;
            }

            int total = locations.Count;
            int loaded = 0;
            var handles = new List<AsyncOperationHandle<Sprite>>();

            foreach (var location in locations)
            {
                var handle = Addressables.LoadAssetAsync<Sprite>(location);
                handles.Add(handle);
                handle.Completed += _ =>
                {
                    loaded++;
                    float progress = (float)loaded / total;
                    onProgress?.Invoke(progress);
                };
            }
            
            foreach (var handle in handles)
                yield return handle;
            
            foreach (var handle in handles)
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                    _preloadedSprites.Add(handle.Result);
            }
            
            Addressables.Release(locationsHandle);
            
            onProgress?.Invoke(1f);
            onComplete?.Invoke();
        }
    }
}