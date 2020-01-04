using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PathfindingVisualisation
{
    public struct Point : IEquatable<Point>
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point Zero { get; } = new Point(0, 0);


        public int X { get; set; }

        public int Y { get; set; }

        public static bool operator ==(Point left, Point right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if (ReferenceEquals(left, null))
            {
                return false;
            }
            if (ReferenceEquals(right, null))
            {
                return false;
            }
            return left.EqualsCore(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }

        public static Point operator +(Point left, Point right)
        {
            return new Point(left.X + right.X, left.Y + right.Y);
        }

        public static Point operator -(Point left, Point right)
        {
            return new Point(left.X - right.X, left.Y - right.Y);
        }

        public static Point operator +(Point point)
        {
            return new Point(point.X, point.Y);
        }

        public static Point operator -(Point point)
        {
            return new Point(-point.X, -point.Y);
        }

        public static IEnumerable<Point> Parse(IEnumerable<string> targetStrings)
        {
            foreach (var targetString in targetStrings)
            {
                if (TryParse(targetString, out var point))
                {
                    yield return point;
                }
            }
        }

        public static bool TryParse(string targetString, out Point point)
        {
            if (!string.IsNullOrWhiteSpace(targetString))
            {
                var match = Regex.Match(targetString, @"^\s*(?<X>[0-9]+)\s*[;,]\s*(?<Y>[0-9]+)\s*$");
                if (match.Success)
                {
                    var x = int.Parse(match.Groups["X"].Value);
                    var y = int.Parse(match.Groups["Y"].Value);
                    point = new Point(x, y);
                    return true;
                }
            }
            point = Zero;
            return false;
        }

        public bool Equals(Point other)
        {
            if (ReferenceEquals(other, this))
            {
                return true;
            }
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            return EqualsCore(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, this))
            {
                return true;
            }
            if (ReferenceEquals(obj, null))
            {
                return false;
            }
            return obj is Point other && EqualsCore(other);
        }

        public override int GetHashCode()
        {
            return this.GetHashCodeFromFields(X, Y);
        }

        public override string ToString()
        {
            return $"[{X};{Y}]";
        }

        private bool EqualsCore(Point other)
        {
            return X.Equals(other.X)
                && Y.Equals(other.Y);
        }
    }
}
