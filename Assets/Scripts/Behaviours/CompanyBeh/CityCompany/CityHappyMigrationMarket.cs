
using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using External;
using GameData;
using GameData.Resources;
using Managers;
using UnityEngine;

namespace Behaviours.Com
{
    ///<Summary>
    ///В зависимости от Happy добавляет жителей и отправляет их в Market бомжевать
    ///</Summary>
    public class CityHappyMigrationMarket : RandomBeh<CitizenCompany>
    {

        private readonly float MaxPotential = 0.1f;
        private List<ResourceType, ResourceData> _cityResources;

        public CityHappyMigrationMarket(CitizenCompany executer, int chance)
        {
            _cityResources = executer.Resources;
        }

        public async override UniTask Execute()
        {
            // var citizens = _cityResources.GetValue(ResourceType.Citizen);
            // var happyScore = _cityResources.GetValue(ResourceType.HappyScore);
            // if (citizens <= 0)
            // {
            //     UnityEngine.Debug.Log($"ВСЕ бомжи ПОКИНУЛИ ГОРОД");
            //     return;
            // }
            //
            // var migrations = happyScore * citizens * MaxPotential;
            // if(migrations < 0){
            //     var allMarketHumans = _executer.City.Market.GetAllHumans();
            //     var percent = (migrations / allMarketHumans) * 100f;
            //     _executer.City.Market.ChangeHumansByPercent(percent);
            // }else{
            //     _executer.City.Market.AddProduct(ResourceType.Citizen, _executer, migrations);
            // }

            return;
        }

    }
}

