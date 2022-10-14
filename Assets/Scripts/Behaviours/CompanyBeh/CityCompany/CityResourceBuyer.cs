using System;
using System.Collections;
using System.Collections.Generic;
using Behaviours;
using Cysharp.Threading.Tasks;
using GameData;
using GameData.Resources;
using Managers;
using UnityEngine;

namespace Behaviours.Com
{
    public class CityResourceBuyer : DelayBeh<Company>
    {
        [SerializeField] private List<ResourceType, PrioritetData> _prioritets;

        public List<ResourceType, PrioritetData> Prioritets => _prioritets;
        private ResourceMarket _market;
        private List<ResourceType, ResourceData> _resources;
        public override void Init()
        {
            base.Init();
            _market = _executer.City.Market;
            _resources = _executer.Resources;
        }

        public async override UniTask Execute()
        {
            var sumPrioritets = 0f;
            var cityMoney = _resources.GetValue(ResourceType.Dollar);
            var citizens = _resources.GetValue(ResourceType.Citizen);
            if (citizens == 0 || cityMoney == 0) return;
            sumPrioritets = CalcSumPrioritets(citizens);
            var budget = (cityMoney / 10) * sumPrioritets;
            budget = (budget > cityMoney) ? cityMoney : budget;

            var sortPrioritets = _prioritets.ToList();
            sortPrioritets.Sort((a, b) => b.Prioritet.CompareTo(a.Prioritet));

            foreach (var p in sortPrioritets)
            {
                var marketRow = _market.GetSection(p.Key);
                var sectionBudget = (int)((p.Prioritet / sumPrioritets) * budget);
                if (marketRow.Count > 0 && sectionBudget > 0)
                {
                    var report = marketRow.BuyByMoney(p.Cash);
                    _resources.Add(p.Key, report.goods, report.middlePrice);
                    p.Cash -= (int)report.passMoney;
                    _prioritets.Set(p);
                    if (sectionBudget > p.Cash * 2)
                    {
                        p.Cash += sectionBudget;
                        _prioritets.Set(p);
                        _resources.Add(ResourceType.Dollar, -sectionBudget);
                        return;
                    }
                }
            }
        }

        public void AddPrioritet(ResourceType res, float prioritet)
        {
            _prioritets.Add(res, prioritet);
        }
        public void RemovePrioritet(ResourceType res)
        {
            _prioritets.Clear(res);
        }
        private float CalcSumPrioritets(float citizens)
        {
            var result = 1f;
            _prioritets.Foreach((prior) =>
            {
                var resources = _resources.GetValue(prior.Key);
                if (resources == 0)
                {
                    resources = 1;
                }
                prior.Prioritet = (citizens * prior.Value) / resources;
                
                var happyScore = resources / (citizens * prior.Value);
                if (happyScore > 1) happyScore = 1;
                _resources.Add(ResourceType.HappyScore, happyScore);
                result += prior.Prioritet;
                return prior;
            });

            return result;
        }

    }
}
