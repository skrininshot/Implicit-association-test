using System;
using UnityEngine;

namespace Views
{
    public class ScreenView : MonoBehaviour
    {
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}