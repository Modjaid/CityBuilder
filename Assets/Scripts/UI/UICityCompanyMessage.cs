using System;
using System.Collections;
using System.Collections.Generic;
using Assets.SimpleLocalization;
using GameData;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UICityCompanyMessage : UICompanyMessage<CitizenCompany>
    {
        [SerializeField] private TextMeshProUGUI _residentsDataText;
        [SerializeField] private TextMeshProUGUI _resourcePerCitizenText;
        [SerializeField] private TextMeshProUGUI _resourcePerCitizenDataText;
        [SerializeField] private TextMeshProUGUI _descriptionDataText;
        public override void Upd()
        {
            var residentsData = "";
            var resourcePerUnitData = "";

            var citizens = (int) _company.Resources.GetValue(ResourceType.Citizen) + 1;
            _company.Resources.Foreach((res) =>
            {
                var resName = LocalizationManager.Localize(res.Key.ToString());
                if (res.Key.IsHuman())
                {
                    residentsData += $"{resName}:{res.Value}  ";
                }
                else
                {
                    resourcePerUnitData += $"{resName}:{res.Value / citizens}  ";
                }
            });
            _residentsDataText.text = residentsData;
            _resourcePerCitizenDataText.text = resourcePerUnitData;
        }
        

        public override void OnChangeSelected(bool isSelected)
        {
            _resourcePerCitizenText.gameObject.SetActive(isSelected);
            _resourcePerCitizenDataText.gameObject.SetActive(isSelected);
            _descriptionDataText.gameObject.SetActive(isSelected);
        }

        public override void Localize()
        {
            // _produceText
        }
    }
}
