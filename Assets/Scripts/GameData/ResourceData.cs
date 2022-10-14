using System;

using UnityEngine;

namespace GameData.Resources
{
    [Serializable]
    public struct ResourceData : IKeyValue<ResourceType>
    {
        [SerializeField] private ResourceType _type;
        [SerializeField] private float _currentCount;
        [SerializeField] private float _price;
        public ResourceType Key
        {
            get => _type;
            set => _type = value;
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

        public bool IsKey(ResourceType key)
        {
            return _type == key;
        }

        public float Add(float value)
        {
            if (value == 0) return 0;

            float sum = Value + value;
            if (sum <= 0)
            {
                Value = 0;
                return value - sum;
            }

            Value = sum;
            return value;
        }
    }

    [Serializable]
    public class PrioritetData : IKeyValue<ResourceType>
    {
        [SerializeField] private ResourceType _type;
        [SerializeField] private float _multiTarget;
        [SerializeField] public float Cash;
        [SerializeField] public float Prioritet;
        public ResourceType Key
        {
            get => _type;
            set => _type = value;
        }

        public float Value
        {
            get => _multiTarget;
            set => _multiTarget = value;
        }

        public bool IsKey(ResourceType key) => _type == key;
    
    }
}
