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
    [SerializeField] private PhaseTipView phaseTipView;
    [SerializeField] private RaceQuestionnaireResultsView resultsView;
    [SerializeField] private QuestionnaireGenerationSettings generationSettings;
    
    public override void InstallBindings()
    {
        Container.BindInstance(new RaceQuestionnaireGenerator().Generate(generationSettings.Get())).AsSingle();
        
        Container.BindInterfacesTo<RaceCorrectAnswerChecker>().AsSingle();
        Container.BindInterfacesTo<RaceQuestionnaireResultsHandler>().AsSingle();

        var timerModel = new TimerModel();
        Container.BindInterfacesTo<TimerController>().AsSingle().WithArguments(timerModel);
        
        var phasesQuestionsAnswersModel = new PhasesQuestionsAnswersModel();
        
        Container.BindInterfacesTo<QuestionnaireController>().AsSingle().WithArguments(welcomeView, resultsView, phaseTipView, questionnaireView, timerModel, phasesQuestionsAnswersModel);
    }
}
