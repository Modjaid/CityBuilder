using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Demo;
using UnityEngine;

namespace PointMap
{
    public class Map
    {
        public Nodes _nodes;
        private HashSet<Agent> _agents;
        private int _minRayLength;
        private int _maxRayLength;
        public bool IsUpdating;


        public Map(int minAxis, int MaxAxis)
        {
            _agents = new HashSet<Agent>();
            _nodes = new Nodes();

            _minRayLength = minAxis;
            _maxRayLength = MaxAxis;
            IsUpdating = false;
        }

        public Map(Points obstacles, HashSet<Agent> agents, int minAxis, int MaxAxis)
        {
            _nodes = new Nodes();
            _agents = agents;

            _minRayLength = 0;
            _maxRayLength = MaxAxis;
        }

        public async UniTask UpdateAgentAreas(HashSet<Agent> agents)
        {
            if(IsUpdating) return;
            IsUpdating = true;
            
            var readyAgents = 0;
            foreach (var agent in agents)
            {
                if (agent.IsActive)
                {
                    _nodes.Clear(agent);
                    agent.Research().ContinueWith(() => readyAgents++);
                }
                else
                {
                    readyAgents++;
                }
            }

            await UniTask.WaitUntil(() => readyAgents == agents.Count);

            foreach (var agent in agents)
            {
                if (agent.IsActive)
                {
                    _nodes.AddAgentPoints(agent);
                }
            }

            EstimateAgents();
            await UniTask.Yield();
            IsUpdating = false;
        }

        private void EstimateAgents()
        {
            var agents = new List<Agent>(_agents);
            foreach (var agent in agents)
            {
                if (agent.IsActive)
                {
                    var points = agent.Area;
                    agent.OfferCost = 0;
                    foreach (var point in points)
                    {
                        //todo: цикл не успевает завершитться кто то асинхронно его сбивает
                        if (agent == null || !_nodes.ContainsKey(point))
                        {
                            Debug.Log("null");
                        }
                        agent.OfferCost += _nodes[point].Count;
                    }
                }
            }

        }

        public List<Agent> GetSortedAgents(bool isReverse)
        {
            var list = new List<Agent>(_agents);
            list.RemoveAll(agent => !agent.IsActive);
            list.Sort();
            if (isReverse)
            {
                list.Reverse();
            }
            return list;
        }
        
        public HashSet<Agent> GetActiveAgents()
        {
            var activeAgents = new HashSet<Agent>();
            foreach (var agent in _agents)
            {
                if (agent.IsActive)
                {
                    activeAgents.Add(agent);
                }
            }
            return activeAgents;
        }
        
        /// <returns>agents for update</returns>
        public HashSet<Agent> SetNewObstacles(Points area)
        {
            var agentsForUpdate = _nodes.SetNullToPoints(area);
            var newAgents = InstanceNewAgents(area);
            agentsForUpdate.UnionWith(newAgents);
            return agentsForUpdate;
        }

        public void AddSimpleObstacles(Points area)
        {
            _nodes.SetNullToPoints(area);
        }

        public Agent AddNewAgent(Point pos, Point dir)
        {
            var newAgent = new Agent(pos, dir, _nodes, _maxRayLength, _minRayLength);
            _agents.Add(newAgent);
            return newAgent;
        }

        private HashSet<Agent> InstanceNewAgents(Points area)
        {
            var agentsForUpdate = new HashSet<Agent>();
            var leftUp = area.leftUp;
            var rightUp = area.RightUp;
            var leftDown = area.LeftDown;
            var rightDown = area.RightDown;
            var agent = new Agent(leftUp, Point.Left, _nodes, _maxRayLength, _minRayLength);
            _agents.Add(agent);
            agentsForUpdate.Add(agent);

            agent = new Agent(leftUp, Point.Up, _nodes, _maxRayLength, _minRayLength);
            _agents.Add(agent);
            agentsForUpdate.Add(agent);

            agent = new Agent(rightUp, Point.Right, _nodes, _maxRayLength, _minRayLength);
            _agents.Add(agent);
            agentsForUpdate.Add(agent);

            agent = new Agent(rightUp, Point.Up, _nodes, _maxRayLength, _minRayLength);
            _agents.Add(agent);
            agentsForUpdate.Add(agent);

            agent = new Agent(rightDown, Point.Right, _nodes, _maxRayLength, _minRayLength);
            _agents.Add(agent);
            agentsForUpdate.Add(agent);

            agent = new Agent(rightDown, Point.Down, _nodes, _maxRayLength, _minRayLength);
            _agents.Add(agent);
            agentsForUpdate.Add(agent);

            agent = new Agent(leftDown, Point.Left, _nodes, _maxRayLength, _minRayLength);
            _agents.Add(agent);
            agentsForUpdate.Add(agent);

            agent = new Agent(leftDown, Point.Down, _nodes, _maxRayLength, _minRayLength);
            _agents.Add(agent);
            agentsForUpdate.Add(agent);
            return agentsForUpdate;
        }
    }
}