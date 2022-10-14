// using System;
// using System.Collections.Generic;
// using Cysharp.Threading.Tasks;
// using GameData;
// using GameData.Companies;
// using GameData.Resources;
// using Managers;
// using UnityEngine;
// using DirAmbition = Managers.Director.Ambition;
//
// namespace Behaviours.Com
// {
//     public class AmbResourceBuyer : AmbitionRealizator
//     {
//         private List<AmbitionTemplate, DirAmbition> _ambitions;
//         private List<ResourceType, ResourceData> _resources;
//         private ResourceMarket _market;
//
//         public override void Init()
//         {
//             base.Init();
//             _market = _executer.City.Market;
//             _resources = _executer.Resources;
//         }
//
//         public override async UniTask Execute()
//         {
//             var companiesRow = _market.GetSection(AmbitionKey.Input);
//             if(AmbitionKey.Money < 0) return;
//             var deal = companiesRow.BuyByMoney(ambition.Money);
//             if (deal.goods == 0) return;
//             ambition.Money -= deal.passMoney;
//             _ambitions.Set(ambition);
//             _resources.TryGetData(ambition.Input, out var resource);
//             resource.Value += (deal.goods);
//             resource.Price = deal.middlePrice;
//             _resources.Set(resource);
//         }
//     }
// }
