using System;

namespace PathfindingVisualisation.Pathfinders
{
    public interface IPathfinder
    {
        string Name { get; }

        MapPath FindPath(Point start, Point target);

        MapPath FindPath(Point start, Point target, Func<Point, Point, double> heuristic);
    }
}