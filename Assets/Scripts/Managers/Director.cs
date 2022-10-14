using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

namespace Managers.Dir
{
    public abstract class Director : Executer
    {
        [SerializeField] private float _money;
        [HideInInspector] public HashSet<LocalCompany>  CompanyCash = new HashSet<LocalCompany>();
        
        public float Money
        {
            get => _money;
            set => _money = value;
        }
    }
}
