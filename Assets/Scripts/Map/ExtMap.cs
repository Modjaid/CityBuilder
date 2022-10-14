
using System;
using System.Collections.Generic;
using GameData;
using Managers;
using PointMap;
using UnityEngine;
using UnityEngine.Rendering;
using Game = GameData.GameData;
using ShuffleRandom = System.Random;

namespace External
{
    public static class Rand{
        public static bool IsChance(float chance){
            var rand = UnityEngine.Random.Range(0, 100);
            return rand < chance;
        }
    }

    public static class ExtVector
    {
        public static Vector2Int ToVectorInt(this Vector3 own)
        {
            return Vector2Int.RoundToInt(new Vector2(own.x, own.z));
        }
        public static Vector3 ToVector(this Vector2Int own)
        {
            var vec = new Vector3(own.x, 0, own.y);
            return vec * Game.MAIN_SCALE;
        }
        public static Point ToPoint(this Vector3 own){
            var convert = own / Game.MAIN_SCALE;
           return new Point(Mathf.RoundToInt(convert.x), Mathf.RoundToInt(convert.z));
        }
    }

    public static class ExtCollection
    {

        public static float GetSumResidentsByCompany(this List<Building> buildings, LocalCompany company)
        {
            var sum = 0f;
            foreach (var building in buildings)
            {
                sum += building.GetResidents(company);
            }

            return sum;
        }

        public static List<Building> FindAllByType(this List<Building> owns, BuildingType type)
        {
            var resluts = new List<Building>();
            foreach (var building in owns)
            {
                if (building.Type == type)
                {
                    resluts.Add(building);
                }
            }

            return resluts;
        }
        public static bool TryFind<T>(this List<T> list, Predicate<T> match, out T item)
        {
            item = default;
            foreach (var i in list)
            {
                if (match(i))
                {
                    item = i;
                    return true;
                }
            }

            return false;
        }

        private static ShuffleRandom rng = new ShuffleRandom();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public static class ExtTransform
    {
        public static Vector3 GetSurfacePos(this Transform go){

            int mask = LayerMask.GetMask("Ground", "Water");
            Vector3 point = go.transform.position + (Vector3.up * 50);
            var ray = new Ray(point, Vector3.down);
            if(Physics.Raycast(ray, out RaycastHit hit, 1000, mask)){
                point = hit.point;
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Water")){
                point += Vector3.up * 0.5f;

                }
            }

            return point;
        }
    }

}
