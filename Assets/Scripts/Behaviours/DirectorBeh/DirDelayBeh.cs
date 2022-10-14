// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Cysharp.Threading.Tasks;
// using Managers;
// using UnityEngine;
//
// namespace Behaviours.Dir
// {
//     [Serializable]
//     public class DirDelayIncomeBeh : DelayBeh<Director> 
//     {
//         [SerializeField] private int _income;
//
//         public async override UniTask Execute()
//         {
//             _executer.Money += _income;
//             return;
//         }
//     }
//
//     [Serializable]
//     public class DirDecreaseChanceCheapArea : DelayBeh<Director>
//     {
//         public string DirDecrease;
//         public List<DirRandomAreaAndBuildingBeh> Behaviours;
//         public DirDecreaseChanceCheapArea(Director director, List<DirRandomAreaAndBuildingBeh> behs, int delayDayCount) : base(director, delayDayCount)
//         {
//             Behaviours = behs;
//         }
//
//         public async override UniTask Execute()
//         {
//             var i = 0;
//             foreach (var behs in Behaviours)
//             {
//                 i++;
//                 behs.ChanceExpensiveArea--;
//             }
//             return;
//         }
//     }
// }