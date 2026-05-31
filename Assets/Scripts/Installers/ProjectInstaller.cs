using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private string mainSceneName = "MainScene";
    [SerializeField] private string langCode = "en";
    
    public override void InstallBindings()
    {
        LocalizationService.LoadLocalizedText(langCode);
        TransitionToMainScene();
    }

    void TransitionToMainScene()
    {
        if (SceneManager.GetActiveScene().name != mainSceneName)
        {
            SceneManager.LoadScene(mainSceneName);
        }
    }
}
