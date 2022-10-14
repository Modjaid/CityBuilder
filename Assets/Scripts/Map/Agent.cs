using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Demo;
using UnityEngine;

namespace PointMap
{
    public class Agent : IComparable<Agent>
    {
        public Point Position;
        public int OfferCost;
        public bool IsActive;
        public Point Direction;
        public Points Area;
        public Point AreaSize;
        private Nodes _nodes;
        private int _maxBound;
        private int _minBound;

        public Agent(Point pos, Point dir, Nodes nodes, int maxBound, int minBound)
        {
            Area = new Points();
            Position = pos;
            Direction = dir;
            _maxBound = maxBound;
            _minBound = minBound;
            _nodes = nodes;
            IsActive = true;
        }

        public async UniTask Research()
        {
            if (!IsActive)
            {
                AreaSize = Point.Zero;
                return;
            }

            var steps = await GetRayLength(Position + Direction, Direction, _maxBound);
            await UniTask.Yield();
            // await TileDrawer.GetOrAddTilesByRay(Position, Direction, _maxBound);
            int minLeftRay = _maxBound;
            int minRightRay = _maxBound;
            var currentPos = Position;

            if (steps < _minBound + 1)
            {
                IsActive = false;
                AreaSize = Point.Zero;
                return;
            }


            for (int i = 1; i < steps; i++)
            {

                currentPos += Direction;
                var currentLeftRay = await GetRayLength(currentPos, Direction.ToLocalLeft, minLeftRay);
                await UniTask.Yield();
                // await TileDrawer.GetOrAddTilesByRay(currentPos, Direction.ToLocalLeft, minLeftRay);
                var currentRightRay = await GetRayLength(currentPos, Direction.ToLocalRight, minRightRay);
                await UniTask.Yield();
                // await TileDrawer.GetOrAddTilesByRay(currentPos, Direction.ToLocalRight, minLeftRay);
                minLeftRay = Math.Min(currentLeftRay, minLeftRay);
                minRightRay = Math.Min(currentRightRay, minRightRay);

                if (minLeftRay + minRightRay < _minBound + 1)
                {
                    IsActive = false;
                    AreaSize = Point.Zero;
                    return;
                }
            }

            currentPos = Position;
            Area = new Points();
            for (int i = 1; i < steps; i++)
            {
                currentPos += Direction;
                var rayPoints = GetFreePoints(currentPos, Direction.ToLocalRight, minLeftRay, minRightRay);
                Area.UnionWith(rayPoints);
                await UniTask.Yield();
            }

            AreaSize = Area.Size;
            await UniTask.Yield();
        }

        private Points GetFreePoints(Point pos, Point dir, int leftCount, int rightCount)
        {
            Points points = new Points();
            pos -= dir * leftCount - dir;
            for (int i = 1; i < leftCount + rightCount; i++)
            {
                points.Add(pos);
                pos += dir;
            }

            return points;
        }

        public async UniTask<int> GetRayLength(Point pos, Point dir, int maxRayLength)
        {
            int i = 0;
            while (i < maxRayLength)
            {
                if (_nodes.IsNullPoint(pos))
                {
                    break;
                }
                
                pos += dir;
                i++;
                // await UniTask.Yield();
            }

            return i;
        }

        public int CompareTo(Agent other)
        {
            return OfferCost.CompareTo(other.OfferCost);
        }
    }
}
