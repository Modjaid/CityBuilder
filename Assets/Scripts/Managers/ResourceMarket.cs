using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameData;
using GameData.Companies;
using UnityEngine;
using Random = System.Random;

namespace Managers
{
    public class ResourceMarket : MonoBehaviour
    {
        [SerializeField] private MarketSectionData[] _list;
        
        public float AddProduct(ResourceType type, Company company, float count)
        {
            return _list[(int) type].Add(company, count);
        }
        
        public float AddProduct(ResourceType type, Company company, float count, float newPrice)
        {
            return _list[(int) type].Add(company, count, newPrice);
        }

        public float GetAllHumans()
        {
            var sum = 0f;
            foreach (var humanType in GameData.GameData.Stuff)
            {
                sum += _list[(int) humanType].GetSumCompanies();
            }

            return sum;
        }
        

        public MarketSectionData GetSection(ResourceType type)
        {
            return _list[(int) type];
        }

        public MarketSectionData[] GetDataForLog()
        {
            return _list;
        }

        [ContextMenu("Initialize Sections")]
        public void InitMarketSection(){
            var valuesAsArray = Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToList();
            _list = new MarketSectionData[valuesAsArray.Count];
            for(int i = 0; i < valuesAsArray.Count; i++){
                var newSection = new MarketSectionData();
                newSection.Key = valuesAsArray[i];
                newSection.Info = $"{newSection.Key}";
                _list[i] = newSection;
            }
        }

        [Serializable]
        public class MarketSectionData
        {
            [SerializeField] public string Info;
            [SerializeField] private ResourceType _type;
            [SerializeField] private List<Company, CountPriceData> _companies;
            public ResourceType Key { get => _type; set => _type = value; }

            /// <summary>
            /// 0 - значит товара нет вообще
            /// </summary>
            ///TODO: Обязательно какой то компонент должен подсчитывать, а свойство сделать буфером
            public float AveragePrice {
                get
                {
                    if (_companies.Count == 0)
                    {
                        return 99999999f;
                    }
                    var result = 1f;
                    _companies.Foreach((resData) => result += resData.Price);
                    return result / _companies.Count;
                }
            }
            
            
            public bool IsKey(ResourceType key)
            {
                return _type == key;
            }

            public float GetSumCompanies(){
                return _companies.GetSumValues();
            }

            public (float goods, float cost) BuyByCount(float requireCount)
            {
                var sellers = GetShuffleList();
                (float goods, float cost) result = (0f, 0f);
                var iterator = 0;
                foreach (var seller in sellers)
                {
                   var delivery = Math.Abs(_companies.Add(seller.Key, -requireCount));
                   if(delivery > 0){
                        result.goods += delivery;
                        var benefit = delivery * seller.Price;
                        
                        seller.Key.AddBenefit(benefit);
                        result.cost += benefit;
                        iterator++;
                    }
                }
                return result;
            } 
            
            public (float goods, float passMoney, float middlePrice) BuyByMoney(float money)
            {
                (float goods, float passMoney, float middlePrice) result = (0f, 0f, 0f);
                var sellers = GetShuffleList();
                var iterator = 0;
                foreach (var seller in sellers)
                {
                    var delivery = money / seller.Price;
                    delivery = Math.Abs(_companies.Add(seller.Key, -delivery));
                    if(delivery > 0){
                        result.goods += delivery;
                        var benefit = delivery * seller.Price;
                        result.passMoney += benefit;
                        seller.Key.AddBenefit(benefit);
                        result.middlePrice += seller.Price;
                        money -= result.passMoney;
                        iterator++;
                    }
                }
                result.middlePrice /= iterator;
                return result;
            }

            public float Add(Company key, float money){
               return _companies.Add(key, money);
            }
            
            public float Add(Company key, float money, float newPrice)
            {
                var pass = _companies.Add(key, money);

                if (_companies.TryGetData(key, out var data))
                {
                    data.Price = newPrice;
                }

                return pass;
            }
            /// <summary>
            /// Фестиваль раз в какое то время меняет список компаний для увеличения шанса покупки товаров
            /// </summary>
            /// <param name="isMaxPrice"></param>
            public void SectionFest(){
                _companies.Sort(isMaxValue: true);
            }

            public List<CountPriceData> GetCompanies(){
                return _companies.ToList(); 
            }
            public int Count => _companies.Count;

            public List<CountPriceData> GetShuffleList()
            {
                var list = _companies.ToList();
                list.Sort((a, b) => a.Price.CompareTo(b.Price));
                var sellers = list.ToArray();
                for (int i = 0; i < sellers.Length - 1; i+=2)
                {
                    var chance = sellers[i].Price / sellers[i + 1].Price;
                    var rand = UnityEngine.Random.Range(0f, 2f);
                    if (chance > rand)
                    {
                        var cash = sellers[i];
                        sellers[i] = sellers[i + 1];
                        sellers[i + 1] = cash;
                    }
                }

                return sellers.ToList();
            }
        }
    }
}


// ResourceType.Dollar
// ResourceType.Food
// ResourceType.Fuel
// ResourceType.Policmen
// ResourceType.Criminal
// ResourceType.FireFighter
// ResourceType.Fire
// ResourceType.Military
// ResourceType.Enemy
// ResourceType.GroundTransport
// ResourceType.FlyTrasport
// ResourceType.WaterTransport
// ResourceType.Culture
// ResourceType.Patriotizm
// ResourceType.Electronics
// ResourceType.Logistics
// ResourceType.Worker
// ResourceType.Masters
// ResourceType.Scientist
// ResourceType.Energy
// ResourceType.BuildingMateria
// ResourceType.MassMedia
// ResourceType.Water
// ResourceType.Garbage
// ResourceType.Parasites
// ResourceType.Medicines
// ResourceType.HighTech
