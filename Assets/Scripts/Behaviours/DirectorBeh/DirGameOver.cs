using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameData;
using GameData.Resources;
using Managers;
using UnityEngine;

namespace Behaviours.Dir
{
    public class DirGameOver : DelayBeh<AIDirector>
    {
        [SerializeField] private int Health;
        public async override UniTask Execute()
        {
            if (_executer.Owns.Count > 0)
            {
                return;
            }

            if (_executer.Money > 1000)
            {
                return;
            }
            Health--;
            if (Health < 1)
            {
                var list = new List<LocalCompany>(_executer.CompanyCash);
                foreach (var company in list)
                {
                    company.PassAllStocks(_executer);
                }
                Debug.LogError($"GAME Over for {_executer.gameObject.name}");
                Destroy(this.gameObject);
                await UniTask.Yield();
                return;
            }
        }
    }
}
