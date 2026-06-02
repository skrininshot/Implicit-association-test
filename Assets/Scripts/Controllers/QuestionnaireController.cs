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
    private readonly QuestionnaireView _questionnaireView;
    private readonly TimerModel _timer;
    private readonly ICorrectAnswerChecker _correctAnswerChecker;
    private readonly IQuestionnaireResultsHandler _resultsHandler;
    
    private readonly QuestionnaireModel _model;
    private readonly PhasesQuestionsAnswersModel _phasesQuestionsAnswersModel;
    
    private int _currentPhase;
    private int _currentQuestion;
    
    private QuestionModel DisplayingQuestion => _model.Phases[_currentPhase].Questions[_currentQuestion];

    private QuestionsAnswersModel _currentPhaseAnswers;
    
    private State _state;

    public QuestionnaireController(QuestionnaireWelcomeView welcomeView, RaceQuestionnaireResultsView resultsView, 
        QuestionnaireView questionnaireView, QuestionnaireModel model, PhasesQuestionsAnswersModel phasesQuestionsAnswersModel,
        ICorrectAnswerChecker correctAnswerChecker, IQuestionnaireResultsHandler resultsHandler)
    {
        _welcomeView = welcomeView;
        _resultsView = resultsView;
        _questionnaireView = questionnaireView;
        _model = model;
        _phasesQuestionsAnswersModel = phasesQuestionsAnswersModel;
        _correctAnswerChecker = correctAnswerChecker;
        _resultsHandler = resultsHandler;
    }
    
    public void Initialize()
    {
        _questionnaireView.Initialize(_model.AnswerOptions);

        foreach (var answerOptionButton in _questionnaireView.AnswerOptionButtons)
        {
            answerOptionButton.Value.Button.onClick.AddListener(() => OnOptionButtonOptionClicked(answerOptionButton.Key));
        }
        
        _welcomeView.ButtonAccept.onClick.AddListener(OnWelcomeButtonClicked);
        _resultsView.ButtonAccept.onClick.AddListener(OnResultsButtonClicked);

        _currentPhaseAnswers = new();
        
        SwitchState(State.Welcome);
    }

    public void Dispose()
    {
        foreach (var answerOptionButton in _questionnaireView.AnswerOptionButtons)
        {
            answerOptionButton.Value.Button.onClick.RemoveListener(() => OnOptionButtonOptionClicked(answerOptionButton.Key));
        }
        
        _welcomeView.ButtonAccept.onClick.RemoveListener(OnWelcomeButtonClicked);
        _resultsView.ButtonAccept.onClick.RemoveListener(OnResultsButtonClicked);
    }


    void OnWelcomeButtonClicked()
    {
        SwitchState(State.Questionnaire);
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
        _resultsView.SetActive(_state == State.Results);
        
        switch (_state)
        {
            case State.Welcome:
                break;
            
            case State.Questionnaire:
                _phasesQuestionsAnswersModel.Reset();
                _currentQuestion = 0;
                _currentPhase = 0;
                UpdateQuestionView();
                break;
            
            case State.Results:
                var results = _resultsHandler.GetResults(_phasesQuestionsAnswersModel);
                _resultsView.SetResults(results);
                break;
        }
    }
    
    void OnOptionButtonOptionClicked(AnswerOptionModel answerOption)
    {
        if (_correctAnswerChecker.IsCorrectAnswer(_currentPhase, DisplayingQuestion, answerOption))
        {
            if (_currentQuestion < _model.Phases[_currentPhase].Questions.Length - 1)
            {
                _currentPhaseAnswers.Add(new QuestionAnswerModel(DisplayingQuestion, answerOption, _timer.Value));
                _currentQuestion++;
                
                UpdateQuestionView();
            }
            else if (_currentPhase < _model.Phases.Length - 1)
            {
                _phasesQuestionsAnswersModel.Add(_currentPhaseAnswers);
                _currentPhase++;
                _currentQuestion = 0;

                UpdateQuestionView();
            }
            else
            {
                SwitchState(State.Results);
            }
            
            _timer.Reset();
        }
        else
        {
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
        _questionnaireView.SetQuestionView(DisplayingQuestion);
    }

    private enum State
    {
        Welcome,
        Questionnaire,
        Results,
    }
}
