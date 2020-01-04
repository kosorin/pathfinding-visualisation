using System;
using System.IO;
using System.Text;

namespace PathfindingVisualisation
{
    public static class MapReader
    {
        public static MapData ReadDataFromFile(string path)
        {
            try
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (var ms = new MemoryStream())
                using (var sr = new StreamReader(ms, Encoding.UTF8))
                {
                    fs.CopyTo(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    var mapData = MapData.Deserialize(sr);
                    return mapData;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Could not load map data from file '{path}'", e);
            }
        }
    }
}
