using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI
{
    public class OnEnableCopyText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _copyText;

        private void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
        }

        [ContextMenu("Copy")]
        public void OnValidate()
        {
            GetComponent<TextMeshProUGUI>().text = _copyText.text;
        }

        private void OnTextChanged(Object obj)
        {
            if (obj == _copyText)
                GetComponent<TextMeshProUGUI>().text = _copyText.text;
        }

        private void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
        }
    }
}
