using System;
using Controllers;
using Models;
using Services;
using Views;
using Zenject;

public class QuestionnaireController : IQuestionnaireController, IInitializable, IDisposable
{
    private readonly QuestionnaireWelcomeView _welcomeView;
    private readonly RaceQuestionnaireResultsView _resultsView;
    private readonly PhaseTipView _phaseTipView;
    private readonly QuestionnaireView _questionnaireView;
    
    private readonly TimerModel _timer;
    private readonly ICorrectAnswerChecker _correctAnswerChecker;
    private readonly IQuestionnaireResultsHandler _resultsHandler;
    
    private readonly QuestionnaireModel _model;
    private readonly PhasesQuestionsAnswersModel _phasesQuestionsAnswersModel;
    
    private int _currentPhase;
    private int _currentQuestion;
    private bool _isCorrect = true;
    
    private QuestionModel DisplayingQuestion => _model.Phases[_currentPhase].Questions[_currentQuestion];

    private PhaseAnswersModel _currentPhaseAnswers;
    
    private State _state;
    
    public QuestionnaireController(QuestionnaireWelcomeView welcomeView, RaceQuestionnaireResultsView resultsView, PhaseTipView phaseTipView, QuestionnaireView questionnaireView, 
        TimerModel timer, PhasesQuestionsAnswersModel phasesQuestionsAnswersModel, QuestionnaireModel model, 
        ICorrectAnswerChecker correctAnswerChecker, IQuestionnaireResultsHandler resultsHandler)
    {
        _welcomeView = welcomeView;
        _resultsView = resultsView;
        _phaseTipView = phaseTipView;
        _questionnaireView = questionnaireView;
        _timer = timer;
        _model = model;
        _phasesQuestionsAnswersModel = phasesQuestionsAnswersModel;
        _correctAnswerChecker = correctAnswerChecker;
        _resultsHandler = resultsHandler;
    }
    
    public void Initialize()
    {
        _questionnaireView.Initialize(_model.AnswerOptions);
        _phaseTipView.AcceptButton.onClick.AddListener(PhaseTipPopupButtonClicked);

        foreach (var answerOptionButton in _questionnaireView.AnswerOptionButtons)
        {
            var answerOptionKey = answerOptionButton.Key;
            answerOptionButton.Value.Button.onClick.AddListener(() => OnOptionButtonOptionClicked(answerOptionKey));
        }
        
        _welcomeView.ButtonAccept.onClick.AddListener(OnWelcomeButtonClicked);
        _resultsView.ButtonAccept.onClick.AddListener(OnResultsButtonClicked);

        _currentPhaseAnswers = new();
        
        SwitchState(State.Welcome);
    }

    public void Dispose()
    {
        _phaseTipView.AcceptButton.onClick.RemoveListener(PhaseTipPopupButtonClicked);
        
        foreach (var answerOptionButton in _questionnaireView.AnswerOptionButtons)
        {
            var answerOptionKey = answerOptionButton.Key;
            answerOptionButton.Value.Button.onClick.RemoveListener(() => OnOptionButtonOptionClicked(answerOptionKey));
        }
        
        _welcomeView.ButtonAccept.onClick.RemoveListener(OnWelcomeButtonClicked);
        _resultsView.ButtonAccept.onClick.RemoveListener(OnResultsButtonClicked);
    }


    void OnWelcomeButtonClicked()
    {
        SwitchState(State.PhaseTip);
    }
    
    void OnResultsButtonClicked()
    {
        SwitchState(State.Welcome);
    }

    void SwitchState(State newState)
    {
        _state = newState;
        
        _welcomeView.SetActive(_state == State.Welcome);
        _questionnaireView.SetActive(_state == State.Questionnaire);
        _phaseTipView.SetActive(_state == State.PhaseTip);
        _resultsView.SetActive(_state == State.Results);
        
        switch (_state)
        {
            case State.Welcome:
                _phasesQuestionsAnswersModel.Reset();
                _currentPhase = 0;
                _currentQuestion = 0;
                _currentPhaseAnswers = new();
               break;
            
            case State.Questionnaire:
                _isCorrect = true;
                _timer.Reset();
                UpdateQuestionView();
                break;
            
            case State.PhaseTip:
                _phaseTipView.SetTip(_model.Phases[_currentPhase].TipLocalizationKey);
                break;
            
            case State.Results:
                var results = _resultsHandler.GetResults(_phasesQuestionsAnswersModel);
                _resultsView.SetResults(results);
                break;
        }
    }

    void PhaseTipPopupButtonClicked()
    {
        SwitchState(State.Questionnaire);
    }
    
    void OnOptionButtonOptionClicked(AnswerOptionModel answerOption)
    {
        if (_correctAnswerChecker.IsCorrectAnswer(_currentPhase, DisplayingQuestion, answerOption))
        {
            int phaseQuestionsCount = _model.Phases[_currentPhase].Questions.Length - 1;
            
            _currentPhaseAnswers.Add(new QuestionAnswerModel(DisplayingQuestion, answerOption, _isCorrect, _timer.Value));
            
            if (_currentQuestion >= phaseQuestionsCount)
            {
                _phasesQuestionsAnswersModel.Add(_currentPhaseAnswers);

                if (_currentPhase < _model.Phases.Length - 1)
                {
                    _currentPhaseAnswers = new();
                    _currentQuestion = 0;
                    _currentPhase++;

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
            _timer.Reset();
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
        _questionnaireView.SetPhaseTip(_model.Phases[_currentPhase].TipLocalizationKey);
        _questionnaireView.SetQuestionView(DisplayingQuestion);
    }

    private enum State
    {
        Welcome,
        PhaseTip,
        Questionnaire,
        Results,
    }
}
