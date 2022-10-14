using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UI;
using UnityEngine;

namespace UI
{
    public abstract class UICompanyMessage<T> : UIBaseMessage where T : Company
    {
        [SerializeField] private SimpleLocalizeText _nameLocalize;
        
        protected T _company;
        private HardDialog _hardDialog;
        private Dictionary<string, string> _tags;

        public virtual void Init(T company)
        {
            _company = company;
            _nameLocalize.Key = company.FirstNameKey;
            base.Init();
        }
        public void OnEnable()
        {
            base.OnEnable();
            Upd();
        }

        public bool IsMatch(Company company)
        {
            return company == _company;
        }
        public abstract void Upd();
    }
}