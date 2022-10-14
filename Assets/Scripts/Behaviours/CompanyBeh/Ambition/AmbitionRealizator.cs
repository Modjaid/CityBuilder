using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameData;
using Managers;
using UnityEngine;

namespace Behaviours.Com{
    public abstract class AmbitionRealizator : DelayBeh<LocalCompany>
    {
        public AmbitionContainer Container { get; set; }

        public override void Init()
        {
            Container = GetComponent<AmbitionContainer>();
            _executer = GetComponentInParent<LocalCompany>();
        }
    }

}
