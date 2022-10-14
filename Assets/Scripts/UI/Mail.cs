using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Linq;
using GameData;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Mail : MonoBehaviour
    {
        [SerializeField] private GameObject _messagePrefab;
        [SerializeField] private Transform _content;
        
        
        [SerializeField] private ResolveData[] _newCandidateResolves;
        [SerializeField] private ResolveData[] _newCompanyResolves;
        [SerializeField] private ResolveData[] _newCompanyResolvesWithHelps;

        public ResolveData[] NewCandidateResolves => _newCandidateResolves;
        public ResolveData[] NewCompanyResolves => _newCompanyResolves;
        public ResolveData[] NewCompanyHelpHersolves => _newCompanyResolvesWithHelps;
        
        private City _city;

        private void Start()
        {
            _city = FindObjectOfType<City>();
        }
        
        public UIMailMessage CreateSimpleMessage(AIDirector sender, string messageKey)
        {
            var uiMessage = Instantiate(_messagePrefab, _content).GetComponent<UIMailMessage>();
            uiMessage.Init();
            uiMessage.AddLocalization(sender.FirstNameKey, 
                sender.LastNameKey, 
                messageKey,
                new Dictionary<string, string>()
            );
            uiMessage.Localize();
            return uiMessage;
        }
        public UIMailMessage CreateNewCandidateMessage(AIDirector candidate, AmbitionData ambition, LocalCompany company)
        {
            var uiMessage = Instantiate(_messagePrefab, _content).GetComponent<UIMailMessage>();
            uiMessage.Init();
            var messageKey = LocalKeys.NewCandidateMessage(Altitude.Friendly, false);
            uiMessage.AddLocalization(candidate.FirstNameKey, candidate.LastNameKey, messageKey,
            new Dictionary<string, string>()
            {
                {"<CompanyName>",company.FirstNameKey}
            });
            
            uiMessage.AddTagsData(new Dictionary<string, string>{
                {"<Money>", ((int) ambition.Money).ToString()},
                {"<PlayerFirstName>", _city.Player.FirstNameKey},
                {"<PlayerLastName>", _city.Player.LastNameKey},
            });
            uiMessage.Localize();
            return uiMessage;
        }

        public UIMailMessage CreateNewCompanyMessage(AIDirector candidate, AmbitionData ambition, string companyNameKey, int helpMoney)
        {
            var uiMessage = Instantiate(_messagePrefab, _content).GetComponent<UIMailMessage>();
            uiMessage.Init();
            var messageKey = "";
            if (helpMoney <= 0)
            {
                messageKey = LocalKeys.CreateNewCompanyMessage(Altitude.Friendly, true);
            }
            else
            {
                messageKey = LocalKeys.CreateNewCompanyMessageWithHelp(Altitude.Friendly, true);
            }
            uiMessage.AddLocalization(candidate.FirstNameKey, candidate.LastNameKey, messageKey,
                new Dictionary<string, string>()
                {
                    {"<CompanyName>", companyNameKey},
                    {"<OutResource>", ambition.Output.ToString()},
                    {"<InResource>", ambition.Input.ToString()}
                });
            
            uiMessage.AddTagsData(new Dictionary<string, string>{
                {"<Money>", ambition.Money.ToString()},
                {"<PlayerFirstName>", _city.Player.FirstNameKey},
                {"<PlayerLastName>", _city.Player.LastNameKey},
                {"<HelpMoney>", helpMoney.ToString()}
            });
            uiMessage.Localize();
            return uiMessage;
        }

    }
}
