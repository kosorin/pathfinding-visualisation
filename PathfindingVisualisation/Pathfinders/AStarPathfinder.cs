using Darkness.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PathfindingVisualisation.Pathfinders
{
    public class AStarPathfinder : IPathfinder
    {
        private readonly IWeightedGraph graph;

        public AStarPathfinder(IWeightedGraph graph)
        {
            this.graph = graph;
        }

        public string Name => "A*";

        public static double ManhattanHeuristic(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        public MapPath FindPath(Point start, Point target)
        {
            return FindPath(start, target, ManhattanHeuristic);
        }

        public MapPath FindPath(Point start, Point target, Func<Point, Point, double> heuristic)
        {
            var data = new Dictionary<Point, Point>
            {
                [start] = start
            };

            var costs = new Dictionary<Point, double>
            {
                [start] = 0
            };

            var frontier = new PriorityQueue<PathNode>();
            frontier.Enqueue(new PathNode(start, null), 0);

            while (frontier.TryDequeue(out var node))
            {
                var current = node.Position;
                if (current == target)
                {
                    break;
                }

                foreach (var neighbor in graph.GetNeighborPositions(current))
                {
                    var newHistory = node.History.Take(5).ToList();
                    newHistory.Insert(0, current);
                    var newCost = costs[current] + graph.GetCost(current, neighbor, newHistory);
                    if (!costs.ContainsKey(neighbor) || newCost < costs[neighbor])
                    {
                        costs[neighbor] = newCost;
                        var priority = newCost + heuristic(neighbor, target);
                        frontier.Enqueue(new PathNode(neighbor, newHistory), priority);
                        data[neighbor] = current;
                    }
                }
            }

            return new MapPath(start, target, data);
        }

        private struct PathNode
        {
            public PathNode(Point position, List<Point> history)
            {
                Position = position;
                History = history ?? new List<Point>();
            }

            public Point Position { get; }

            public List<Point> History { get; }

            public override int GetHashCode()
            {
                return Position.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is PathNode node)
                {
                    return Position.Equals(node.Position);
                }
                return false;
            }
        }
    }
}
