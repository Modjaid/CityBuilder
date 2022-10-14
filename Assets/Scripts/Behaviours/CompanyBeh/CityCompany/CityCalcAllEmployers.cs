using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameData;
using GameData.Resources;
using Managers;
using UnityEngine;

namespace Behaviours.Com
{
    public class CityCalcAllEmployers : DelayBeh<CitizenCompany>
    {
        private List<ResourceType, ResourceData> _resources;
        public override void Init()
        {
            base.Init();
            _resources = _executer.Resources;
        }
        public async override UniTask Execute()
        {
            var cash = new Dictionary<ResourceType, float>();
            var allCompanies = _executer.City.FindExecuters<LocalCompany>();
            foreach (var company in allCompanies)
            {
                company.Resources.Foreach((res) =>
                {
                    if (res.Key.IsEmployer())
                    {
                        if (cash.ContainsKey(res.Key))
                        {
                            cash[res.Key] += res.Value;
                        }
                        else
                        {
                            cash[res.Key] = res.Value;
                        }
                    }
                });
            }
            foreach (var res in cash)
            {
                _resources.Set(res.Key, (int)res.Value);
            }
        }
    }

}
