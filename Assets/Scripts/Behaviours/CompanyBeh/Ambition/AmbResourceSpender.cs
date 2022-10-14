using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviours;
using Cysharp.Threading.Tasks;
using GameData;
using GameData.Companies;
using GameData.Resources;
using Managers;
using UnityEngine;

namespace Behaviours.Com
{
    public class AmbResourceSpender : AmbitionRealizator
    {
        public float ResourceSpend;
        private List<AmbitionContainer, AmbitionData> _ambitions;
        private List<ResourceType, ResourceData> _resources;

        public override void Init()
        {
            base.Init();
            _resources = _executer.Resources;
        }

        public async override UniTask Execute()
        {
            if (_resources.TryGetData(Container.Input, out var res))
            {
                var value = (res.Value < 1) ? 1 : res.Value;
                _resources.Add(Container.Input, -ResourceSpend * value);
            }
        }
    }
}