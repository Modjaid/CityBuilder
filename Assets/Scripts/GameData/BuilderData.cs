using System.Collections;
using System.Collections.Generic;
using System.Linq;
using External;
using Managers;
using PointMap;
using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "BuilderData", menuName = "GameData/BuilderData", order = 1)]
    public class BuilderData : ScriptableObject
    {
        public List<Building> Projects;
        public Transform Parent;
    }
}
