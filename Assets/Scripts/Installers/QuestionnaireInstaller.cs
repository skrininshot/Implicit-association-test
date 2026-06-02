using Controllers;
using Models;
using Services;
using UnityEngine;
using Views;
using Zenject;

public class QuestionnaireInstaller : MonoInstaller
{
    [SerializeField] private QuestionnaireWelcomeView welcomeView;
    [SerializeField] private QuestionnaireView questionnaireView;
    [SerializeField] private RaceQuestionnaireResultsView resultsView;

    public override void InstallBindings()
    {
        Container.BindInstance(new RaceQuestionnaireGenerator().Generate()).AsSingle();
        
        Container.BindInterfacesTo<RaceCorrectAnswerChecker>();
        Container.BindInterfacesTo<RaceQuestionnaireResultsHandler>();

        var timerModel = new TimerModel();
        Container.BindInterfacesTo<TimerController>().AsSingle().WithArguments(timerModel);
        
        var phasesQuestionsAnswersModel = new PhasesQuestionsAnswersModel();
        
        Container.BindInterfacesTo<QuestionnaireController>().AsSingle().WithArguments(welcomeView, resultsView, questionnaireView, timerModel, phasesQuestionsAnswersModel);
    }
}
