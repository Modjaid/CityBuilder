using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviours.Com;
using Cysharp.Threading.Tasks;
using GameData;
using Managers;
using PointMap;
using UnityEngine;

namespace Behaviours.Dir
{
    public class DirBuildingCreator : RandomBeh<AIDirector>
    {
        [SerializeField] private List<Building> _projects;

        private CityCalcRequireSpaces _requests;
        private ResourceMarket _market;
        private Map _map;

        private Transform _cityParent;

        public override void Init()
        {
            base.Init();
            _requests = _executer.City.MainCompany.GetComponent<CityCalcRequireSpaces>();
            _map = _executer.City.Map;
            _market = _executer.City.Market;
            _cityParent = new GameObject().transform;
            _cityParent.parent = this.transform;
            _cityParent.name = "building";
            if (_projects.Count == 0)
            {
                _projects = Resources.LoadAll<Building>("Buildings").ToList();
            }
        }

        public async override UniTask Execute()
        {
            var requestList = _requests.Requires.ToList();
            var report = new List<Building, FloatKeyData<Building>>();
            var projects = GetProjectsByMoney(_executer.Money);
            foreach (var request in requestList)
            {
                foreach (var project in projects)
                {
                    if (project.building.Type == request.Key)
                    {
                        var placements = project.building.GetMaxPlacements();
                        if (request.Value < placements)
                        {
                            placements = request.Value;
                        }

                        report.Add(project.building, placements);
                    }
                }
            }

            await UniTask.WaitUntil(() => !_map.IsUpdating);
            if (report.Count > 0)
            {
                var candidate = report.GetMaxValue;
                if (TrySetBuildingToAnyArea(candidate.Key.Size, out var isRotate, out var pos ))
                {
                    var newGo = Build(candidate.Key, isRotate, pos);
                    _executer.Owns.Add(newGo);
                    _executer.City.Add(newGo);
                    GetMarketResources(candidate.Key);
                }
            }

            await UniTask.Yield();
        }


        public float CalcCost(Building building)
        {
            var cost = 0f;
            foreach (var res in building.RequireResources)
            {
                var row = _market.GetSection(res.Key);
                if (row.GetSumCompanies() > res.Value)
                {
                    cost += row.AveragePrice * res.Value;
                }
                else
                {
                    return 99999999f;
                }
            }

            return cost;
        }

        private void GetMarketResources(Building building)
        {
            var cost = 0f;
            foreach (var res in building.RequireResources)
            {
                var row = _market.GetSection(res.Key);
                row.SectionFest();
                var report = row.BuyByCount(res.Value);
                if (report.goods > 0)
                {
                    cost += report.cost;
                }
            }

            if(float.IsNaN(cost) || float.IsInfinity(cost)){
                Debug.LogError($"БЕСКОНЕЧНО В PrePayFoRent");
            }
            _executer.Money -= cost;
        }

        private bool TrySetBuildingToAnyArea(Point size, out bool isRotate, out Vector3 pos)
        {
            var agents = _map.GetSortedAgents(1.IsChance());
            foreach (var agent in agents)
            {
                if (size.IsFitTo(agent.AreaSize, out isRotate))
                {
                    size = isRotate ? size.Inverse() : size;
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
            newBuild.transform.parent = _cityParent;
            
            newBuild.Build(isRotate);
            return newBuild;
        }

        private List<(Building building, float cost)> GetProjectsByMoney(float money)
        {
            var list = new List<(Building, float)>();
            foreach (var project in _projects)
            {
                var cost = CalcCost(project);
                if (cost < money)
                {
                    list.Add((project, cost));
                }
            }

            return list;
        }
    }
}