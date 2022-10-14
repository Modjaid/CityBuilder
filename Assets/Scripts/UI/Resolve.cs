using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Resolve : MonoBehaviour
    {
        [SerializeField] private bool _isPositive;
        [SerializeField] private ResolveType _answer;
        [SerializeField] private LocalizeText _localized;

        public string GetText => _localized.Text.text;
        public bool IsPositive => _isPositive;
        public ResolveType Answer
        {
            get => _answer;
            set => _answer = value;
        }
        private UIBaseMessage _message;
        
        public void Subscribe(UIBaseMessage message){
            _message = message;
            transform.GetComponentInChildren<Button>().onClick.AddListener(() => _message.OnPressResolve(_answer));
        }

        public void AddData(string data)
        {
            _localized.SetTags(new Dictionary<string, string>()
            {
                {"<Data>", data}
            });
            
        }
        
        // public string LocalKey{}
    }
}