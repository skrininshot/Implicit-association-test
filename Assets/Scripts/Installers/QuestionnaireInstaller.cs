using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Models;
using Services;
using UnityEngine;
using Zenject;

public class QuestionnaireInstaller : MonoInstaller
{
    [SerializeField] private QuestionnaireView questionnaireView;

    public override void InstallBindings()
    {
        Container.BindInstance(QuestionnaireGenerator.Generate()).AsSingle();
        Container.BindInstance(new QuestionnaireAnswersModel()).AsSingle();
        Container.BindInterfacesTo<QuestionnaireController>().AsSingle().WithArguments(questionnaireView);
    }
}
