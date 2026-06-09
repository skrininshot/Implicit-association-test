using System.Collections.Generic;
using Models;
using Services;
using TMPro;
using UnityEngine;

namespace Views
{
     public class QuestionnaireView : View
     {
          [SerializeField] private Transform buttonOptionsContainer;
          [SerializeField] private Transform stimulusContainer;
     
          [SerializeField] private ButtonOptionView buttonOptionViewPrefab;
          [SerializeField] private StimulusViewTypePrefab[] stimulusViewTypePrefabs;

          [SerializeField] private GameObject mistakeView;
          [SerializeField] private LocalizedTMP phaseTipText;
     
          private Dictionary<AnswerOptionModel, ButtonOptionView> _answerOptionButtons = new();
          private Dictionary<StimulusType, StimulusView> _stimulusViews;
     
          public IReadOnlyDictionary<AnswerOptionModel, ButtonOptionView> AnswerOptionButtons => _answerOptionButtons;

          private string _phaseLocalizationTextKey;
     
          public void Initialize(AnswerOptionModel[] answerOptions)
          {
               for (int i = 0; i < buttonOptionsContainer.childCount; i++)
               {
                    Destroy(buttonOptionsContainer.GetChild(i).gameObject);
               }
          
               for (int i = 0; i < stimulusContainer.childCount; i++)
               {
                    Destroy(stimulusContainer.GetChild(i).gameObject);
               }

               _stimulusViews = new();

               foreach (var element in stimulusViewTypePrefabs)
               {
                    var stimulusView = Instantiate(element.Prefab, stimulusContainer);
                    _stimulusViews.Add(element.Type, stimulusView);
               }
          
               _answerOptionButtons = new();
          
               foreach (var answerOption in answerOptions)
               {
                    var buttonView = Instantiate(buttonOptionViewPrefab, buttonOptionsContainer);
                    buttonView.Initialize(answerOption);
                    _answerOptionButtons.Add(answerOption, buttonView);
               }
          }

          public void SetQuestionView(QuestionModel model)
          {
               foreach (var stimulusViews in _stimulusViews)
               {
                    stimulusViews.Value.gameObject.SetActive(false);
               }    
          
               if (_stimulusViews.ContainsKey(model.Category.StimulusType))
               {
                    _stimulusViews[model.Category.StimulusType].gameObject.SetActive(true);
                    _stimulusViews[model.Category.StimulusType].SetData(model.Category.Data);
               }
          }

          private void OnEnable()
          {
               LocalizationService.OnLanguageChanged += UpdateTip;
          }

          private void OnDisable()
          {
               LocalizationService.OnLanguageChanged -= UpdateTip;
          }

          private void UpdateTip()
          {
               phaseTipText.UpdateLocalizationKey(_phaseLocalizationTextKey);
          }

          public void UpdatePhaseTip(string localizationTextKey)
          {
               _phaseLocalizationTextKey = localizationTextKey;
               UpdateTip();
          }

          public void SetMistakeView(bool active)
          {
               mistakeView.gameObject.SetActive(active);
          }

          [System.Serializable]
          public class StimulusViewTypePrefab
          {
               public StimulusType Type;
               public StimulusView Prefab;
          }
     }
}
