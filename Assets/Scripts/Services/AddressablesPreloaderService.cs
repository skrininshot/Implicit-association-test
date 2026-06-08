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
        public IEnumerator PreloadCoroutine(Action<float> onProgress, Action onComplete)
        {
            var allKeys = new List<object>();
            foreach (var locator in Addressables.ResourceLocators)
            {
                foreach (var key in locator.Keys)
                {
                    allKeys.Add(key);
                }
            }

            if (allKeys.Count == 0)
            {
                Debug.LogWarning("No Addressable keys found. Skipping preload.");
                onComplete?.Invoke();
                yield break;
            }
            
            var downloadHandle = Addressables.DownloadDependenciesAsync(allKeys);
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
            
            int total = allKeys.Count;
            int loaded = 0;
            var handles = new List<AsyncOperationHandle>();

            foreach (var key in allKeys)
            {
                var handle = Addressables.LoadAssetAsync<Sprite>(key);
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

            onProgress?.Invoke(1f);
            onComplete?.Invoke();
        }
    }
}