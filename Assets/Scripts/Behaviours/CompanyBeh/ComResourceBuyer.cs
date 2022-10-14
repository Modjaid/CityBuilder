using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameData;
using GameData.Resources;
using Managers;
using UnityEngine;
using static Managers.AIDirector;

namespace Behaviours.Com
{
    public class ComResourceBuyer : DelayBeh<LocalCompany>
    {
        [SerializeField] private List<ResourceType, PrioritetData> _prioritets;
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
            var allMoney = _resources.GetValue(ResourceType.Dollar);
            if (allMoney == 0) return;
            var priors =  UpdAmbitionPrioritets();
            var sumPrioritets = CalcSumPrioritets(priors);
            var budget = allMoney * (sumPrioritets * 0.01f);
            budget = (budget > allMoney) ? allMoney : budget;
            priors.Sort((a, b) => b.Prioritet.CompareTo(a.Prioritet));

            foreach (var prior in priors)
            {
                var marketRow = _market.GetSection(prior.Key);
                var sectionBudget = (int)((prior.Prioritet / sumPrioritets) * budget);
                if (marketRow.Count > 0 && sectionBudget > 0)
                {
                    var report = marketRow.BuyByMoney(prior.Cash);
                    _resources.Add(prior.Key, report.goods, report.middlePrice);
                    prior.Cash -= (int)report.passMoney;
                    _prioritets.Set(prior);
                    if (sectionBudget > prior.Cash * 2)
                    {
                        prior.Cash += sectionBudget;
                        _prioritets.Set(prior);
                        _resources.Add(ResourceType.Dollar, -sectionBudget);
                        return;
                    }
                }
            }
            await UniTask.Yield();
        }

        public void AddPrioritet(ResourceType res, float prioritet)
        {
            _prioritets.Add(res, prioritet);
        }
        public void RemovePrioritet(ResourceType res)
        {
            _prioritets.Clear(res);
        }
        private float CalcSumPrioritets(List<PrioritetData> priors)
        {
            var sum = 0f;
            foreach(var prior in priors){
                sum += prior.Prioritet;
            }
            return sum;
        }

        private List<PrioritetData> UpdAmbitionPrioritets()
        {
            var factors = new Dictionary<ResourceType, (float multipleTarget, float commonFactor)>();
            var updPrioritets = new List<PrioritetData>();
            if (transform.TryGetAmbitions<AmbitionContainer>(out var ambitions))
            {
                foreach (var amb in ambitions)
                {
                    _prioritets.TryGetData(amb.Input, out var prioritet);
                    var resources = _resources.GetValue(amb.Input);
                    if (resources == 0)
                    {
                        resources = 1;
                    }
                    prioritet.Prioritet = (amb.Factor * 2f) / resources;
                    _prioritets.Set(prioritet);
                    updPrioritets.Add(prioritet);
                }
            }
            return updPrioritets;
        }

    }
}
