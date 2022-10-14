using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using GameData;
using GameData.Companies;
using GameData.Resources;
using Managers;
using UnityEngine;

namespace Behaviours.Com
{
    public class CityHappyEstimater : DelayBeh<CitizenCompany>
    {
        [SerializeField] private List<ResourceType, PrioritetData> _prioritets;
        private List<ResourceType, ResourceData> _resources;


        public override void Init()
        {
            base.Init();
            _resources = _executer.Resources;

            if(TryGetComponent<CityResourceBuyer>(out var resourceBuyer)){
                _prioritets = resourceBuyer.Prioritets;
            }else{
                Debug.LogError($"CityResourceBuyer for CitizenCompany not found");
            }
        }

        public async override UniTask Execute()
        {
            // var citizens = 1f;
            // citizens = _resources.GetValue(ResourceType.Citizen);
            // var homes = _spaces.GetSumValues();
            // var homeBall = (homes / citizens) - 1;
            // var sumPriors = 0f;
            // _prioritets.Foreach((prior) => {
            //     (prior.Value * citizens)
            // });
        }
    }
}
