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
    public class AmbSoftResourceFactor : AmbitionRealizator
    {
        [SerializeField] private float _multiplePrice = 40f;
        private ResourceMarket _market;
        private List<ResourceType, ResourceData> _resources;
        private float ResourceSpend = 0.1f;
        public override void Init()
        {
            base.Init();
            _resources = _executer.Resources;
            _market = _executer.City.Market;
            ResourceSpend = Container.ResourceSpend;
        }

        public async override UniTask Execute()
        {
            if(_resources.TryGetData(Container.Input, out var tools)){
                var newResources = (tools.Value > Container.Factor) ? Container.Factor : tools.Value;
                var price = (tools.Price * ResourceSpend) / (float) Math.Log(Container.Factor) * _multiplePrice;
                _market.AddProduct(Container.Output, _executer, newResources, Math.Abs(price));
            }
        }
    }
}
