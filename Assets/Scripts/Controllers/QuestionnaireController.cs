using System;
using Factories;
using Models;
using Services;
using Views;
using Zenject;

namespace Controllers
{
    public class QuestionnaireController : IQuestionnaireController, IInitializable, IDisposable
    {
        private readonly SelectIATView _selectIatView;
        private readonly QuestionnaireWelcomeView _welcomeView;
        private readonly PhaseTipView _phaseTipView;
        private readonly QuestionnaireView _questionnaireView;
        private readonly QuestionnaireResultsView _resultsView;
    
        private readonly TimerModel _timer;
        private readonly PhasesQuestionsAnswersModel _phasesQuestionsAnswersModel;
    
        private readonly QuestionnaireConfig _questionnaireConfig;
        private readonly IatFactory _factory;
        
        private QuestionnaireModel _model;
        private int _currentPhase;
        private int _currentQuestion;
        private bool _isCorrect = true;
        private IatConfig _currentConfig;
    
        private QuestionModel DisplayingQuestion => _model.Phases[_currentPhase].Questions[_currentQuestion];

        private ICorrectAnswerChecker _currentChecker;
        private IQuestionnaireResultsHandler _currentResultsHandler;
        
        private PhaseAnswersModel _currentPhaseAnswers;
    
        private State _state;
    
        public QuestionnaireController(SelectIATView selectIatView, QuestionnaireWelcomeView welcomeView, PhaseTipView phaseTipView, QuestionnaireView questionnaireView,  QuestionnaireResultsView resultsView,
            TimerModel timer, PhasesQuestionsAnswersModel phasesQuestionsAnswersModel, IatFactory factory, QuestionnaireConfig questionnaireConfig)
        {
            _selectIatView = selectIatView;
            _welcomeView = welcomeView;
            _phaseTipView = phaseTipView;
            _questionnaireView = questionnaireView;
            _resultsView = resultsView;
            
            _timer = timer;
            _phasesQuestionsAnswersModel = phasesQuestionsAnswersModel;
            _factory = factory;
            _questionnaireConfig = questionnaireConfig;
        }
    
        public void Initialize()
        {
            foreach (var iatConfig in _questionnaireConfig.IatConfigs)
            {
                var button = _selectIatView.AddPreviewButton(iatConfig.ButtonPreview);
                button.onClick.AddListener(() => OnIatSelectedButtonClicked(iatConfig));
            }
            
            _welcomeView.ButtonAccept.onClick.AddListener(OnWelcomeStartButtonClicked);
            _welcomeView.ButtonBack.onClick.AddListener(OnWelcomeBackButtonClicked);
            _phaseTipView.AcceptButton.onClick.AddListener(PhaseTipPopupButtonClicked);
        
            _resultsView.ButtonAccept.onClick.AddListener(OnResultsButtonClicked);
            
            SwitchState(State.SelectIat);
        }

        public void Dispose()
        {
            foreach (var button in _selectIatView.Buttons)
            {
                button.onClick.RemoveAllListeners();
            }

            _welcomeView.ButtonAccept.onClick.RemoveAllListeners();
            
            foreach (var answerOptionButton in _questionnaireView.AnswerOptionButtons)
                answerOptionButton.Value.Button.onClick.RemoveAllListeners();
            
            _phaseTipView.AcceptButton.onClick.RemoveAllListeners();
            _welcomeView.ButtonBack.onClick.RemoveAllListeners();
            _resultsView.ButtonAccept.onClick.RemoveAllListeners();
        }
        
        void OnIatSelectedButtonClicked(IatConfig iatConfig)
        {
            _currentConfig = iatConfig;
            SwitchState(State.Welcome);
        }
        
        void OnWelcomeBackButtonClicked()
        {
            SwitchState(State.SelectIat);
        }
        
        void OnWelcomeStartButtonClicked()
        {
            SwitchState(State.PhaseTip);
        } 
    
        void OnResultsButtonClicked()
        {
            SwitchState(State.SelectIat);
        }

        void SwitchState(State newState)
        {
            _state = newState;
        
            _selectIatView.SetActive(_state == State.SelectIat);
            _welcomeView.SetActive(_state == State.Welcome);
            _questionnaireView.SetActive(_state == State.Questionnaire);
            _phaseTipView.SetActive(_state == State.PhaseTip);
            _resultsView.SetActive(_state == State.Results);

            if (_state == State.Questionnaire)
            {
                foreach (var answerOptionButton in _questionnaireView.AnswerOptionButtons)
                {
                    var answerOptionKey = answerOptionButton.Key;
                    answerOptionButton.Value.Button.onClick.AddListener(() => OnOptionButtonClicked(answerOptionKey));
                }
            }
            else
            {
                foreach (var answerOptionButton in _questionnaireView.AnswerOptionButtons)
                    answerOptionButton.Value.Button.onClick.RemoveAllListeners();
            }
            
            switch (_state)
            {
                case State.SelectIat:
                    break;
                
                case State.Welcome:
                    var generator = _factory.CreateGenerator(_currentConfig);
                    _currentChecker = _factory.CreateChecker(_currentConfig);
                    _currentResultsHandler = _factory.CreateResultsHandler(_currentConfig);
                    
                    _model = generator.Generate();
                    
                    _questionnaireView.Initialize(_model.AnswerOptions);
                    _currentPhaseAnswers = new(_model.Phases[_currentPhase].Phase);

                    _phasesQuestionsAnswersModel.Reset();
                    _currentPhase = 0;
                    _currentQuestion = 0;
                    _currentPhaseAnswers = new(_model.Phases[_currentPhase].Phase);
                    break;
            
                case State.Questionnaire:
                    _isCorrect = true;
                    _timer.Reset();
                    UpdateQuestionView();
                    break;
            
                case State.PhaseTip:
                    _phaseTipView.SetTip(_model.Phases[_currentPhase].Phase.TipLocalizationKey);
                    break;
            
                case State.Results:
                    var results = _currentResultsHandler.GetResults(_phasesQuestionsAnswersModel);
                    _resultsView.SetResults(_currentConfig, results);
                    break;
            }
        }

        void PhaseTipPopupButtonClicked()
        {
            SwitchState(State.Questionnaire);
        }
    
        void OnOptionButtonClicked(AnswerOptionModel answerOption)
        {
            if (_currentChecker.IsCorrectAnswer(_model.Phases[_currentPhase].Phase.Name, DisplayingQuestion, answerOption))
            {
                int phaseQuestionsCount = _model.Phases[_currentPhase].Questions.Length - 1;
            
                _currentPhaseAnswers.Add(new QuestionAnswerModel(DisplayingQuestion, answerOption, _isCorrect, _timer.Value));
            
                if (_currentQuestion >= phaseQuestionsCount)
                {
                    _phasesQuestionsAnswersModel.Add(_currentPhaseAnswers);

                    if (_currentPhase < _model.Phases.Length - 1)
                    {
                        _currentPhase++;
                        _currentPhaseAnswers = new(_model.Phases[_currentPhase].Phase);
                        _currentQuestion = 0;

                        SwitchState(State.PhaseTip);
                    }
                    else
                    {
                        SwitchState(State.Results);
                    }
                }
                else
                {
                    _currentQuestion++;
                    UpdateQuestionView();
                }
            
                _isCorrect = true;
            }
            else
            {
                _isCorrect = false;
                SetMistakeView(true);
            }
        }

        void SetMistakeView(bool active)
        {
            _questionnaireView.SetMistakeView(active);
        }
    
        void UpdateQuestionView()
        {
            SetMistakeView(false);
            _questionnaireView.UpdatePhaseTip(_model.Phases[_currentPhase].Phase.TipLocalizationKey);
            _questionnaireView.SetQuestionView(DisplayingQuestion);
            _timer.Reset();
        }

        private enum State
        {
            SelectIat,
            Welcome,
            PhaseTip,
            Questionnaire,
            Results,
        }
    }
}
