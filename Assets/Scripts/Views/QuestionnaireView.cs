using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using Views;

public class QuestionnaireView : ScreenView
{
     [SerializeField] private Transform buttonOptionsContainer;
     [SerializeField] private Transform characteristicsContainer;
     
     [SerializeField] private ButtonOptionView buttonOptionViewPrefab;
     [SerializeField] private CharacteristicViewTypePrefab[] characteristicViewTypePrefabs;

     [SerializeField] private GameObject mistakeView;
     
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
          foreach (var characteristic in model.Characteristics)
          {
               if (_characteristicViews.ContainsKey(characteristic.DataType))
                    _characteristicViews[characteristic.DataType].SetData(characteristic.Data);
          }
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
