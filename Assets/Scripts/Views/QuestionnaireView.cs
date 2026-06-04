using System.Collections.Generic;
using Models;
using Services;
using TMPro;
using UnityEngine;
using Views;

public class QuestionnaireView : View
{
     [SerializeField] private Transform buttonOptionsContainer;
     [SerializeField] private Transform characteristicsContainer;
     
     [SerializeField] private ButtonOptionView buttonOptionViewPrefab;
     [SerializeField] private CharacteristicViewTypePrefab[] characteristicViewTypePrefabs;

     [SerializeField] private GameObject mistakeView;
     [SerializeField] private TMP_Text phaseTipText;
     
     private Dictionary<AnswerOptionModel, ButtonOptionView> _answerOptionButtons;
     private Dictionary<CharacteristicType, CharacteristicView> _characteristicViews;
     
     public IReadOnlyDictionary<AnswerOptionModel, ButtonOptionView> AnswerOptionButtons => _answerOptionButtons;
     
     public void Initialize(AnswerOptionModel[] answerOptions)
     {
          for (int i = 0; i < buttonOptionsContainer.childCount; i++)
          {
               Destroy(buttonOptionsContainer.GetChild(i).gameObject);
          }
          
          for (int i = 0; i < characteristicsContainer.childCount; i++)
          {
               Destroy(characteristicsContainer.GetChild(i).gameObject);
          }

          _characteristicViews = new();

          foreach (var element in characteristicViewTypePrefabs)
          {
               var characteristicView = Instantiate(element.Prefab, characteristicsContainer);
               _characteristicViews.Add(element.Type, characteristicView);
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
          foreach (var characteristicViews in _characteristicViews)
          {
               characteristicViews.Value.gameObject.SetActive(false);
          }    
          
          foreach (var characteristic in model.Characteristics)
          {
               if (_characteristicViews.ContainsKey(characteristic.DataType))
               {
                    _characteristicViews[characteristic.DataType].gameObject.SetActive(true);
                    _characteristicViews[characteristic.DataType].SetData(characteristic.Data);
               }
          }
     }

     public void SetPhaseTip(string localizationTextKey)
     {
          phaseTipText.text = LocalizationService.GetValue(localizationTextKey);
     }

     public void SetMistakeView(bool active)
     {
          mistakeView.gameObject.SetActive(active);
     }

     [System.Serializable]
     public class CharacteristicViewTypePrefab
     {
          public CharacteristicType Type;
          public CharacteristicView Prefab;
     }
}
