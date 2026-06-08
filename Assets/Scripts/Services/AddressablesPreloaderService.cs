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
        private const string GroupName = "Default Local Group";

        public IEnumerator PreloadCoroutine(Action<float> onProgress, Action onComplete)
        {
            var downloadHandle = Addressables.DownloadDependenciesAsync(GroupName);
            while (!downloadHandle.IsDone)
            {
                onProgress?.Invoke(downloadHandle.PercentComplete);
                yield return null;
            }

            if (downloadHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to download dependencies for group: {GroupName}");
                onComplete?.Invoke();
                yield break;
            }

            Addressables.Release(downloadHandle);
            
            var locationsHandle = Addressables.LoadResourceLocationsAsync(GroupName, typeof(Sprite));
            yield return locationsHandle;
            if (locationsHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load resource locations for group: {GroupName}");
                onComplete?.Invoke();
                yield break;
            }

            var locations = locationsHandle.Result;
            int total = locations.Count;
            int loaded = 0;
            var handles = new List<AsyncOperationHandle>();

            foreach (var location in locations)
            {
                var handle = Addressables.LoadAssetAsync<Sprite>(location);
                handles.Add(handle);
                handle.Completed += _ =>
                {
                    loaded++;
                    onProgress?.Invoke((float)loaded / total);
                };
            }
            
            foreach (var handle in handles)
                yield return handle;

            foreach (var handle in handles)
                Addressables.Release(handle);
            Addressables.Release(locationsHandle);

            onProgress?.Invoke(1f);
            onComplete?.Invoke();
        }
    }
}