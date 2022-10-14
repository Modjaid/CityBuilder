//
// using System.Collections.Generic;
// using Behaviours.Dir;
// using Cysharp.Threading.Tasks;
// using External;
// using GameData;
// using GameData.Companies;
// using GameData.Resources;
// using Managers;
// using UnityEngine;
// using UnityEngine.Serialization;
//
// namespace Behaviours.Com
// {
//     public class ComSpaceManager : DelayBeh<LocalCompany>
//     {
//         public float TargetSpace;
//         private List<ResourceType, ResourceData> _resources;
//         private List<Building, SpaceData> _spaces;
//
//         public override void Init()
//         {
//             base.Init();
//             _resources = _executer.Resources;
//             // _spaces = _executer.Spaces;
//         }
//
//         public async override UniTask Execute()
//         {
//             var allStuff = _resources.GetSumStuff();
//             var placements = _spaces.GetSumValues();
//             placements = (placements == 0) ? 1 : placements;
//             var currentFillSpace = placements / allStuff;
//             
//             
//             
//             if (currentFillSpace < TargetSpace)
//             {
//                 placements = (int) ((allStuff - placements) * TargetSpace);
//                 if (!_executer.Ambitions.TryGetByIndex(0, out var amb))
//                 {
//                     return;
//                 }
//                 var citySpaces = _executer.City.GetBuildings(amb.WorkPlace);
//                 citySpaces.Shuffle();
//                 foreach (var citySpace in citySpaces)
//                 {
//                     if (GetPermision(citySpace.model, citySpace.dir))
//                     {
//                         placements -= UpdSpace(citySpace.model, placements);
//                     }
//                 }
//             }
//             else if(currentFillSpace > TargetSpace)
//             {
//                 placements = (int) ((allStuff * TargetSpace) - placements);
//                 foreach (var spacesKey in _spaces.Keys)
//                 {
//                     placements -= UpdSpace(spacesKey, placements);
//                 }
//             }
//         }
//
//         private float UpdSpace(Building space,float newPlacements)
//         {
//             var offset = space.ChangePlacements(_executer, newPlacements);
//             return _spaces.Add(space, offset);
//         }
//         
//
//         private bool GetPermision(Building space, Director director)
//         {
//             if (director.TryGetComponent<DirRentCollector>(out var collector))
//             {
//                 var companyDebt = collector.GetDebt(_executer);
//                 var companyMoney = _resources.GetValue(ResourceType.Dollar);
//                 if (companyDebt > 0)
//                 {
//                     Debug.Log(
//                         $"Компания ищет новые офисы у тех кому должна с успехом:{companyMoney > companyDebt}");
//                     if (companyMoney < companyDebt)
//                     {
//                         return false;
//                     }
//         
//                     collector.PrePayFoRent(_executer, space);
//                 }
//             }
//
//             return true;
//         }
//         public async void OnDisable()
//         {
//             await Execute();
//             base.OnDisable();
//         }
//     }
// }
