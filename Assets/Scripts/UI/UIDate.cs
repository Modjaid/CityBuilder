using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIDate : MonoBehaviour
    {
        private TextMeshProUGUI _date;

        void Start()
        {
            _date = GetComponent<TextMeshProUGUI>();
            TimeLaps.Instance.OnNextStep += OnNextDay;
        }

        private void OnNextDay()
        {
            var currentStep = TimeLaps.Instance.CurrentDay;
            var date = DateTime.Today.AddDays(currentStep);
            _date.text = date.ToString("dd/MMMM/yyyy");
        }

        private void OnDestroy()
        {
            TimeLaps.Instance.OnNextStep -= OnNextDay;
        }

    }
}
