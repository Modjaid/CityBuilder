//
// using System;
// using System.Collections.Generic;
// using System.Runtime.CompilerServices;
// using Cysharp.Threading.Tasks;
// using Cysharp.Threading.Tasks.Triggers;
// using PointMap;
// using UnityEngine;
// using Random = UnityEngine.Random;
// using Game = GameData.GameData;
//
// namespace Demo
// {
//     public class TileDrawer : MonoBehaviour
//     {
//         public TestTile TilePrefab;
//         public float DelayStep;
//
//         private Dictionary<Point, TestTile> _tiles;
//         
//         private static TileDrawer Instance;
//
//         [HideInInspector] public bool IsNext = false;
//
//         public static Color RandColor
//         {
//             get { return Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f); }
//         }
//
//         public void Awake()
//         {
//             Instance = this;
//             _tiles = new Dictionary<Point, TestTile>();
//         }
//
//         public void Update()
//         {
//             if (Input.GetKeyDown(KeyCode.Space))
//             {
//                 IsNext = !IsNext;
//                 DelayStep = 0.01f;
//             }
//
//             // if(Input.GetKeyDown(KeyCode.Q)){
//             //     foreach(var tile in _tiles.Values){
//             //         Destroy(tile.gameObject);
//             //     }
//             //     Init();
//             //     SearchingView();
//             // }
//         }
//
//         public TestTile[] GetOrAddTiles(Points points, Color color, TileType type)
//         {
//             if(!Instance.enabled) return null;
//             
//             var list = new List<TestTile>();
//             foreach (var point in points)
//             {
//                 if (_tiles.ContainsKey(point))
//                 {
//                     _tiles[point].Material.color = color;
//                     _tiles[point].Type = type;
//                     list.Add(_tiles[point]);
//                     continue;
//                 }
//
//                 var newTile = Instantiate(TilePrefab.gameObject, point.ToVector(), Quaternion.identity)
//                     .GetComponent<TestTile>();
//                 newTile.gameObject.name = $"[X:{point.x}] [Y:{point.y}]";
//                 newTile.Pos = point;
//                 newTile.Material.color = color;
//                 newTile.transform.localScale *= Game.MAIN_SCALE;
//                 newTile.Type = type;
//                 _tiles[point] = newTile;
//                 list.Add(newTile);
//             }
//
//             return list.ToArray();
//         }
//
//         public static void SetArea(Points area, Color colorTileType, TileType type)
//         {
//             if(!Instance.enabled) return;
//             
//             foreach (var point in area)
//             {
//                 GetOrAddTile(point, colorTileType, type);
//             }
//             foreach (var point in area)
//             {
//                 GetOrAddTile(point, Color.white, type);
//             }
//         }
//
//         public static async UniTask GetOrAddTilesByRay(Point pos, Point dir, int maxRayLength)
//         {
//             if(!Instance.enabled) return;
//             
//             int i = 0;
//             while (i < maxRayLength)
//             {
//                 if (Instance._tiles.TryGetValue(pos, out var tile))
//                 {
//                     if (tile.Type == TileType.Obstacle)
//                     {
//                        await GetOrAddTile(pos, Color.cyan, TileType.Obstacle).Indicate(Color.red, 0.3f, 4);
//                         break;
//                     }
//                 }
//
//                await GetOrAddTile(pos, Color.white, TileType.Empty).Indicate(Color.blue, 0, 1);
//                 await UniTask.Yield();
//                 pos += dir;
//                 i++;
//             }
//         }
//
//         public static TestTile GetOrAddTile(Point point, Color color, TileType type)
//         {
//             if(!Instance.enabled) return null;
//             
//             var _tiles = Instance._tiles;
//             if (_tiles.ContainsKey(point))
//             {
//                 _tiles[point].Material.color = color;
//                 _tiles[point].Type = type;
//                 return _tiles[point];
//             }
//
//             var newTile = Instantiate(Instance.TilePrefab.gameObject, point.ToVector(), Quaternion.identity)
//                 .GetComponent<TestTile>();
//             newTile.gameObject.name = $"[X:{point.x}] [Y:{point.y}]";
//             newTile.Pos = point;
//             newTile.Material.color = color;
//             newTile.transform.localScale *= Game.MAIN_SCALE;
//             newTile.Type = type;
//             _tiles[point] = newTile;
//             return newTile;
//         }
//
//         public void DestroyAllTiles()
//         {
//             if(!Instance.enabled) return;
//             
//             var destoys = new List<Point>();
//             foreach (var tile in _tiles)
//             {
//                 if (tile.Value.Type == TileType.OfferPoint)
//                 {
//                     Destroy(tile.Value.gameObject);
//                     destoys.Add(tile.Key);
//                 }
//             }
//
//             foreach (var point in destoys)
//             {
//                 _tiles.Remove(point);
//             }
//
//         }
//
//         public static void DestroyAllByType(TileType type)
//         {
//             if(!Instance.enabled) return;
//             
//             var destoys = new List<Point>();
//             foreach (var tile in Instance._tiles)
//             {
//                 if (tile.Value.Type == type)
//                 {
//                     Destroy(tile.Value.gameObject);
//                     destoys.Add(tile.Key);
//                 }
//                 foreach (var point in destoys)
//                 {
//                     Instance._tiles.Remove(point);
//                 }
//             }
//         }
//         
//         public static async UniTask IndicateAllTilesByType(TileType type, Color color, float delay = 0.2f, int indicateCount = 3)
//         {
//             if(!Instance.enabled) return;
//             
//             foreach (var tile in Instance._tiles)
//             {
//                 if (tile.Value.Type == type)
//                 {
//                     tile.Value.Indicate(color, delay, indicateCount);
//                     // await UniTask.Delay(TimeSpan.FromSeconds(0.2), ignoreTimeScale: false);
//                 }
//             }
//         }
//         public static async UniTask IndicateAllTilesByArea(Points Area, Color color, float delay = 0.2f, int indicateCount = 3)
//         {
//             if(!Instance.enabled) return;
//             
//             foreach (var point in Area)
//             {
//                 if (Instance._tiles.TryGetValue(point, out var value))
//                 {
//                     value.Indicate(color, delay, indicateCount);
//                     // await UniTask.Delay(TimeSpan.FromSeconds(0.2), ignoreTimeScale: false);
//                 }
//             }
//         }
//
//         public void DrawAll(Nodes nodes)
//         {
//             foreach (var node in nodes)
//             {
//                 if (node.Value != null)
//                 {
//                     var strong = node.Value.Count;
//                     var color = new Color(1, 0, 0, strong * 0.1f);
//                     GetOrAddTile(node.Key, color, TileType.OfferPoint);
//                 }
//                 else
//                 {
//                     GetOrAddTile(node.Key, Color.black, TileType.Obstacle);
//                 }
//             }
//         }
//         
//         
//     }
//
//     [Serializable]
//     public struct EditorAgent
//     {
//         public Point pos;
//         public DirEnum dir;
//
//         public Point Dir
//         {
//             get
//             {
//                 switch (dir)
//                 {
//                     case DirEnum.Up: return Point.Up;
//                     case DirEnum.Down: return Point.Down;
//                     case DirEnum.Left: return Point.Left;
//                     case DirEnum.Right: return Point.Right;
//                 }
//
//                 return Point.Down;
//             }
//         }
//     }
//
//     public enum DirEnum
//     {
//         Left,
//         Right,
//         Up,
//         Down
//     }
// }