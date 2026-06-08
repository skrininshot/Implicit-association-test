using System.Collections;
using Services;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class BootstrapLoader : MonoBehaviour
    {
        [Inject] private IAddressablesPreloader _preloader;
        [Inject] private ISceneLoader _sceneLoader;

        private void Start()
        {
            StartCoroutine(LoadRoutine());
        }

        private IEnumerator LoadRoutine()
        {
            yield return _preloader.PreloadCoroutine(OnProgress, OnComplete);
        }

        private void OnProgress(float progress)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalCall("SetAddressablesProgress", progress);
#endif
        }

        private void OnComplete()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalCall("AddressablesLoaded");
#endif
            StartCoroutine(_sceneLoader.LoadMainScene());
        }
    }
}