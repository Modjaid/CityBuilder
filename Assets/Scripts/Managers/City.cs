using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviours.Com;
using GameData;
using Initializator;
using UnityEngine;
// using Behaviour = Behaviours.StepBehaviour<Managers.Executer>;
using Map = PointMap.Map;

namespace Managers
{
    public class City : MonoBehaviour
    {
        public event Action<bool, Building> OnChangeBuildings;
        public event Action<bool, Executer> OnChangeExecutors;

        [HideInInspector] [SerializeField]
        private List<Executer> _executors;
        [HideInInspector] [SerializeField]
        private List<Building> _buildings;
        
        
        [SerializeField] public ResourceMarket Market;
        [SerializeField] private MapInit _mapInit;

        [HideInInspector] public CitizenCompany MainCompany;
        [HideInInspector] public Player Player;
        [HideInInspector] public Map Map;

        public void Init()
        {
            Map = _mapInit.Init();
        }

        public List<(AIDirector dir,Building model)> GetBuildings()
        {
            var result = new List<(AIDirector dir, Building building)>();
            var directors = FindExecuters<AIDirector>();
            foreach (var dir in directors)         
            {
                foreach (var building in dir.Owns)
                {
                    result.Add((dir, building));
                }
            }

            return result;
        }

        public List<(AIDirector dir,Building model)> GetBuildings(BuildingType type)
        {
            var result = new List<(AIDirector dir, Building building)>();
            var directors = FindExecuters<AIDirector>();
            foreach (var dir in directors)         
            {
                foreach (var building in dir.Owns)
                {
                    if (building.Type == type)
                    {
                        result.Add((dir, building));
                    }
                }
            }

            return result;
        }
        public List<(AIDirector dir,Building model)> GetBuildings(ResourceType type)
        {
            var result = new List<(AIDirector dir, Building building)>();
            var directors = FindExecuters<AIDirector>();
            foreach (var dir in directors)         
            {
                foreach (var building in dir.Owns)
                {
                    if (building.IsSuitType(type))
                    {
                        result.Add((dir, building));
                    }
                }
            }

            return result;
        }

        public void Awake()
        {
            Init();
            StartCoroutine(DebugResources());
        }
        public void Add(Building building)
        {
            _buildings.Add(building);
            OnChangeBuildings?.Invoke(true, building);
        }

        public void Remove(Building building)
        {
            _buildings.Remove(building);
            OnChangeBuildings?.Invoke(false, building);
        }

        public void Add(Executer exe)
        {
            _executors.Add(exe);
            OnChangeExecutors?.Invoke(true, exe);
        }

        public void Remove(Executer exe)
        {
            _executors.Remove(exe);
            OnChangeExecutors?.Invoke(false, exe);
        }


        // public LocalCompany[] GetlandLords()
        // {
        //     return FindExecutersByBeh<LocalCompany, RentCitizenCollector>().ToArray();
        // }

        public T FindExecuter<T>() where T : Executer
        {
            T executer = null;
            foreach (var exe in _executors)
            {
                if (exe is T)
                {
                    executer = exe as T;
                }
            }

            return executer;
        }

        public T[] FindExecuters<T>() where T : Executer
        {
            var list = new List<T>();
            foreach (var exe in _executors)
            {
                if (exe is T)
                {
                    list.Add(exe as T);
                }
            }

            return list.ToArray();
        }

        // private E FindExecuterByBeh<E, B>() where E : Executer where B : Behaviour
        // {
        //     foreach (var exe in Executors)
        //     {
        //         if (exe is E)
        //         {
        //             if (exe.GetBehaviour<B>() != null)
        //             {
        //                 return exe as E;
        //             }
        //         }
        //     }

        //     return null;
        // }

        // private List<E> FindExecutersByBeh<E, B>() where E : Executer where B : Behaviour
        // {
        //     var list = new List<E>();
        //     foreach (var exe in Executors)
        //     {
        //         if (exe is E)
        //         {
        //             if (exe.GetBehaviour<B>() != null)
        //             {
        //                 list.Add(exe as E);
        //             }
        //         }
        //     }

        //     return list;
        // }


        [TextArea(0, 1000)] [SerializeField] private string _log;

        private IEnumerator DebugResources()
        {
            yield return new WaitForSeconds(2);
            while (true)
            {
                var sections = Market.GetDataForLog();
                _log = "";
                for (int i = 0; i < sections.Length; i++)
                {
                    if (sections[i].Count == 0) continue;

                    _log += $"{((ResourceType) i).ToString()}\n";
                    foreach (var kv in sections[i].GetCompanies())
                    {
                        _log += $"{kv.Key.gameObject.name} = {(int) kv.Value}_{(int) kv.Price}$\n";
                    }

                    _log += "\n";

                }

                yield return new WaitForSeconds(1);
            }
        }
    }
}

