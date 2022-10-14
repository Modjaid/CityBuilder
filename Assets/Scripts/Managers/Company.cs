using System;
using System.Collections.Generic;
using GameData;
using GameData.Companies;
using GameData.Resources;
using UnityEngine;

namespace Managers
{
    public abstract class Company : Executer
    {
        [SerializeField] public List<ResourceType, ResourceData> Resources = new List<ResourceType, ResourceData>();
        [SerializeField] public List<Company, FloatKeyData<Company>> Tax = new List<Company, FloatKeyData<Company>>();

        public event Action<float> OnPayRent;
        public virtual void AddBenefit(float money)
        {
            Resources.Add(ResourceType.Dollar, money);
        }

         public virtual float GetPayForRent(float mustPay){
            OnPayRent?.Invoke(mustPay);
            return Math.Abs(Resources.Add(ResourceType.Dollar, -mustPay));
        }

         public abstract SpaceData GetSpaceData(Building building);

         public abstract List<Building, SpaceData> GetAllSpaces();
         

         protected override void OnUpdateLog()
        {
            Log += $"Ресурсы:\n";
            foreach (var kv in Resources.ToList())
            {
                if(kv.Value == 0) continue;
                Log += $"{kv.Key.ToString()} : кол-во:{kv.Value} цена:{kv.Price}\n";
            }

            Log += $"Налоги:\n";
            foreach (var kv in Tax.ToList())
            {
                Log += $"{kv.Key.gameObject.name} налог:{kv.Value}\n";
            }
            base.OnUpdateLog();
        }

    }
}
