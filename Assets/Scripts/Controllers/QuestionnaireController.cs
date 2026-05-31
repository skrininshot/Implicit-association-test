using System;
using Controllers;
using Models;
using Views;
using Zenject;

public class QuestionnaireController : IQuestionnaireController, IInitializable, IDisposable
{
    private readonly QuestionnaireView _view;
    private QuestionnaireModel _model;
    private QuestionnaireAnswersModel _answersModel;
    private int _displayingQuestion;
    private QuestionModel CurrentQuestion => _model.Questions[_displayingQuestion];

    public QuestionnaireController(QuestionnaireView view, QuestionnaireModel model)
    {
        _view = view;
        _model = model;
    }
    
    public void Initialize()
    {
        _view.Initialize(_model.AnswerOptions);

        foreach (var answerOptionButton in _view.AnswerOptionButtons)
        {
            answerOptionButton.Value.Button.onClick.AddListener(() => OnButtonOptionClicked(answerOptionButton.Key));
        }

        _displayingQuestion = 0;
        UpdateQuestionView();
    }

    public void Dispose()
    {
        foreach (var answerOptionButton in _view.AnswerOptionButtons)
        {
            answerOptionButton.Value.Button.onClick.RemoveListener(() => OnButtonOptionClicked(answerOptionButton.Key));
        }
    }

    void OnButtonOptionClicked(AnswerOptionModel answerOption)
    {
        _answersModel.RegisterAnswer(CurrentQuestion, answerOption);

        _displayingQuestion++;
        UpdateQuestionView();
    }

    void UpdateQuestionView()
    {
        _view.SetQuestionView(CurrentQuestion);
    }
}
