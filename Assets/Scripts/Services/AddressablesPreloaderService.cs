using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Services
{
    public interface IAddressablesPreloader
    {
        IEnumerator PreloadCoroutine(string label, Action<float> onProgress, Action onComplete);
    }

    public class AddressablesPreloaderService : IAddressablesPreloader
    {
        public IEnumerator PreloadCoroutine(string label, Action<float> onProgress, Action onComplete)
        {
            var handle = Addressables.DownloadDependenciesAsync(label);
            while (!handle.IsDone)
            {
                onProgress?.Invoke(handle.PercentComplete);
                yield return null;
            }

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                onProgress?.Invoke(1f);
                onComplete?.Invoke();
            }
            else
            {
                Debug.LogError($"Addressables preload failed for label: {label}");
                onProgress?.Invoke(1f);
                onComplete?.Invoke(); // или вызов ошибки
            }

            Addressables.Release(handle);
        }
    }
}