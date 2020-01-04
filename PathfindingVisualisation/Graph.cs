using System;
using System.Collections.Generic;
using System.Linq;

namespace PathfindingVisualisation
{
    public class Graph : IWeightedGraph
    {
        private MapData data;

        public Graph(MapData data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int CostIncrease { get; set; } = 1;

        public MapData Data
        {
            get => data;
            set
            {
                data = value;
                Width = data.Width;
                Height = data.Height;
            }
        }

        public bool this[Point position]
        {
            get => Data[position];
            set => Data[position] = value;
        }

        public double GetCost(Point from, Point to, List<Point> partialPath)
        {
            var cost = 0;
            var path = new[] { to, from }.Concat(partialPath).ToList();
            for (var i = 0; i < path.Count - 2; i++)
            {
                var current = path[i];
                var previous = path[i + 2];
                if (previous.X != current.X && previous.Y != current.Y)
                {
                    cost += CostIncrease;
                }
                cost += 3;
            }
            return cost;
        }

        public bool IsInBounds(Point position) => Data.IsInBounds(position);

        public bool IsInBounds(int x, int y) => Data.IsInBounds(x, y);

        public IEnumerable<Point> GetNeighborPositions(Point position)
        {
            if (!CanStepOn(position))
            {
                yield break;
            }

            foreach (var neighborPosition in position.GetNeighbors())
            {
                if (CanStepOn(neighborPosition))
                {
                    yield return neighborPosition;
                }
            }
        }

        private bool CanStepOn(Point position)
        {
            if (!IsInBounds(position))
            {
                return false;
            }

            if (this[position])
            {
                return false;
            }
            return true;
        }
    }
}
