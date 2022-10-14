using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIDirectorMessage : UIBaseMessage
    {
        [SerializeField] private SimpleLocalizeText _nameLocalize;
        [SerializeField] private TextMeshProUGUI _companiesData;
        [SerializeField] private TextMeshProUGUI _moneyData;
        private AIDirector _director;
        
        public void Init(AIDirector director)
        {
            _director = director;
            _nameLocalize.Key = director.FirstNameKey;
            _avatar.sprite = director.Avatar;
            base.Init();
        }
        public override void OnChangeSelected(bool isSelected)
        {
            
        }

        public override void Localize()
        {
            
        }
        
        public void OnEnable()
        {
            base.OnEnable();
            Upd();
        }

        public void Upd()
        {
            var companyData = "";
            foreach (var company in _director.CompanyCash)
            {
                companyData += $"{company.FirstName} {company.Members.GetValue(_director)}%\n";
            }
            _companiesData.text = companyData;
            _moneyData.text = $"{(int)_director.Money}$";

        }
        
    }
}
