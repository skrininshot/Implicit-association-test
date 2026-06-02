using System;
using Controllers;
using Models;
using Services;
using Views;
using Zenject;

public class QuestionnaireController : IQuestionnaireController, IInitializable, IDisposable
{
    private readonly QuestionnaireWelcomeView _welcomeView;
    private readonly QuestionnaireResultsView _resultsView;
    private readonly QuestionnaireView _questionnaireView;
    private readonly TimerModel _timer;
    private readonly ICorrectAnswerChecker _correctAnswerChecker;
    
    private readonly QuestionnaireModel _model;
    private readonly IAnswersCollectController _answersCollectController;
    
    private int _currentPhase;
    private int _currentQuestion;
    
    private QuestionModel DisplayingQuestion => _model.Phases[_currentPhase].Questions[_currentQuestion];

    private State _state;

    public QuestionnaireController(QuestionnaireWelcomeView welcomeView, QuestionnaireResultsView resultsView, 
        QuestionnaireView questionnaireView, QuestionnaireModel model, IAnswersCollectController answersCollectController,
        ICorrectAnswerChecker correctAnswerChecker)
    {
        _welcomeView = welcomeView;
        _resultsView = resultsView;
        _questionnaireView = questionnaireView;
        _model = model;
        _answersCollectController = answersCollectController;
        _correctAnswerChecker = correctAnswerChecker;
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
                _answersCollectController.Reset();
                _currentQuestion = 0;
                _currentPhase = 0;
                UpdateQuestionView();
                break;
            
            case State.Results:
                break;
        }
    }
    
    void OnOptionButtonOptionClicked(AnswerOptionModel answerOption)
    {
        if (_currentQuestion < _model.Phases[_currentPhase].Questions.Length - 1)
        {
            if (_correctAnswerChecker.IsCorrectAnswer(_currentPhase, DisplayingQuestion, answerOption))
            {
                _currentQuestion++;
                UpdateQuestionView();
            }
            else
            {
                SetMistakeView(true);
            }
        }
        else if (_currentPhase < _model.Phases.Length - 1)
        {
            _currentQuestion++;
            UpdateQuestionView();
        }
        else
        {
            SwitchState(State.Results);
        }
        
        _timer.Reset();
        
        _answersCollectController.RegisterAnswer(DisplayingQuestion, answerOption, _timer.Value);
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
