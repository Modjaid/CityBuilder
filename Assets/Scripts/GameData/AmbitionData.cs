using System;
using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

namespace GameData
{
    

    /// <summary>
    /// Амбиция с фактором вывода на CurrentCount
    /// </summary>
    [Serializable]
    public struct AmbitionData : IKeyValue<AmbitionContainer>
    {
        [SerializeField] private AmbitionContainer _ambitionPattern;

        [Tooltip("Компания будет умножать вывод ресурсов")] [SerializeField]
        private float _factor;

        [Tooltip("Компания будет затрачивать в своем буфере ресурсов")] [SerializeField]
        private float _resourceSpend;

        [Tooltip("Выгорание амбиции, будет вычитать с factor, _count")] [SerializeField]
        private float _factorSpend;

        [Tooltip("Инвестиции в амбицию")] [SerializeField]
        private float _money;



        public AmbitionContainer Key
        {
            get => _ambitionPattern;
            set => _ambitionPattern = value;
        }

        public float Value
        {
            get => _factor;
            set { _factor = value; }
        }

        public ResourceType Input => _ambitionPattern.Input;
        public ResourceType Output => _ambitionPattern.Output;
        public BuildingType WorkPlace => _ambitionPattern.WorkPlace;

        public bool IsHide => _ambitionPattern.IsHide;
        public string Description => _ambitionPattern.Description;

        public float Factor
        {
            get => _factor;
            set => _factor = value;
        }

        public float ResourceSpend
        {
            get => _resourceSpend;
            set => _resourceSpend = value;
        }

        public float FactorSpend
        {
            get => _factorSpend;
            set => _factorSpend = value;
        }

        public float Money
        {
            get => _money;
            set => _money = value;
        }

        public bool IsKey(AmbitionContainer key)
        {
            return _ambitionPattern == key;
        }

        public static AmbitionData operator +(AmbitionData a, AmbitionData b)
        {
            AmbitionData result = new AmbitionData();
            result.Key = a.Key;
            result.Money = a.Money + b.Money;
            result.Factor = a.Factor + b.Factor;
            result.FactorSpend = b._factorSpend;
            result.ResourceSpend = b.ResourceSpend;
            return result;
        }
    }
}
