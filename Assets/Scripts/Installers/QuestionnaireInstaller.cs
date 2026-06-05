using Controllers;
using Factories;
using Models;
using Services;
using UnityEngine;
using Views;
using Zenject;

namespace Installers
{
    public class QuestionnaireInstaller : MonoInstaller
    {
        [SerializeField] private QuestionnaireConfig questionnaireConfig;
        
        [SerializeField] private SelectIATView selectIATView;
        [SerializeField] private QuestionnaireWelcomeView welcomeView;
        [SerializeField] private PhaseTipView phaseTipView;
        [SerializeField] private QuestionnaireView questionnaireView;
        [SerializeField] private QuestionnaireResultsView resultsView;

        public override void InstallBindings()
        {
            Container.Bind<IatFactory>().AsSingle();
            
            Container.BindInstance(questionnaireConfig).AsSingle();
            
            Container.BindInstance(selectIATView).AsSingle();
            Container.BindInstance(welcomeView).AsSingle();
            Container.BindInstance(phaseTipView).AsSingle();
            Container.BindInstance(questionnaireView).AsSingle();
            Container.BindInstance(resultsView).AsSingle();
            
            Container.BindInterfacesTo<IatCorrectAnswerChecker>().AsSingle();
            Container.BindInterfacesTo<IatResultsHandler>().AsSingle();

            var timerModel = new TimerModel();
            Container.BindInterfacesTo<TimerController>().AsSingle().WithArguments(timerModel);
        
            var phasesQuestionsAnswersModel = new PhasesQuestionsAnswersModel();
        
            Container.BindInterfacesTo<QuestionnaireController>().AsSingle().WithArguments(timerModel, phasesQuestionsAnswersModel);
        }
    }
}
