using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using GameData;
using Managers;
using UnityEngine;

namespace Behaviours.Com{
    public class RentCompanyCollector : DelayBeh<Executer>
    {
        // public RentCompanyCollector(Executer executer, int delayDayCount) : base(executer, delayDayCount)
        // {
        //     
        // }

        public async override UniTask Execute()
        {
            // var owns = _executer.Owns.ToList();
            // foreach(var building in owns){
            //     var buildingRentCost = _executer.Owns.GetValue(building.Key);
            //     var members = building.Key.GetMembers();
            //     foreach(var member in members){
            //         var paySum = buildingRentCost * member.Value;
            //         var benefit = Math.Abs(member.Key.Resources.Add(ResourceType.Dollar, -paySum));
            //         // _executer.Money += benefit;
            //         if(benefit < paySum){
            //             Debug.Log($"Собственник: {_executer.Name} выселяет штат компании {member.Key.Name} из {building.Key}");
            //             member.Key.SendHumansFromBuildingToMarket(building.Key);
            //         }
            //     }
            // }

            return;
        }
    }

}
