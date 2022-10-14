using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Behaviours;
using Behaviours.Com;
using Cysharp.Threading.Tasks;
using GameData;
using GameData.Companies;
using Managers.Dir;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class  
        LocalCompany : Company
    {

        [SerializeField]
        private List<Director, FloatKeyData<Director>> _sovietMembers = new List<Director, FloatKeyData<Director>>();

        [SerializeField] public float AmbitionAcceptRange = 70f;
        protected DirectorSoviet _soviet;
        public float AverageInvestorsMoney => _soviet.AverageMonetaryValue;

        /// <summary>
        /// Считает стоимость всей компании (Heavy Property)
        /// </summary>
        public float Turnover
        {
            get
            {
                var sum = 1000f;
                // Ambitions.Foreach((ambition) => sum += ambition.Money);
                Resources.Foreach((resource) => sum += resource.Value * resource.Price);
                return sum;
            }
        }

        public List<Director, FloatKeyData<Director>> Members => _sovietMembers;

        private int _spaceArea = 0;
        public int SpaceArea
        {
            get
            {
                var area = 0f;
                foreach (Transform child in transform)
                {
                    if (child.TryGetComponent<AmbStuffManager>(out var stuffManager))
                    {
                        area = stuffManager.Spaces.GetSumValues();
                    }
                }

                return (int) area;
            }
        }
        
        public ResourceType Output
        {
            get
            {
                if (transform.TryGetAmbition<AmbitionContainer>(out var ambition))
                {
                    return ambition.Output;
                }

                return ResourceType.Citizen;
            }
        }

        public DirectorSoviet Soviet => _soviet;

        public void SetMainDirector(Director director)
        {
            _sovietMembers.Add(director, 100f);
        }
        public override void Init()
        {
            base.Init();
            _soviet = new DirectorSoviet(_sovietMembers, this);
        }

        public override void AddBenefit(float money)
        {

            if (float.IsNaN(money) || float.IsInfinity(money))
            {
                Debug.LogError($"БЕСКОНЕЧНО money:{money}");
            }
            else
            {
                _soviet.SplitBenefit(money);
            }
        }

        public void PassAllStocks(AIDirector passer)
        {
            var passPercent = _soviet.GetStockPercen(passer);
            var members = _soviet.Directors;
            members.Remove(passer);
            if (members.Count > 0)
            {
                var deltaPercent = passPercent / members.Count;
                foreach (var member in members)
                {
                    _soviet.BuyNewStocks(member, passer, deltaPercent);
                }
            }
            else
            {
                BANCROT();
            }
        }

        public void BANCROT(){
            var newName = $"Bancrot_{gameObject.name}";
            gameObject.name = newName;
            AmbitionAcceptRange = 100f;
            AddBenefit(Resources.Clear(ResourceType.Dollar));
            Resources.Foreach((res) => City.Market.AddProduct(res.Key, this, res.Value, res.Price / 2));
            foreach (var ambition in GetComponentsInChildren<AmbitionContainer>())
            {
                Destroy(ambition.gameObject);
            }
            foreach(var beh in GetComponents<StepBehaviour>()){
                Destroy(beh);
            }
        }

        private void AddAmbition(AmbitionData ambData)
        {
            AmbitionContainer container;
            if(!transform.TryGetAmbition(ambData.Key, out container))
            {
                container = Instantiate(ambData.Key, this.transform);
                container.Key = ambData.Key;
            }
            container.Factor += ambData.Factor;
            container.Money += ambData.Money;
            container.ResourceSpend = ambData.ResourceSpend;
            container.AmbitionSpend = ambData.FactorSpend;

        }

        public void AddEasyAmbition(AIDirector candidate, AmbitionData newAmbition)
        {
            AddAmbition(newAmbition);
            if (newAmbition.Money > 0)
            {
                _soviet.Investment(candidate, newAmbition.Money);
                Resources.Add(ResourceType.Dollar, newAmbition.Money);
                _soviet.FindMainDirector();
            }
        }
        /// <param name="candidate">Совет директоров смотрит на свое отношение к директору, на его акции</param>
        /// <param name="newAmbition">Значения добавочной амбиции суммируются поверх амбиции в компании</param>
         /// <returns>-1 = быстрый отказ, 0 = ждать ответа игрока, 1 = Быстрое согласие</returns>
        public int ReviewAmbitionWithNewCandidate(AIDirector candidate, AmbitionData newAmbition)
        {
            if (newAmbition.Money <= 0)
            {
                return -1;
            }
            if (transform.TryGetAmbition<AmbitionContainer>(out var ambition) && newAmbition.Money > 0)
            {
                if (ambition.Output == newAmbition.Output)
                {
                   return _soviet.ReviewAmbitionWithNewCandidate(newAmbition, candidate);
                }

                return -1;
            }

           return _soviet.ReviewAmbitionWithNewCandidate(newAmbition, candidate);
        }
        
        public bool TryAddAmbition(AIDirector candidate, AmbitionData newAmbition)
        {
            if (transform.TryGetAmbition<AmbitionContainer>(out var ambition))
            {
                if (_soviet.IsMember(candidate) && ambition.Output == newAmbition.Output)
                {
                    AddAmbition(newAmbition);
                    if (newAmbition.Money > 0)
                    {
                        _soviet.Investment(candidate, newAmbition.Money);
                        Resources.Add(ResourceType.Dollar, newAmbition.Money);
                        _soviet.FindMainDirector();
                    }
                    return true;
                }
            }

            return false;
        }
        
        /// <param name="candidate">Совет директоров смотрит на свое отношение к директору, на его акции</param>
        /// <param name="newAmbition">Значения добавочной амбиции суммируются поверх амбиции в компании</param>
        /// <returns>true - быстрый ответ, иначе ждем игрока</returns>
        // public bool ReviewAmbition(Player candidate, float investments)
        // {
        //     if (transform.TryGetAmbition<AmbitionContainer>(out var ambition))
        //     {
        //         if (ambition.Output == newAmbition.Output)
        //         {
        //             _soviet.ReviewAmbition(newAmbition, candidate);
        //             return;
        //         }
        //
        //         return;
        //     }
        //
        //     _soviet.ReviewAmbition(newAmbition, candidate);
        // }

        public override SpaceData GetSpaceData(Building building)
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<AmbStuffManager>(out var stuffManager))
                {
                    if (stuffManager.Spaces.TryGetData(building, out var spaceData))
                    {
                        return spaceData;
                    }
                }
            }
            
            throw new NullReferenceException();
        }

        public override List<Building, SpaceData> GetAllSpaces()
        {
            var allSpaces = new List<Building, SpaceData>();
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<AmbStuffManager>(out var stuffManager))
                {
                    stuffManager.Spaces.Foreach((space) => allSpaces.Add(space.Key, space.Value));
                }
            }

            return allSpaces;
        }


        protected override void OnUpdateLog()
        {
            var ambitions = transform.GetAmbitions();
            if (ambitions.Count > 0)
            {
                Log += $"Цель компании: {ambitions[0].Output.ToString()}\n";
            }

            // Log += $"Оффисы:\n";
            // foreach (var building in Spaces.ToList())
            // {
            //     Log +=
            //         $"Здание({building.Key.name}) площадью:{building.Key.GetFreePlacements()}/{building.Key.GetMaxPlacements()} заселено компанией:{building.Key.GetResidents(this)}/" +
            //         $"{building.Key.GetPlacements(this)}\n";
            // }
            Log += $"Директора: главный -{_soviet.MainDirector.gameObject.name}\n";
            foreach (var dir in _sovietMembers.ToList())
            {
                Log += $"{dir.Key.gameObject.name} = {dir.Value} акций\n";
            }
            base.OnUpdateLog();

        }
    }
}