using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using PointMap;
using UnityEngine;

namespace GameData{
    [CreateAssetMenu(fileName = "TerrainObstacles", menuName = "GameData/TerrainObstacles", order = 1)]
    public class TerrainObstacles : ScriptableObject    
    {
        public Point[] Obstacles;

        public IEnumerator InitObstacles(Terrain terrain){
            var obstacles = new HashSet<Point>();
            int width = (int)(terrain.terrainData.size.x / GameData.MAIN_SCALE);
            int height = (int)(terrain.terrainData.size.z / GameData.MAIN_SCALE);
            for(int x = 1; x <= width; x++){
                for(int y = 1; y <= height; y++){
                    
                    var point = new Point(x,y);
                    var currentPos = point.ToVector() + Vector3.up * 15;
                     currentPos += terrain.transform.position;
                     
                     if (TryTouchGround(currentPos))
                     {
                         obstacles.Add(point);
                         Debug.DrawRay(currentPos, Vector3.up * 5, Color.green, 5f);
                     }
                     else
                     {
                         Debug.DrawRay(currentPos, Vector3.down, Color.red, 1f, false);
                     }
                     
                }
                yield return null;
            }
            Obstacles = obstacles.ToArray();
            Debug.Log("FINISH");
        }

        public bool TryTouchGround(Vector3 currentPos)
        {
            var waterCash = new HashSet<Vector3>();
            var left = new Vector3(-1, 0, 0);
            var leftUp = new Vector3(-1, 0, 1);
            var up = new Vector3(0, 0, 1);
            var rightUp = new Vector3(1, 0, 1);
            var right = new Vector3(1, 0, 0);
            var rightDown = new Vector3(1, 0, -1);
            var down = new Vector3(0, 0, -1);   
            var leftDown = new Vector3(-1, 0, -1);

            RayWithOffset(new Vector3(0, 0, 0));
            RayWithOffset(left);
            RayWithOffset(leftUp);
            RayWithOffset(up);
            RayWithOffset(rightUp);
            RayWithOffset(right);
            RayWithOffset(rightDown);
            RayWithOffset(down);
            RayWithOffset(leftDown);

            return (waterCash.Count < 7 && waterCash.Count > 0);

            void RayWithOffset(Vector3 offset)
            {
                if(Physics.Raycast(currentPos + offset, Vector3.down, out var hit)){
                    if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")){
                        return;
                    }
                }
                waterCash.Add(currentPos + offset);
            }
        }
    }
}
