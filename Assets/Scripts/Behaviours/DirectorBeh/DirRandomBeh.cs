// using System.Collections;
// using Cysharp.Threading.Tasks;
// using External;
// using GameData;
// using Managers;
// using UnityEngine;
//
// namespace Behaviours.Dir
// {
//     /// <summary>
//     /// Шанс выпадет либо на дорогой район либо на дешевый ===>>>
//     /// 
//     /// Идет по миксеру всех зданий в попытке вставить в выйгрышный район ===>>>
//     ///
//     /// Если промах упускает шанс
//     /// </summary>
//     public class DirRandomAreaAndBuildingBeh : BuildBeh<Director>
//     {
//         public int ChanceExpensiveArea;
//         public DirRandomAreaAndBuildingBeh(Director director, BuilderData builder, int chance, int chanceCheapArea)
//         {
//             ChanceExpensiveArea = chanceCheapArea;
//         }
//         public async override UniTask Execute()
//         {
//             if (Builder.IsLock)
//             {
//                 return;
//             }
//             var projects = Builder.GetProjectsByMoney(_executer.Money);
//             
//             if(projects.Count == 0) return;
//             
//             projects.Shuffle();
//
//             bool expensiveOrCheapArea = (ChanceExpensiveArea <= Random.Range(0, 100));
//             foreach (var project in projects)
//             {
//                 if (Builder.TrySetBuildingToAnyArea(project, expensiveOrCheapArea))
//                 {
//                     return;
//                 }
//             }
//             Debug.Log($"MISS projects:{projects.Count}");
//             return;
//         }
//     }
// }