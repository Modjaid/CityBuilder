// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using Cysharp.Threading.Tasks;
// using PointMap;
// using UnityEngine;
// using Random = UnityEngine.Random;
//
// namespace Demo
// {
//     public class DemoBuilder : MonoBehaviour
//     {
//         public int maxAxisLength;
//         public List<EditorAgent> Agents;
//         public List<Point> _buildings;
//         public float offerDelta;
//         public int borderX;
//         public int borderY;
//
//         private Map _map;
//
//         void Start()
//         {
//             _map = new Map(4, maxAxisLength);
//             foreach (var agent in Agents)
//             {
//                 _map.AddNewAgent(agent.pos, agent.Dir);
//                 TileDrawer.GetOrAddTile(agent.pos, Color.blue, TileType.Agent);
//             }
//             InitBorders();
//             Foo();
//         }
//
//         private async UniTask Foo()
//         {
//             var agents = _map.GetActiveAgents();
//             while (true)
//             {
//                 await TileDrawer.IndicateAllTilesByType(TileType.Obstacle, Color.cyan,0, 0);
//                 await TileDrawer.IndicateAllTilesByType(TileType.Agent, Color.red);
//                 await UniTask.Delay(TimeSpan.FromSeconds(0.5f), ignoreTimeScale: false);
//                 await _map.UpdateAgentAreas(agents);
//
//                 foreach (var agent in agents)
//                 {
//                     if (agent.IsActive)
//                     {
//                         await UniTask.Delay(TimeSpan.FromSeconds(0.3f), ignoreTimeScale: false);
//                         TileDrawer.SetArea(agent.Area, Color.gray, TileType.OfferPoint);
//                         TileDrawer.IndicateAllTilesByArea(agent.Area,new Color(agent.OfferCost * offerDelta,agent.OfferCost * offerDelta, agent.OfferCost * offerDelta, 1), 0f, 0);
//                     }
//                 }
//                 await UniTask.Delay(TimeSpan.FromSeconds(0.5f), ignoreTimeScale: false);
//                 // await TileDrawer.IndicateAllTilesByType(TileType.OfferPoint, Color.grey, 0, 0);
//                 var sortedAgents = _map.GetSortedAgents(false);
//                 var buildings = new List<Point>(_buildings);
//                 var targetAgent = sortedAgents[0];
//                 var building = buildings[Random.Range(0, _buildings.Count)];
//                 bool isRotate;
//                 while (!building.IsFitTo(targetAgent.AreaSize, out isRotate) && buildings.Count > 0)
//                 {
//                     if (sortedAgents.Count == 0)
//                     {
//                         sortedAgents = _map.GetSortedAgents(true);
//                         sortedAgents.Reverse();
//                         building = buildings[Random.Range(0, buildings.Count)];
//                         buildings.Remove(building);
//                     }
//                     sortedAgents.Remove(targetAgent);
//                     targetAgent = sortedAgents[0];
//
//                     await TileDrawer.IndicateAllTilesByType(TileType.OfferPoint, Color.red, 0.2f,1);
//                     await UniTask.Delay(TimeSpan.FromSeconds(0.5f), ignoreTimeScale: false);
//                     await UniTask.Yield();
//                 }
//                 
//                 
//                 await TileDrawer.IndicateAllTilesByArea(targetAgent.Area, Color.green, 0.5f,1);
//                 await UniTask.Delay(TimeSpan.FromSeconds(1.5f), ignoreTimeScale: false);
//
//                 var size = (isRotate) ? building.Inverse() : building;
//                 var buildingArea = new Points(targetAgent.Area.leftUp, size);
//                 agents = _map.SetNewObstacles(buildingArea);
//
//                 foreach(var agent in agents){
//                    TileDrawer.GetOrAddTile(agent.Position, Color.yellow, TileType.Agent).Indicate(Color.gray, 0.3f, 10);
//                 }
//
//                 TileDrawer.SetArea(buildingArea, Color.cyan, TileType.Obstacle);
//                 TileDrawer.IndicateAllTilesByArea(buildingArea, Color.cyan, 0f, 0);
//
//             }
//
//         }
//
//         private void InitBorders()
//         {
//             var topBorder = new Points(new Point(-borderX / 2, borderY / 2), new Point(borderX, 2));
//             var rightBorder =  new Points(new Point(borderX / 2, borderY / 2), new Point(2, borderY));
//             var bottomBorder =  new Points(new Point(-borderX / 2, -borderY / 2), new Point(borderX, 2));
//             var leftBorder =  new Points(new Point(-borderX / 2, borderY / 2), new Point(2, borderY));
//             
//             _map.AddSimpleObstacles(topBorder);
//             _map.AddSimpleObstacles(rightBorder);
//             _map.AddSimpleObstacles(bottomBorder);
//             _map.AddSimpleObstacles(leftBorder);
//             TileDrawer.SetArea(topBorder, Color.black, TileType.Obstacle);
//             TileDrawer.SetArea(rightBorder, Color.black, TileType.Obstacle);
//             TileDrawer.SetArea(bottomBorder, Color.black, TileType.Obstacle);
//             TileDrawer.SetArea(leftBorder, Color.black, TileType.Obstacle);
//         }
//     }
// }
