using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviours.Dir;
using Cysharp.Threading.Tasks;
using GameData;
using Managers.Dir;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class Player : Director
    {
        [SerializeField] private int _newCandidateMessageDelay = 30;
        [SerializeField] private int _newCompanyMessageDelay = 30;
        private Mail _mail;

        public override string FirstName
        {
            get => _firstNameKey;
            set => _firstNameKey = value;
        }

        public override string LastName
        {
            get => _lastNameKey;
            set => _lastNameKey = value;
        }

        public override void Init()
        {
            base.Init();
            City.Player = this;
            _mail = FindObjectOfType<Mail>();
        }

        public void NewCandidateConsent(AIDirector candidate, AmbitionData ambition, LocalCompany company,
            float currentVotes)
        {
            var mailMessage = _mail.CreateNewCandidateMessage(candidate, ambition, company);
            mailMessage.AddResolveOptions((answer) =>
                {
                    if (answer == ResolveType.Yes)
                    {
                        candidate.SetAnswerForNewCandidateConsent(company, ambition,true);
                    }
                    else
                    {
                        candidate.SetAnswerForNewCandidateConsent(company, ambition,false);
                    }
                },
                new[]
                {
                    (ResolveType.Yes, ""), (ResolveType.No, "")
                },
                _mail.NewCandidateResolves,
                _newCandidateMessageDelay,
                ResolveType.No);
        }

        public void CreateNewCompany(AIDirector director, AmbitionData ambition, string companyNameKey)
        {
            var mailMessage = _mail.CreateNewCompanyMessage(director, ambition, companyNameKey, 0);
            var creator = director.GetComponent<FindAmbitionRealization>();
            var resolves = new List<(ResolveType type, string data)>();
            resolves.Add((ResolveType.Yes, ""));
            resolves.Add((ResolveType.No,""));
            resolves.Add((ResolveType.Require,""));
            resolves.Add((ResolveType.BigRequire, ""));
            var invest = (int) Random.Range(ambition.Money / 5, ambition.Money * 1.5f);
            if (invest < Money)
            {
                resolves.Add((ResolveType.Invest, invest.ToString()));
            }

            var bigInvest = (int) Random.Range(ambition.Money, Money / 3);
            if (bigInvest < Money)
            {
                resolves.Add((ResolveType.BigInvest, bigInvest.ToString()));
            }
            resolves.Add((ResolveType.BigNo, ""));
            mailMessage.AddResolveOptions((answer) =>
                {
                    var resulInvestments = 0;
                    switch (answer)
                    {
                        case ResolveType.Invest:
                            resulInvestments = invest;
                            break;
                        case ResolveType.BigInvest:
                            resulInvestments = bigInvest;
                            break;
                    }
                    director.SetAnswerForCreateNewCompany(ambition, answer, resulInvestments, companyNameKey);
                },
                resolves.ToArray(),
                _mail.NewCompanyResolves,
                _newCompanyMessageDelay,
                ResolveType.No
            );
        }

        public void CreateNewCompanyWithHelp(AIDirector director, AmbitionData ambition, int helpMoney ,string companyNameKey)
        {
            var mailMessage = _mail.CreateNewCompanyMessage(director, ambition, companyNameKey, helpMoney);
            var creator = director.GetComponent<FindAmbitionRealization>();
            var resolves = new List<(ResolveType type, string data)>();
            if (helpMoney < Money)
            {
                resolves.Add((ResolveType.Yes, helpMoney.ToString()));
            }
            resolves.Add((ResolveType.BigRequire, "с нашей компанией говна"));
                
            var investPotential = ((Money / helpMoney));
            var requierSum = (investPotential > 1) ? 1 : investPotential;
            requierSum = (Random.Range(0, requierSum / 3) * helpMoney);
            resolves.Add((ResolveType.Require, ((int) requierSum).ToString()));
            var investments = (int) Random.Range(1, ((investPotential - 1) / 2) + 1) * helpMoney;
            if (investPotential > 1)
            {
                resolves.Add((ResolveType.Invest, (investments).ToString()));
                resolves.Add((ResolveType.BigYes, (investments).ToString()));
            }
            resolves.Add((ResolveType.No, ""));
                
            mailMessage.AddResolveOptions((answer) =>
                {
                    var resulInvestments = 0;
                    switch (answer)
                    {
                        case ResolveType.Invest:
                            resulInvestments = investments;
                            break;
                        case ResolveType.BigYes:
                            resulInvestments = investments;
                            break;
                        case ResolveType.Yes:
                            resulInvestments = (int) requierSum;
                            break;
                        case ResolveType.Require:
                            resulInvestments = (int) requierSum;
                            break;
                    }
                    director.SetAnswerForCreateNewCompany(ambition, answer, resulInvestments, companyNameKey);
                },
                resolves.ToArray(),
                _mail.NewCompanyHelpHersolves,
                _newCompanyMessageDelay,
                ResolveType.No
            );
        }

        public void Answer(AIDirector sender, string textKey)
        {
            var mailMessage = _mail.CreateSimpleMessage(sender, textKey);
        }
    }
}
