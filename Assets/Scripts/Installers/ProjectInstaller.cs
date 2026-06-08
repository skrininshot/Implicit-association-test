using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private string mainSceneName = "MainScene";
        [SerializeField] private string langCode = "en";
    
        public override void InstallBindings()
        {
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<IAddressablesPreloader>().To<AddressablesPreloaderService>().AsSingle();
            LocalizationService.LoadLocalizedText(langCode);
        }
    }
}
