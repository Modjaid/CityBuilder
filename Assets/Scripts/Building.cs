using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameData;
using PointMap;
using UnityEngine;
using UnityEngine.Serialization;
using Game = GameData.GameData;

namespace Managers
{
    [SelectionBase]
    public class Building : MonoBehaviour
    {
        [TextArea(0, 1000)] [SerializeField] private string _log;

        [Header("Вместимость людей (всех типов)")] [SerializeField]
        private int _space;

        [SerializeField] private BuildingType _type;

        [Header("Обязательные для строительства ресурсы")] [SerializeField]
        private FloatKeyData<ResourceType>[] _requireResources;
        [HideInInspector] public BuildingType Type => _type;
        [HideInInspector] public Point Size => GetComponent<BuildingMesh>().Size;
        
        [HideInInspector] private List<Company, FloatKeyData<Company>> _bookingSpace;
        private BuildingMesh _mesh;
        public List<FloatKeyData<Company>> GetMembers()
        {
            return _bookingSpace.ToList();
        }

        public FloatKeyData<ResourceType>[] RequireResources => _requireResources;
        
        public void Awake()
        {
            _mesh = GetComponent<BuildingMesh>();
            _bookingSpace = new List<Company, FloatKeyData<Company>>();
            StartCoroutine(DebugLog());
        }

        public void Build(bool isRotate)
        {
            if (isRotate)
            {
                _mesh.Rotate90();
            }

            _mesh.Grow();
        }
        

        public float ChangePlacements(Company company, float newPlaces)
        {
            var bookingPlaces = _bookingSpace.GetSumValues();
            var sum = bookingPlaces + newPlaces;
            if (sum >= _space)
            {
                sum = _space - bookingPlaces;
                return _bookingSpace.Add(company, sum);
            }

            if (sum < 0)
            {
                return _bookingSpace.Add(company, sum);
            }

            return _bookingSpace.Add(company, newPlaces);
        }
        public bool IsSuitType(ResourceType resType){
            var suitTypes = GameData.GameData.BuildingPlacements[(int) _type];
            return suitTypes.Contains(resType);
        }

        public float ClearCompany(Company company)
        {
            var placements = _bookingSpace.GetValue(company);
            _bookingSpace.Clear(company);
            return placements;
        }

        public float GetPlacements(Company company)
        {
            return _bookingSpace.GetValue(company);
        }

        public float GetFreePlacements()
        {
            return _bookingSpace.GetSumValues();
        }

        public float GetMaxPlacements()
        {
            return _space;
        }

        public float GetResidents(Company company)
        {
            return _bookingSpace.GetValue(company);
        }

        public float GetResidents()
        {
            return _bookingSpace.GetSumValues();
        }
        

        private IEnumerator DebugLog()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                _log = $"bookingPlacements = {GetResidents()}\n";
                foreach (var kv in _bookingSpace.ToList())
                {
                    _log += $"{kv.Key.ToString()} : {kv.Value}\n";
                }
            }
        }


    }
}

