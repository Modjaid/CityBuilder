using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameData;
using GameData.Resources;
using Managers;
using UnityEngine;

namespace Behaviours.Com
{
    public class ComResourceSpender : DelayBeh<Company>
    {
        [SerializeField] private List<SpendData> _spendResources;
        private List<ResourceType, ResourceData> _resources;
        public override void Init()
        {
            base.Init();
            _resources = _executer.Resources;
        }

        public async override UniTask Execute()
        {
            var citizens = _resources.GetValue(ResourceType.Citizen);
            foreach (var spendRes in _spendResources)
            {
                _resources.Add(spendRes.Type, -spendRes.Spend * citizens);
            }
        }

        [Serializable]
        public class SpendData
        {
            [SerializeField] public ResourceType Type;
            [Header("Затраты на одного жителя")]
            public float Spend;
        }
    }
    
}
