using System;
using System.Collections.Generic;
using Behaviours.Dir;
using External;
using GameData;
using Managers.Dir;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class AIDirector : Director
    {

        [SerializeField] public List<Building> Owns = new List<Building>();

        [SerializeField] public List<AmbitionContainer, AmbitionData> Experience = new List<AmbitionContainer, AmbitionData>();
        
        [SerializeField]
        public List<Director, FloatKeyData<Director>> Respects = new List<Director, FloatKeyData<Director>>();

        public bool TryGetConsent(Director candidate, AmbitionData ambition)
        {
            var ball = 0f;
            if (Experience.TryGetData(ambition.Key, out var expeirence))
            {
                ball += ambition.Factor / expeirence.Factor;
            }

            if (Respects.TryGetData(candidate, out var respect))
            {
                //TODO: Определиться какая дельта положительная будет у респекта
                ball += respect.Value;
            }


            if (Rand.IsChance(ball))
            {
                return true;
            }
            return false;
        }

        public LocalCompany CreateNewCompany(AmbitionData ambition, string companyNameKey)
        {
            var newCompany = ExecuterAsset.CreateLocalCompany(this, ambition.Key, companyNameKey);
            newCompany.AddEasyAmbition(this, ambition);
            newCompany.Resources.Add(ResourceType.Dollar, -ambition.Money);
            return newCompany;
        }
        
        public void SetAnswerForCreateNewCompany(AmbitionData ambition, ResolveType answer, float investments, string companyName)
        {
            float respectBall = 0;
            respectBall += Random.Range(0, 10 * (investments / ambition.Money));
            // var message = LocalKeys.CreateNewCompanyAnswer(answer, Altitude.Friendly, true, true);
            // City.Player.Answer(this ,message);
            LocalCompany newCompany;
            var chance = 1;
            var messageKey = "";
            switch (answer)
            {
                case ResolveType.No: respectBall -= Random.Range(0, 15);  break;
                case ResolveType.BigNo:
                    respectBall -= Random.Range(0, 15);
                    messageKey = LocalKeys.CreateNewCompanyAnswer(answer, Altitude.Friendly, true, true);
                    City.Player.Answer(this, messageKey);
                    break;
                
                case ResolveType.Yes:
                    respectBall += Random.Range(0, 10 * (investments / ambition.Money));
                    messageKey = LocalKeys.CreateNewCompanyAnswer(answer, Altitude.Friendly, true, true);
                    newCompany = CreateNewCompany(ambition, companyName);
                    newCompany.Soviet.Investment(City.Player, investments);
                    newCompany.Resources.Add(ResourceType.Dollar, investments);
                    City.Player.Money -= investments;
                    City.Player.Answer(this ,messageKey);
                    RebootAmbitionFinder();
                    break;
                
                case ResolveType.BigYes:
                    respectBall += Random.Range(0, 10 * (investments / ambition.Money));
                    messageKey = LocalKeys.CreateNewCompanyAnswer(answer, Altitude.Friendly, true, true);
                    newCompany = CreateNewCompany(ambition, companyName);
                    newCompany.Soviet.Investment(City.Player, investments);
                    newCompany.Resources.Add(ResourceType.Dollar, investments);
                    City.Player.Money -= investments;
                    City.Player.Answer(this ,messageKey);
                    RebootAmbitionFinder();
                    break;
                
                case ResolveType.Invest:
                    if (chance.IsChance())
                    {
                        respectBall += Random.Range(0, 10 * (investments / ambition.Money));
                        messageKey = LocalKeys.CreateNewCompanyAnswer(answer, Altitude.Friendly, true, true);
                        newCompany = CreateNewCompany(ambition, companyName);
                        newCompany.Soviet.Investment(City.Player, investments);
                        newCompany.Resources.Add(ResourceType.Dollar, investments);
                        City.Player.Money -= investments;
                        City.Player.Answer(this ,messageKey);
                        RebootAmbitionFinder();
                    }
                    else
                    {
                        messageKey = LocalKeys.CreateNewCompanyAnswer(answer, Altitude.Friendly, true, false);
                        City.Player.Answer(this ,messageKey);
                    }
                    break;
                
                case ResolveType.BigInvest:
                    if (chance.IsChance())
                    {
                        respectBall += Random.Range(0, 10 * (investments / ambition.Money));
                        messageKey = LocalKeys.CreateNewCompanyAnswer(answer, Altitude.Friendly, true, true);
                        newCompany = CreateNewCompany(ambition, companyName);
                        newCompany.Soviet.Investment(City.Player, investments);
                        newCompany.Resources.Add(ResourceType.Dollar, investments);
                        City.Player.Money -= investments;
                        City.Player.Answer(this ,messageKey);
                        RebootAmbitionFinder();
                    }
                    else
                    {
                        messageKey = LocalKeys.CreateNewCompanyAnswer(answer, Altitude.Friendly, true, false);
                        City.Player.Answer(this ,messageKey);
                    }
                    break;
                
                case ResolveType.Require:
                    if (chance.IsChance())
                    {
                        respectBall += Random.Range(0, 10 * (investments / ambition.Money));
                        messageKey = LocalKeys.CreateNewCompanyAnswer(answer, Altitude.Friendly, true, true);
                        newCompany = CreateNewCompany(ambition, companyName);
                        newCompany.Soviet.Investment(City.Player, investments);
                        newCompany.Resources.Add(ResourceType.Dollar, investments);
                        City.Player.Money -= investments;
                        City.Player.Answer(this ,messageKey);
                        RebootAmbitionFinder();

                    }
                    else
                    {
                        messageKey = LocalKeys.CreateNewCompanyAnswer(answer, Altitude.Friendly, true, false);
                        City.Player.Answer(this ,messageKey);
                    }
                    break;
                
                case ResolveType.BigRequire:
                    if (chance.IsChance())
                    {
                        respectBall += Random.Range(0, 10 * (investments / ambition.Money));
                        messageKey = LocalKeys.CreateNewCompanyAnswer(answer, Altitude.Friendly, true, true);
                        newCompany = CreateNewCompany(ambition, companyName);
                        newCompany.Soviet.Investment(City.Player, investments);
                        newCompany.Resources.Add(ResourceType.Dollar, investments);
                        City.Player.Money -= investments;
                        City.Player.Answer(this ,messageKey);
                        RebootAmbitionFinder();
                    }
                    else
                    {
                        messageKey = LocalKeys.CreateNewCompanyAnswer(answer, Altitude.Friendly, true, false);
                        City.Player.Answer(this ,messageKey);
                    }
                    break;
            }
        }
        
        public void SetAnswerForNewCandidateConsent(LocalCompany company, AmbitionData ambition, bool isOk)
        {
            var finder =  GetComponent<FindAmbitionRealization>();
            var creator = GetComponent<DirAmbitionCreator>();
            if (isOk)
            {
                finder.ClearFind();
                company.AddEasyAmbition(this, ambition);
                Money -= ambition.Money;
                CompanyCash.Add(company);
                Experience.Add(ambition);
                creator.enabled = true;
                return;
            }

            finder.enabled = true;
        }
        protected override void OnUpdateLog()
        {
            Log += $"CompanyCash: {CompanyCash.Count}\n";
            foreach(var company in CompanyCash){
                Log += $"{company} \n";
            }

            Log += $"Собственность:\n";
            foreach(var own in Owns){
                Log += $"{own.name}\n";
            }
            base.OnUpdateLog();
        }
        
        public void SendQueryForCreateNewCompany(AmbitionData ambition)
        {
            var queryMoney = (int) Random.Range(ambition.Factor - ambition.Money, ambition.Money);
            queryMoney = (queryMoney < 0) ? 0 : queryMoney;
            string companyName = LocalKeys.CompanyName(ambition.Output);
            
            
            // City.Player.CreateNewCompanyWithHelp(this, ambition,queryMoney, companyName); 
            if (queryMoney > 0)
            {
                City.Player.CreateNewCompanyWithHelp(this, ambition,queryMoney, companyName); 
            }
            else
            {
                City.Player.CreateNewCompany(this, ambition, companyName);
            }
        }

        private void RebootAmbitionFinder()
        {
            GetComponent<FindAmbitionRealization>().ClearFind();
            GetComponent<DirAmbitionCreator>().enabled = true;
        }
        
        public void OnDestroy()
        {
            //TODO: Раздать бедным или тем кого уважает
            foreach (var own in Owns)
            {
                
            }
            foreach (var company in CompanyCash)
            {
                
            }
        }
        
    }
}
