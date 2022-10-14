using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PointMap;
using UnityEngine;

namespace Demo
{
    [SelectionBase]
    public class TestTile : MonoBehaviour
    {
        public Point Pos;

        public Material Material
        {
            get { return transform.GetComponentInChildren<Renderer>().material; }
        }

        public TileType Type;

        public async UniTask Indicate(Color indicateColor, float delay, int indicateCount = 0)
        {
            var mat = Material;
            var color = Material.color;
            mat.color = indicateColor;
            for (int i = 0; i < indicateCount; i++)
            {
                mat.color = indicateColor;
                await UniTask.Delay(TimeSpan.FromSeconds(delay), ignoreTimeScale: false);
                mat.color = color;
                await UniTask.Delay(TimeSpan.FromSeconds(delay), ignoreTimeScale: false);
                await UniTask.Yield();

            }
        }
    }

    public enum TileType
    {
        Obstacle,
        Agent,
        Empty,
        OfferPoint
    }
}

