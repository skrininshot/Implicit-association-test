using System;
using UnityEngine;

namespace Views
{
    public class View : MonoBehaviour
    {
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}