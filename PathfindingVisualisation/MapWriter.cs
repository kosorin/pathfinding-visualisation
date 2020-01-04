using System;
using System.IO;
using System.Text;

namespace PathfindingVisualisation
{
    public static class MapWriter
    {
        public static void WriteDataToFile(string path, MapData mapData)
        {
            try
            {
                using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                using (var ms = new MemoryStream())
                using (var sw = new StreamWriter(ms, Encoding.UTF8))
                {
                    mapData.Serialize(sw);
                    sw.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.CopyTo(fs);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Could not save map data to file '{path}'", e);
            }
        }
    }
}
