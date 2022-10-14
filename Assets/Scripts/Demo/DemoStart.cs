using System;
using System.Collections;
using System.Collections.Generic;
using Behaviours.Dir;
using Cysharp.Threading.Tasks;
using External;
using GameData;
using Initializator;
using Managers;
using PointMap;
using UnityEngine;
using Random = System.Random;

namespace Demo
{
    public class DemoStart : MonoBehaviour
    {
        [SerializeField] private List<DemoCompanyData> _builders;
        [SerializeField] private MapInit _mapInit;
        [SerializeField] private float Delay;
        private Map _map;
        public void Start()
        {
            StartCoroutine(StartBuilding());
        }

        private IEnumerator StartBuilding()
        {
            _map = _mapInit.Init();
            
            yield return new WaitForSeconds(1);
            // StartCoroutine(Draw());
            while (true)
            {
                yield return new WaitUntil(() => !_map.IsUpdating);
                _builders.Shuffle();
                _builders.Sort((a, b) =>
                {
                    var randA = UnityEngine.Random.Range(0, a.Chance);
                    var randB = UnityEngine.Random.Range(0, b.Chance);
                    return randB.CompareTo(randA);
                });
                var projects = _builders[0].Builder.Projects;
                var candidate = projects[UnityEngine.Random.Range(0, projects.Count)];

                if (TrySetBuildingToAnyArea(candidate, 1.IsChance(), out var isRotate, out var pos))
                {
                    Build(candidate, isRotate, pos);
                }

                yield return new WaitForSeconds(Delay);
            }
        }

        private IEnumerator Draw()
        {
            while (true)
            {
                foreach (var node in _map._nodes)
                {
                    if (node.Value == null)
                    {
                        Debug.DrawRay(node.Key.ToVector(), Vector3.up * 40f, Color.red, 100f, false);
                    }
                }

                yield return null;
            }
        }

        private bool TrySetBuildingToAnyArea(Building building, bool isExpensiveArea, out bool isRotate, out Vector3 pos)
        {
            var agents = _map.GetSortedAgents(isExpensiveArea);
            var log = "";
            foreach (var agent in agents)
            {
                if (building.Size.IsFitTo(agent.AreaSize, out isRotate))
                {
                    var size = isRotate ? building.Size.Inverse() : building.Size;
                    var area = new Points(agent.Area.leftUp, size);
                    pos = Point.Center(size, agent.Area.leftUp);
                    var newAgents = _map.SetNewObstacles(area);
                    _map.UpdateAgentAreas(newAgents);
                    return true;
                }
                
            }

            isRotate = false;
            pos = new Vector3();
            return false;
        }
        
        private Building Build(Building build, bool isRotate, Vector3 pos)
        {
            var newBuild = Instantiate(build.gameObject, pos, Quaternion.identity).GetComponent<Building>();
            newBuild.Build(isRotate);
            return newBuild;
        }
        
        [Serializable]
        public struct DemoCompanyData
        {
            [SerializeField] public string Description;
            [SerializeField] public BuilderData Builder;
            [Header("Шанс активности компании")]
            [Range(0,100)]
            [SerializeField] public int Chance;
        }
    }
    
}
