
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameData;
using GameData.Resources;
using Managers;
using UnityEngine;

namespace Behaviours.Com
{
    public class ComEasyBenefit : DelayBeh<Company>
    {
        [SerializeField] private float _countBenefit;
        [SerializeField] private ResourceType _type = ResourceType.Dollar;
        private List<ResourceType, ResourceData> _resources;
        public override void Init()
        {
            base.Init();
            _resources = _executer.Resources;
        }

        public async override UniTask Execute()
        {
            _resources.Add(_type, _countBenefit);
        }
    }
}
