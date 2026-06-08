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
        private const string DefaultGroupLabel = "default";

        public IEnumerator PreloadCoroutine(Action<float> onProgress, Action onComplete)
        {
            var downloadHandle = Addressables.DownloadDependenciesAsync(DefaultGroupLabel);
            while (!downloadHandle.IsDone)
            {
                onProgress?.Invoke(downloadHandle.PercentComplete);
                yield return null;
            }

            if (downloadHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Failed to download Addressables dependencies");
                onComplete?.Invoke();
                yield break;
            }

            Addressables.Release(downloadHandle);
            
            var locationsHandle = Addressables.LoadResourceLocationsAsync(DefaultGroupLabel, typeof(Sprite));
            yield return locationsHandle;

            if (locationsHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Failed to load resource locations");
                onComplete?.Invoke();
                yield break;
            }

            var locations = locationsHandle.Result;
            int total = locations.Count;
            int loaded = 0;
            
            List<AsyncOperationHandle> loadHandles = new List<AsyncOperationHandle>();
            foreach (var location in locations)
            {
                var handle = Addressables.LoadAssetAsync<Sprite>(location);
                loadHandles.Add(handle);
                handle.Completed += _ =>
                {
                    loaded++;
                    onProgress?.Invoke((float)loaded / total);
                };
            }
            
            foreach (var handle in loadHandles)
                yield return handle;
            
            foreach (var handle in loadHandles)
                Addressables.Release(handle);

            Addressables.Release(locationsHandle);

            onProgress?.Invoke(1f);
            onComplete?.Invoke();
        }
    }
}