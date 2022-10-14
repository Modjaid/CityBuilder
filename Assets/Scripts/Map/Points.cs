

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game = GameData.GameData;

namespace PointMap
{
    
    public class Nodes : Dictionary<Point, HashSet<Agent>>
    {
        public Nodes() : base()
        {
        }

        public void Clear(Agent agent)
        {
            var area = agent.Area;
            foreach (var point in area)
            {
                if (this.ContainsKey(point))
                {
                    this[point]?.Remove(agent);
                }
            }
        }

        public void AddAgentPoints(Agent agent)
        {
            var area = agent.Area;
            foreach (var point in area)
            {
                HashSet<Agent> agents;
                this.TryGetValue(point, out agents);
                if (agents == null)
                {
                    agents = new HashSet<Agent>();
                }

                agents.Add(agent);
                this[point] = agents;
            }
        }
        
        /// <summary>
        /// if node dont has any agents it mean obstacle
        /// </summary>
        /// <returns>remove agents</returns>
        public HashSet<Agent> SetNullToPoints(Points area)
        {
            var agents = new HashSet<Agent>();
            // var log = "";
            foreach (var point in area)
            {
                // log += $"point[{point}] ";
                if (TryGetValue(point, out var nodeAgents))
                {
                    if (nodeAgents != null)
                    {
                        agents.UnionWith(nodeAgents);
                        this[point] = null;
                    }
                }
                else
                {
                    this[point] = null;
                }
            }
            
            return agents;
        }

        public bool IsNullPoint(Point point)
        {
            if (this.TryGetValue(point, out var agents))
            {
                if (agents == null)
                {
                    return true;
                }
            }

            return false;
        }
    }

    [Serializable]
    public class Points : HashSet<Point>
    {

        public Points() : base()
        {
        }
        public Points(Point[] points) : base(points){

        }

        public Points(HashSet<Point> points) : base(points)
        {
        }

        public Points(List<Point> points) : base(points)
        {
        }

        public Points(Point pos, Point size) : base()
        {
            for (int x = pos.x; x < size.x + pos.x; x++)
            {
                for (int y = pos.y; y > pos.y - size.y; y--)
                {
                    this.Add(new Point(x, y));
                }
            }
        }


        public int MaxAxis
        {
            get
            {
                var maxLength = int.MinValue;
                foreach (var point in this)
                {
                    var max = Mathf.Max(point.x, point.y);
                    maxLength = Mathf.Max(max, maxLength);
                }

                return maxLength;
            }
        }

        public int MinAxis
        {
            get
            {
                var minLength = int.MaxValue;
                foreach (var point in this)
                {
                    var min = Mathf.Min(point.x, point.y);
                    minLength = Mathf.Min(min, minLength);
                }

                return minLength;
            }
        }

        public Point leftUp
        {
            get
            {
                var point = this.ElementAt(0);
                var next = point;
                var dir = Point.LeftUp;
                next += dir;
                while (this.Contains(next))
                {
                    point = next;
                    ;
                    next += dir;
                }


                dir = Point.Left;
                next = point;
                next += dir;
                while (this.Contains(next))
                {
                    point = next;
                    next += dir;
                }

                dir = Point.Up;
                next = point;
                next += dir;
                while (this.Contains(next += dir))
                {
                    point = next;
                    next += dir;

                }

                return point;
            }

            // get{
            //     var startPoint = this.ElementAt(0);
            //     foreach(var point in this){
            //        startPoint = (point < startPoint) ? point : startPoint;
            //     }
            //     return startPoint;
            // }
        }

        public Point RightUp
        {

            get
            {
                var point = this.ElementAt(0);
                var next = point;
                var dir = Point.RightUp;
                next += dir;
                while (this.Contains(next))
                {
                    point = next;
                    ;
                    next += dir;
                }


                dir = Point.Right;
                next = point;
                next += dir;
                while (this.Contains(next))
                {
                    point = next;
                    next += dir;

                }

                dir = Point.Up;
                next = point;
                next += dir;
                while (this.Contains(next += dir))
                {
                    point = next;
                    next += dir;

                }

                return point;
            }
        }

        public Point RightDown
        {

            get
            {
                var point = this.ElementAt(0);
                var next = point;
                var dir = Point.RightDown;
                next += dir;
                while (this.Contains(next))
                {
                    point = next;
                    ;
                    next += dir;
                }


                dir = Point.Right;
                next = point;
                next += dir;
                while (this.Contains(next))
                {
                    point = next;
                    next += dir;

                }

                dir = Point.Down;
                next = point;
                next += dir;
                while (this.Contains(next))
                {
                    point = next;
                    next += dir;

                }

                return point;
            }
        }

        public Point LeftDown
        {

            get
            {
                var point = this.ElementAt(0);
                var next = point;
                var dir = Point.LeftDown;
                next += dir;
                while (this.Contains(next))
                {
                    point = next;
                    ;
                    next += dir;
                }


                dir = Point.Left;
                next = point;
                next += dir;
                while (this.Contains(next))
                {
                    point = next;
                    next += dir;

                }

                dir = Point.Down;
                next = point;
                next += dir;
                while (this.Contains(next))
                {
                    point = next;
                    next += dir;

                }

                return point;
            }
        }

        public Point Size
        {
            get
            {

#if UNITY_EDITOR
                if (this.Count == 0) Debug.Log("AreaSize == 0");
#endif
                var anyElement = this.ElementAt(0);
                var axisX = anyElement.x;
                var axisY = anyElement.y;

                var size = new Point(0, 0);
                foreach (var point in this)
                {
                    if (point.x == axisX)
                    {
                        size.y++;
                    }

                    if (point.y == axisY)
                    {
                        size.x++;
                    }
                }

                return size;
            }
        }
    }


    [Serializable]
    public struct Point
    {
        public int x;
        public int y;

        public bool IsDiagonal
        {
            get { return Mathf.Abs(x) + Mathf.Abs(y) == 2; }
        }

        public Point ToLocalLeft
        {
            get { return new Point(-y, x); }
        }

        public Point ToLocalRight
        {
            get { return new Point(y, -x); }
        }

        public Point TurnByAxis
        {
            get
            {
                if (y > 0)
                {
                    return Point.Down;
                }

                return Point.Up;
            }
        }

        public int SumAxis
        {
            get { return x + y; }
        }
        
        public static Vector3 Center(Point size, Point pos)
        {
            var x = pos.x + (size.x / 2f);
            var y = pos.y - (size.y / 2f);
            Debug.Log($"x<{x}> = pos.x<{pos.x}> + (size.x<{size.x}> / 2f) \ny<{y}> = pos.y<{pos.y}> - (size.y<{size.y}> / 2f)");
            return new Vector3(x, 0, y) * GameData.GameData.MAIN_SCALE;
        }

        public static readonly Point Zero = new Point(0, 0);
        public static readonly Point One = new Point(1, 1);
        public static readonly Point Up = new Point(0, 1);
        public static readonly Point Down = new Point(0, -1);
        public static readonly Point Left = new Point(-1, 0);
        public static readonly Point Right = new Point(1, 0);

        public static readonly Point LeftUp = new Point(-1, 1);
        public static readonly Point RightUp = new Point(1, 1);
        public static readonly Point LeftDown = new Point(-1, -1);
        public static readonly Point RightDown = new Point(1, -1);

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Point RoundToPoint(Vector2 v) => new Point(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));

        public Point Inverse()
        {
            return new Point(y, x);
        }

        public Vector3 ToVector()
        {
            var vec = new Vector3(x, 0, y);
            return vec * Game.MAIN_SCALE;
        }

        public bool IsFitTo(Point to, out bool isRotate)
        {
            if (to.x >= x && to.y >= y)
            {
                isRotate = false;
                return true;
            }

            if (to.y >= x && to.x >= y)
            {
                isRotate = true;
                return true;
            }

            isRotate = false;
            return false;
        }

        public static Point operator -(Point v) => new Point(-v.x, -v.y);

        public static Point operator +
            (Point a, Point b) => new Point(a.x + b.x, a.y + b.y);

        public static Point operator -(Point a, Point b) => new Point(a.x - b.x, a.y - b.y);

        public static Point operator *(Point a, Point b) => new Point(a.x * b.x, a.y * b.y);

        public static Point operator *(int a, Point b) => new Point(a * b.x, a * b.y);

        public static Point operator *(Point a, int b) => new Point(a.x * b, a.y * b);

        public static Point operator /(Point a, int b) => new Point(a.x / b, a.y / b);

        public static bool operator ==(Point lhs, Point rhs) => lhs.x == rhs.x && lhs.y == rhs.y;

        public static bool operator !=(Point lhs, Point rhs) => !(lhs == rhs);

        public static bool operator <(Point lhs, Point rhs)
        {
            if (lhs.x < rhs.x)
            {
                return lhs.y >= rhs.y;
            }

            ;
            if (lhs.x == rhs.x)
            {
                return lhs.y > rhs.y;
            }

            return false;
        }

        public static bool operator >(Point lhs, Point rhs)
        {
            if (lhs.x < rhs.x)
            {
                return lhs.y <= rhs.y;
            }

            if (lhs.x == rhs.x)
            {
                return lhs.y < rhs.y;
            }

            return false;
        }


        /// <param name="direction">Если по диагонали то всегда будет X</param>
        /// <returns>X or Y</returns>
        public int GetAxisByDirection(Point direction)
        {
            if (direction.IsDiagonal)
            {
                return x;
            }

            return direction.x == 0 ? y : x;
        }

        public override int GetHashCode()
        {
            int num1 = this.x;
            int hashCode = num1.GetHashCode();
            num1 = this.y;
            int num2 = num1.GetHashCode() << 2;
            return hashCode ^ num2;
        }

        public override string ToString()
        {
            return $"[{x}:{y}]";
        }
    }
}
