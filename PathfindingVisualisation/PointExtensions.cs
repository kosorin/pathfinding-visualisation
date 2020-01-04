using System;
using System.Collections.Generic;

namespace PathfindingVisualisation
{
    public static class PointExtensions
    {
        public static IEnumerable<Point> GetNeighbors(this Point point)
        {
            yield return point + new Point(-1, 0);
            yield return point + new Point(0, -1);
            yield return point + new Point(1, 0);
            yield return point + new Point(0, 1);
        }

        public static Point ToPoint(this System.Windows.Point point, int gridSize)
        {
            return new Point((int)Math.Floor(point.X / gridSize), (int)Math.Floor(point.Y / gridSize));
        }
    }
}
