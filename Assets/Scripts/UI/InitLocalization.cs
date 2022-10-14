using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using UnityEngine;

namespace UI
{
    public class InitLocalization : MonoBehaviour
    {
        [SerializeField] private Language Language;

        public void Awake()
        {
            LocalizationManager.Read();
            LocalizationManager.Language = Language.ToString();
            // LocalizationManager.Read();
            //
            // switch (Application.systemLanguage)
            // {
            //     case SystemLanguage.Russian:
            //         LocalizationManager.Language = "Russian";
            //         break;
            //     default:
            //         LocalizationManager.Language = "English";
            //         break;
            // }
        }

        public void SetLocalization(string localization)
        {
            LocalizationManager.Language = localization;
        }

    }

    public enum Language
    {
        Russian,
        English,
        Test
    }
}
