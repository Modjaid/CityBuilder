// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using Cysharp.Threading.Tasks;
// using External;
// using GameData;
// using GameData.Resources;
// using Managers;
// using UnityEngine;
// using static Managers.ResourceMarket;
//
// namespace Behaviours.Com
// {
//     ///<Summary>
//     /// Сборщик денег с жителей
//     ///</Summary>
//     public class RentCitizenCollector : DelayBeh<LocalCompany>
//     {
//         public float SolventBall;
//         public const float MaxSolventBall = 200;
//         private List<ResourceType, ResourceData> _cityResources;
//         private List<ResourceType, ResourceData> _ownResources;
//         private List<Building> _ownHouses;
//         private MarketSectionData _citizensMarket;
//
//         public RentCitizenCollector(LocalCompany executer, int delayDayCount)
//         {
//             _cityResources = _executer.City.MainCompany.Resources;
//             _ownResources = _executer.Resources;
//             // _ownHouses = _executer.GetOffices().Keys;
//             _citizensMarket = _executer.City.Market.GetSection(ResourceType.Citizen);
//         }
//
//         public async override UniTask Execute()
//         {
//             var payments = _ownHouses.GetSumResidentsByCompany(_executer);
//             var rentSum = payments * _ownResources.GetData(ResourceType.Citizen).Price;
//             var benefit = Math.Abs(_cityResources.Add(ResourceType.Dollar, -rentSum));
//
//
//             if (benefit < rentSum)
//             {
//                 SolventBall -= ((rentSum - benefit) / rentSum) * 100f;
//                 if (SolventBall < 0)
//                 {
//                     var percent = SolventBall * 0.1f;
//                     foreach (var house in _ownHouses)
//                     {
//                         var citizens = house.GetResidents(_executer);
//                         var delta = (int) Math.Round((citizens / 100) * percent);
//                         var result = house.ChangeResidents(_executer, delta);
//                         Debug.Log(
//                             $"Жители перестают платить пора бы их уже выселять {this._executer}");
//                         _ownResources.Add(ResourceType.Citizen, result);
//                         _cityResources.Add(ResourceType.Citizen, result);
//                         _executer.City.Market.AddProduct(ResourceType.Citizen, _executer.City.MainCompany,
//                             Math.Abs(result));
//                     }
//                 }
//             }
//             else
//             {
//                 SolventBall += 100;
//                 if (SolventBall > MaxSolventBall)
//                 {
//                     SolventBall = MaxSolventBall;
//                 }
//
//                 if (SolventBall >= 0)
//                 {
//                     // var currentCitizens = _ownResources.Get(ResourceType.Citizen).Value;
//                     // if (_citizenPlan > currentCitizens)
//                     // {
//                     //     var targetHireCitizens =
//                     //         (_citizenPlan - currentCitizens) * (SolventBall / MaxSolventBall);
//                     //     targetHireCitizens = (int) Math.Round(targetHireCitizens);
//
//                         // _citizensMarket.ChangeValuesByOrder(
//                         //     -targetHireCitizens,
//                         //     (company, fromMarketPersons) =>
//                         //     {
//                         //         var restHires = Math.Abs(fromMarketPersons);
//                         //         foreach (var house in _ownHouses)
//                         //         {
//                         //              var succesfullHires = house.ChangeResidents(_executer, restHires);
//                         //              restHires -= succesfullHires;
//                         //             _ownResources.Add(ResourceType.Citizen, succesfullHires);
//                         //             _cityResources.Add(ResourceType.Citizen, succesfullHires);
//                         //             _cityResources.Add(ResourceType.Dollar, 
//                         //                 succesfullHires * company.Prices.GetValue(ResourceType.Citizen));
//                         //             if (restHires <= 0)
//                         //             {
//                         //                 break;
//                         //             }
//                         //         }
//                         //         _citizensMarket.AddValue(company, restHires);
//                         //     },
//                         //     (company1, company2) =>
//                         //         company1.key.CompareByResourcePrice(company2.key, ResourceType.Citizen)
//                         // );
//                     // }
//                 }
//             }
//
//             _ownResources.Add(ResourceType.Dollar, benefit);
//             return;
//         }
//     }
// }
