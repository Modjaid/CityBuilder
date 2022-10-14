using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameData;
using GameData.Resources;
using Managers;
using UnityEngine;

namespace Behaviours.Com
{
    public class CityPopulationManager : DelayBeh<CitizenCompany>
    {
        [SerializeField] private float _startGrow;
        [SerializeField] private float _speed;
        private List<ResourceType, ResourceData> _resources;
        public override void Init()
        {
            base.Init();
            _resources = _executer.Resources;
        }

        public async override UniTask Execute()
        {
            var happy = _resources.GetValue(ResourceType.HappyScore);
            var citizens = _resources.GetValue(ResourceType.Citizen);

            var offset = happy - _startGrow;
            // if (happy > _startGrow)
            // {
            //     offset = happy - _startGrow;
            // }
            // else
            // {
            //     offset = _startGrow - happy;
            //     offset = citizens * offset * _speed;
            // }
            
            _resources.Add(ResourceType.Citizen, citizens * offset * _speed);
        }
    }
}
