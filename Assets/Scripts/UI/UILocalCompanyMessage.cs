using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UILocalCompanyMessage : UICompanyMessage<LocalCompany>
    {
        [SerializeField] private TextMeshProUGUI _produceText;
        [SerializeField] private TextMeshProUGUI _rentAreaText;
        [SerializeField] private TextMeshProUGUI _inputsText;
        [SerializeField] private TextMeshProUGUI _directors;
        [SerializeField] private TextMeshProUGUI _desctiptionText;
        
        public override void Upd()
        {
            _produceText.text = LocalizationManager.Localize(_company.Output.ToString());
            _rentAreaText.text = $"{_company.SpaceArea} m2";

            var resourceInfo = "";
            _company.Resources.Foreach((res) =>
            {
                var resType = LocalizationManager.Localize(res.Key.ToString());
                resourceInfo += $"{res.Value} {resType},";
            });
            _inputsText.text = resourceInfo;
            
            var directorsInfo = "";
            _company.Members.Foreach((dir) =>
            {
                var firstName = dir.Key.FirstName;
                var lastName =  dir.Key.LastName;
                directorsInfo += $"{firstName} {lastName} {dir.Value}%\n";

            });
            _directors.text = directorsInfo;
        }

        public override void OnChangeSelected(bool isSelected)
        {
            _inputsText.gameObject.SetActive(isSelected);
            _directors.gameObject.SetActive(isSelected);
            _desctiptionText.gameObject.SetActive(isSelected);
        }

        public override void Localize()
        {
            // _produceText
        }
    }
}
