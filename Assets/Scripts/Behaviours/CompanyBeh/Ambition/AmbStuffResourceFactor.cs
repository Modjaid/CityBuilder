using System;
using System.Collections;
using System.Collections.Generic;
using Behaviours;
using Cysharp.Threading.Tasks;
using GameData;
using GameData.Companies;
using GameData.Resources;
using Managers;
using UnityEngine;

namespace Behaviours.Com
{
    public class AmbStuffResourceFactor : AmbitionRealizator
    {
        [SerializeField] private float _multiplePrice = 40f;
        private ResourceMarket _market;
        private List<ResourceType, ResourceData> _resources;
        private List<Building, SpaceData> _spaces;
        private ResourceType _factorType;
        private float ResourceSpend = 0.1f;
        public override void Init()
        {
            base.Init();
            _resources = _executer.Resources;
            _market = _executer.City.Market;
            _spaces = GetComponent<AmbStuffManager>().Spaces;
            _factorType = Container.Input;
            ResourceSpend = Container.ResourceSpend;
        }

        public async override UniTask Execute()
        {

            if (_resources.TryGetData(_factorType, out var stuff))
            {
                var newResources = 0f;
                var factor = CalcFactor(stuff.Value, Container.Factor);
                var stuffResBonus = stuff.Value * factor;
                var softResBonus = stuff.Value / 4;
                newResources = stuffResBonus + softResBonus;
                var price = (stuff.Price * ResourceSpend) / (float) Math.Log(Container.Factor) *
                            _multiplePrice;
                if (price == 0)
                {
                    Debug.LogError($"По какой то причине Price нулевой");
                }

                _market.AddProduct(Container.Output, _executer, newResources, Math.Abs(price));
            }
        }

        public float CalcFactor(float stuffCount, float ambFactor)
        {
            var factorX = 1f;
            if (stuffCount < ambFactor / 3)
            {
                factorX = 4;
            }
            else if (stuffCount < ambFactor / 2)
            {
                factorX = 3;
            }
            else if (stuffCount < ambFactor)
            {
                factorX = 2;
            }
            return factorX;
        }
    }
}
