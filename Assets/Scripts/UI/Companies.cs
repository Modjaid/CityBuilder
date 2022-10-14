using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Managers;
using Managers.Com;
using UnityEngine;

namespace UI
{
    public class Companies : MonoBehaviour
    {
        [SerializeField] private GameObject _localCompanyMessagePrefab;
        [SerializeField] private GameObject _cityCompanyMessagePrefab;
        [SerializeField] private Transform _content;
        [SerializeField] private City _city;
        private List<UILocalCompanyMessage> _companies;

        public void Awake()
        {
            _companies = new List<UILocalCompanyMessage>();
            _city.OnChangeExecutors += OnChangeCityExecutors;
        }
        

        public void OnDisable()
        {
            
        }

        public void OnAddCompany()
        {
            
        }

        public void OnChangeCityExecutors(bool isAdd, Executer exe)
        {
            if (isAdd)
            {
                switch (exe)
                {
                    case LocalCompany com :
                        var message = Instantiate(_localCompanyMessagePrefab, _content)
                            .GetComponent<UILocalCompanyMessage>();
                        message.Init(com);
                        break;
                    case CitizenCompany com: 
                        var message2 = Instantiate(_cityCompanyMessagePrefab, _content)
                            .GetComponent<UICityCompanyMessage>();
                        message2.Init(com);
                        message2.transform.SetSiblingIndex(0);
                        break;
                    case PartyCompany com: 
                        break;
                    case ForeignCompany com:
                        break;
                }
            }
        }

        // public UICompanyMessage CreateNewCompanyMessage()
        // {
        //     var uiMessage = Instantiate(_messagePrefab, _content).GetComponent<UIMailMessage>();
        //     uiMessage.Init();
        //     var messageKey = "";
        //     if (helpMoney <= 0)
        //     {
        //         messageKey = LocalKeys.CreateNewCompanyMessage(Altitude.Friendly, true);
        //     }
        //     else
        //     {
        //         messageKey = LocalKeys.CreateNewCompanyMessageWithHelp(Altitude.Friendly, true);
        //     }
        //     uiMessage.AddLocalization(candidate.FirstNameKey, candidate.LastNameKey, messageKey,
        //         new Dictionary<string, string>()
        //         {
        //             {"<CompanyName>", companyNameKey},
        //             {"<OutResource>", ambition.Output.ToString()},
        //             {"<InResource>", ambition.Input.ToString()}
        //         });
        //     
        //     uiMessage.AddTagsData(new Dictionary<string, string>{
        //         {"<Money>", ambition.Money.ToString()},
        //         {"<PlayerFirstName>", _city.Player.FirstName},
        //         {"<PlayerLastName>", _city.Player.LastName},
        //         {"<HelpMoney>", helpMoney.ToString()}
        //     });
        //     uiMessage.Localize();
        //     return uiMessage;
        // }
    }
}
