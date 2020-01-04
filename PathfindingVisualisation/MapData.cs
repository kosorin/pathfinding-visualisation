using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PathfindingVisualisation
{
    public class MapData : IEquatable<MapData>
    {
        private static bool defaultValue = false;


        public int Width { get; private set; }

        public int Height { get; private set; }

        private bool[,] rawData;
        private bool[,] RawData
        {
            get => rawData;
            set
            {
                rawData = value;
                Width = rawData.GetWidth();
                Height = rawData.GetHeight();
            }
        }

        public MapData(bool[,] rawData)
        {
            RawData = rawData ?? throw new ArgumentNullException(nameof(rawData));
        }


        public bool this[Point position]
        {
            get => RawData[position.Y, position.X];
            set => RawData[position.Y, position.X] = value;
        }

        public bool this[int x, int y]
        {
            get => RawData[y, x];
            set => RawData[y, x] = value;
        }

        public bool AtSafe(Point position)
        {
            if (IsInBounds(position))
            {
                return this[position];
            }
            return defaultValue;
        }

        public bool AtSafe(int x, int y)
        {
            if (IsInBounds(x, y))
            {
                return this[x, y];
            }
            return defaultValue;
        }

        public bool IsInBounds(Point position) => position.X >= 0 && position.Y >= 0 && position.X < Width && position.Y < Height;

        public bool IsInBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;


        public void Replace(MapData other, Point offset)
        {
            RawData.SetSubArraySafe2D(offset.Y, offset.X, other.RawData);
        }

        public void Replace(Func<bool, bool> replace)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    this[x, y] = replace(this[x, y]);
                }
            }
        }


        public bool Equals(MapData other)
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
            return obj is MapData other && EqualsCore(other);
        }

        public override int GetHashCode()
        {
            var seed = RawData.GetHashCodeFromArray2D();
            return this.GetHashCodeFromFieldsWithSeed(seed, Width, Height);
        }

        public static bool operator ==(MapData left, MapData right)
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

        public static bool operator !=(MapData left, MapData right)
        {
            return !(left == right);
        }

        protected virtual bool EqualsCore(MapData other)
        {
            return Width == other.Width
                && Height == other.Height
                && RawData.Equals2D(other.RawData);
        }


        internal void Serialize(StreamWriter sw)
        {
            sw.Write($"{Width};");
            sw.Write($"{Height};");
            sw.WriteLine();
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    sw.Write($"{(this[x, y] ? 1 : 0)}\t");
                }
                sw.WriteLine();
            }
            sw.WriteLine();
        }

        internal static MapData Deserialize(StreamReader sr)
        {
            var sizeLine = sr.ReadLine();
            var sizeMatch = Regex.Match(sizeLine, $@"^(?<{nameof(Width)}>[0-9]+);(?<{nameof(Height)}>[0-9]+);$");
            if (sizeMatch.Success)
            {
                var width = int.Parse(sizeMatch.Groups[nameof(Width)].Value);
                var height = int.Parse(sizeMatch.Groups[nameof(Height)].Value);

                var rawData = new bool[height, width];
                for (var y = 0; y < height; y++)
                {
                    var line = sr.ReadLine();
                    var lineData = line.Split()
                      .Where(x => !string.IsNullOrWhiteSpace(x))
                      .Select(int.Parse)
                      .Select(x => x != 0)
                      .ToList();

                    if (lineData.Count != width)
                    {
                        throw new InvalidOperationException($"Wrong column count: {lineData.Count}; Expected: {width}; Row: {y + 1}");
                    }

                    for (var x = 0; x < width; x++)
                    {
                        rawData[y, x] = lineData[x];
                    }
                }
                return new MapData(rawData);
            }
            else
            {
                throw new Exception("Could not parse map size");
            }
        }
    }
}
