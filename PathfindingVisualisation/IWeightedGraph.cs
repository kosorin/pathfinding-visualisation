using System.Collections.Generic;

namespace PathfindingVisualisation
{
    public interface IWeightedGraph
    {
        double GetCost(Point from, Point to, List<Point> history);

        IEnumerable<Point> GetNeighborPositions(Point position);
    }
}
