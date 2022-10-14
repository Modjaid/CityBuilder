// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using Cysharp.Threading.Tasks;
// using GameData;
// using Managers;
// using UnityEngine;
// using static Managers.Director;
//
// namespace Behaviours.Com
// {
//     public class ComAmbitionManager : DelayBeh<LocalCompany>
//     {
//         public override void Init()
//         {
//             base.Init();
//             _ambitions = _executer.Ambitions;
//         }
//         public async override UniTask Execute()
//         {
//             var ambitionContainers = transform.GetComponentsInChildren<AmbitionTemplate>();
//             foreach (var container in ambitionContainers)
//             {
//                 if (!_ambitions.Contains(container.Key))
//                 {
//                     RemoveAmbition(container);
//                 }
//             }
//             _ambitions.Foreach((amb) =>
//             {
//                 if (!ambitionContainers.Any((container) => container.Key == amb.Key)){
//                     var ambitionTemplate = Instantiate(amb.Key, this.transform);
//                     ambitionTemplate.Key = amb.Key;
//                 }
//             });
//         }
//         private void RemoveAmbition(AmbitionTemplate container)
//         {
//             var SendProductsToMarket = _ambitions.ToList().All((amb) => amb.Input != container.Input);
//
//             if (SendProductsToMarket && _executer.Resources.TryGetData(container.Input, out var resources))
//             {
//                 _executer.Resources.Add(container.Input, -resources.Value);
//                 _executer.City.Market.AddProduct(container.Input, _executer, resources.Value, resources.Price);
//             }
//             Destroy(container.gameObject);
//         }
//
//         private void OnDisable(){
//             base.OnDisable();
//             Execute();
//         }
//     }
//
// }
