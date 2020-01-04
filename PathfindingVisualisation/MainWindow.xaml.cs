using PathfindingVisualisation.Pathfinders;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PathfindingVisualisation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int GridSize = 24;

        private readonly string path;

        private Point start;

        private Point target;

        public MainWindow()
        {
            InitializeComponent();

            path = "MapData.txt";

            try
            {
                Graph = new Graph(MapReader.ReadDataFromFile(path));
            }
            catch
            {
                Graph = new Graph(new MapData(new bool[24, 32]));
            }

            Pathfinder = new AStarPathfinder(Graph);

            NodeCountInfo.Content = Surface.Children.Count;
            StartPositionInfo.Content = Start;
            TargetPositionInfo.Content = Target;
            PositionInfo.Content = Point.Zero;
            CostIncreaseInfo.Content = Graph.CostIncrease;
            PathCostInfo.Content = 0;
            AlgorithmInfo.Content = Pathfinder.Name;

            Update();
        }

        public Point Start
        {
            get => start;
            private set
            {
                start = value;
                StartPositionInfo.Content = value;
            }
        }

        public Point Target
        {
            get => target;
            private set
            {
                target = value;
                TargetPositionInfo.Content = value;
            }
        }

        public Graph Graph { get; private set; }

        public IPathfinder Pathfinder { get; private set; }

        private void Update()
        {
            var path = Pathfinder.FindPath(Start, Target);
            PathCostInfo.Content = GetPathCost(path);

            Surface.Width = Graph.Width * GridSize;
            Surface.Height = Graph.Height * GridSize;

            for (var y = 0; y < Graph.Height; y++)
            {
                for (var x = 0; x < Graph.Width; x++)
                {
                    var position = new Point(x, y);
                    var node = GetNode(position);
                    if (node != null)
                    {
                        node.Background =
                            position == start ? Brushes.Green :
                            position == target ? Brushes.Red :
                            path.HasRoute && path.Route.ContainsKey(position) ? Brushes.Blue :
                            Graph[position] ? Brushes.Black :
                            Brushes.Transparent;
                    }
                }
            }
        }

        private object GetPathCost(MapPath path)
        {
            var cost = 0;
            if (path.HasRoute)
            {
                var direction = 2;
                var previous = Start;
                var current = path.Route[Start];
                for (var i = 1; i < path.RouteLength; i++)
                {
                    var next = path.Route[current];
                    cost += 3;
                    if (next.X != previous.X && next.Y != previous.Y)
                    {
                        cost += 1;
                    }
                    previous = current;
                    current = next;
                }
            }
            return cost;
        }

        private Border GetNode(Point position)
        {
            if (!Graph.IsInBounds(position))
            {
                return null;
            }
            var node = Surface.Children.OfType<Border>().FirstOrDefault(x => x.Tag is Point p && p == position);
            if (node == null)
            {
                node = new Border
                {
                    Tag = position,
                    Background = Brushes.Transparent,
                    Width = GridSize + 1,
                    Height = GridSize + 1,
                    BorderThickness = new Thickness(1),
                    BorderBrush = Brushes.DarkGray,
                    Margin = new Thickness(-1, -1, 0, 0),
                };
                Canvas.SetLeft(node, position.X * GridSize);
                Canvas.SetTop(node, position.Y * GridSize);
                Surface.Children.Add(node);
                NodeCountInfo.Content = (int)NodeCountInfo.Content + 1;
            }
            return node;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var position = GetPosition(e);
            if (Graph.IsInBounds(position))
            {
                if (e.RightButton == MouseButtonState.Pressed && !Graph[position])
                {
                    if (Keyboard.IsKeyDown(Key.LeftCtrl))
                    {
                        Start = position;
                    }
                    else
                    {
                        Target = position;
                    }
                    Update();
                }
                else if (Start != position && Target != position)
                {
                    Graph[position] = !Graph[position];
                    Update();
                }
            }
        }

        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            PositionInfo.Content = GetPosition(e);
        }

        private Point GetPosition(MouseEventArgs e)
        {
            return e.GetPosition(Surface).ToPoint(GridSize);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MapWriter.WriteDataToFile(path, Graph.Data);
        }

        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Graph.CostIncrease = Math.Max(0, Graph.CostIncrease + Math.Sign(e.Delta));
            CostIncreaseInfo.Content = Graph.CostIncrease;
            Update();
        }
    }
}
