using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.SimpleLocalization;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIMailMessage : UIBaseMessage
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _message;
        [SerializeField] private TextMeshProUGUI _pressedAnswer;
        private HardDialog _hardDialog;
        private string _lastNameKey;
        private string _firstNameKey;
        private Dictionary<string, string> _tags;

        
        public override void Init()
        {
            transform.SetSiblingIndex(0);
            base.Init();
            _hardDialog = FindObjectOfType<HardDialog>();
        }
        
        public void AddLocalization(string firstNameKey, string lastNameKey,string messageKey, Dictionary<string, string> tags)
        {
            _lastNameKey = lastNameKey;
            _firstNameKey = firstNameKey;
            _messageKey = messageKey;
            _tags = tags;
        }

        public override void Localize()
        {
            var firstName = LocalizationManager.Localize(_firstNameKey);
            var lastName = LocalizationManager.Localize(_lastNameKey);
            
            _name.text = $"{firstName} {lastName}";
            var message = LocalizationManager.LocalizeWithTags(_messageKey, _tags);
            message = UpdTextTagData(message);
            _message.text = message;
        }

        public override void OnMessageTimeOut()
        {
            // TimeLaps.Instance.Pause = true;
            // _hardDialog.transform.GetChild(0).gameObject.SetActive(true);
            ClearButtons();
        }
        
        public override void OnChangeSelected(bool isSelected)
        {
            _message.enableWordWrapping = isSelected;
            if (isSelected)
            {
                _message.overflowMode = TextOverflowModes.Overflow;
            }
            else
            {
                _message.overflowMode = TextOverflowModes.Ellipsis;
            }
        }
        public override void OnPressResolve(ResolveType resolve)
        {
            var button = _dialogButtons.Find((button) => button.Answer == resolve);
            _pressedAnswer.text = button.GetText;
            base.OnPressResolve(resolve);
        }
    }
}
