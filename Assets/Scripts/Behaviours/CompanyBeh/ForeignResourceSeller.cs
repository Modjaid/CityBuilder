using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameData;
using Managers;
using Managers.Com;
using UnityEngine;

namespace Behaviours.Com{
    public class ForeignResourceSeller : DelayBeh<ForeignCompany>
    {
        private int[] _costs;
        public ForeignResourceSeller(ForeignCompany executer, int delayDayCount)
        {
            // _costs = executer.Costs;
        }

        public async override UniTask Execute()
        {
            return;
            // var citizens = _executer.City.Resources.GetValue(ResourceType.Citizen);
        }
    }
}
