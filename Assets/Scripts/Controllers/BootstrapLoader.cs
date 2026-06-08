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
        Application.ExternalEval(
            $"SetAddressablesProgress({progress.ToString("F3", System.Globalization.CultureInfo.InvariantCulture)});"
        );
#endif
        }

        private void OnComplete()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        Application.ExternalEval("AddressablesLoaded();");
#endif
            StartCoroutine(_sceneLoader.LoadMainScene());
        }
    }
}