using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PracticalWork4
{
    internal class Files
    {
        public static List<T> Deserialization<T>(string path)
        {
            List<T> list = new List<T>();

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<T>();

                list = records.ToList();
            }

            return list;
        }

        public static void Serialization<T>(List<T> list, string path)
        {
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(list);
            }
        }
    }
}