using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Assets.SimpleLocalization;
using TMPro;
using UnityEngine;

namespace UI
{
    public class SimpleLocalizeText : LocalizeText
    {
        [SerializeField] protected string _messageKey;
        public override void Init()
        {
            Key = _messageKey;
            base.Init();
        }
    }

}