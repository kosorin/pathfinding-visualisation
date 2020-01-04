using System.Collections.Generic;

namespace PathfindingVisualisation
{
    public class MapPath
    {
        public MapPath(Point start, Point target, Dictionary<Point, Point> data)
        {
            Start = start;
            Target = target;
            Data = data;

            Route = FindRoute();
        }

        public Point Start { get; }

        public Point Target { get; }

        public Dictionary<Point, Point> Data { get; }

        public bool HasRoute => Route != null;

        public Dictionary<Point, Point> Route { get; }

        public int RouteLength => Route?.Count ?? int.MaxValue;

        private Dictionary<Point, Point> FindRoute()
        {
            var route = new Dictionary<Point, Point>
            {
                [Target] = Target,
            };

            var current = Target;
            if (!Data.TryGetValue(current, out var next))
            {
                return null;
            }

            while (current != Start)
            {
                route[next] = current;
                current = next;

                if (!Data.TryGetValue(current, out next))
                {
                    return null;
                }
            }

            if (!route.ContainsKey(Start))
            {
                return null;
            }
            return route;
        }
    }
}
