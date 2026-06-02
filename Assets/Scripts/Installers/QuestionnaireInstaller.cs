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
    [SerializeField] private QuestionnaireResultsView resultsView;

    public override void InstallBindings()
    {
        ICorrectAnswerChecker correctAnswerChecker = new RaceCorrectAnswerChecker();
        
        IQuestionnaireGenerator questionnaireGenerator = new RaceQuestionnaireGenerator();
        var questionnaireModel = questionnaireGenerator.Generate();
        Container.BindInstance(questionnaireModel).AsSingle();

        var timerModel = new TimerModel();
        Container.BindInterfacesTo<TimerController>().AsSingle().WithArguments(timerModel);
        
        Container.BindInterfacesTo<AnswersCollectController>().AsSingle().WithArguments(new QuestionsAnswersDataSetModel());
        Container.BindInterfacesTo<ResultsHandlerController>().AsSingle().WithArguments(new QuestionsAnswersDataSetModel());
        Container.BindInterfacesTo<QuestionnaireController>().AsSingle().WithArguments(welcomeView, resultsView, questionnaireView, timerModel, correctAnswerChecker);
    }
}
