using System;
using Managers;
using Managers.Com;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameData.Companies
{
    [Serializable]
    public class CountPriceData : IKeyValue<Company>
    {
        [SerializeField] private Company _company;
        [SerializeField] private float _currentCount;
        [SerializeField] private float _price;

        public Company Key
        {
            get => _company;
            set => _company = value;
        }

        public float Value
        {
            get => _currentCount;
            set => _currentCount = value;
        }

        public float Price
        {
            get => _price;
            set => _price = value;
        }

        public bool IsKey(Company key)
        {
            return _company == key;
        }
    }

    [Serializable]
    public struct SpaceData : IKeyValue<Building>
    {
        [SerializeField] private Building _building;
        [SerializeField] private float _space;

        public Building Key { get => _building; set => _building = value; }
        /// <summary>
        /// Общее пространство
        /// </summary>
        public float Value { get => _space; set => _space = value; }
        

        public bool IsKey(Building key) => Key == key;
    }
    
    [Serializable]
    public struct RequireSpaceData : IKeyValue<BuildingType>
    {
        [SerializeField] private BuildingType _resType;
        [SerializeField] private float _requireStuff;
        public BuildingType Key
        {
            get => _resType;
            set => _resType = value;
        }
        public float Value
        {
            get => _requireStuff;
            set => _requireStuff = value;
        }
        public bool IsKey(BuildingType key) => key == _resType;

        // public static RequireSpaceData operator +(RequireSpaceData a, RequireSpaceData b)
        // {
        //     var spacesB = b.Resources.ToList();
        //     foreach (var space in spacesB)
        //     {
        //         a.Resources.Add(space);
        //     }
        //
        //     a.Value = a.Resources.GetSumValues();
        //     return a;
        // }
    }
}
