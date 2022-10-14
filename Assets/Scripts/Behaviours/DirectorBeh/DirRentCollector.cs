using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameData;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Behaviours.Dir
{
    public class DirRentCollector : DelayBeh<AIDirector>
    {
        [Tooltip("Плата за одно место")]
        [SerializeField] private float _rentCost = 3f;

        [SerializeField] private List<Company, FloatKeyData<Company>> _debtors;
        private List<Building> _owns;
        public override void Init()
        {
            base.Init();
            _owns = _executer.Owns;
        }

        public async override UniTask Execute()
        {
            foreach (var building in _owns)
            {
                var arendators = building.GetMembers();
                foreach (var arendator in arendators)
                {
                    var spaceData = arendator.Key.GetSpaceData(building);

                    var mustPay = spaceData.Value * _rentCost;
                    var truePay = arendator.Key.GetPayForRent(mustPay);
                    _executer.Money += truePay;
                    if (mustPay > truePay)
                    {
                        _debtors.Add(arendator.Key, -truePay);
                        continue;
                    }

                    mustPay = _debtors.GetValue(arendator.Key);
                    truePay = arendator.Key.GetPayForRent(mustPay);
                    _executer.Money += truePay;
                    _debtors.Add(arendator.Key, -truePay);
                }
            }
        }

        public void Pay(Company company, float money)
        {
            _debtors.Add(company, -money);
        }

        public float GetDebt(Company company) => _debtors.GetValue(company);

        public void PrePayFoRent(Company arendator, Building building)
        {
            var spaceData = arendator.GetSpaceData(building);
            var mustPay = (spaceData.Value * _rentCost) + _debtors.GetValue(arendator);
            var truePay = arendator.GetPayForRent(mustPay);
            _debtors.Add(arendator, -truePay);
            _executer.Money += truePay;
        }
    }
}
