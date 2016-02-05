using Motorcycle.XmlWorker;
using System.IO;
using System.Linq;
using NLog;

namespace Motorcycle.Config.Data
{
    internal static class RemoveEntries
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly object locker = new object();

        public static void Remove(int lineNum, ProductEnum product)
        {
            string direction;

            switch (product)
            {
                case ProductEnum.Motorcycle:
                    direction = "moto";
                    break;
                case ProductEnum.Spare:
                    direction = "spare";
                    break;
                case ProductEnum.Equip:
                    direction = "equip";
                    break;
                default:
                    return;
            }

            lock (locker)
            {
                var filePath = FilePathXmlWorker.GetFilePath(direction);
                if(string.IsNullOrEmpty(filePath))
                    return;
                var rows = File.ReadAllLines(filePath).ToList();
                rows[lineNum] = string.Empty;
                File.WriteAllLines(filePath, rows);
            }
        }

        /// <summary>
        /// Move to unposted + remove row from input file
        /// </summary>
        public static void Remove(DicHolder dicHolder, ProductEnum product, SiteEnum site)
        {
            Remove(dicHolder.LineNum, product);
            lock (locker)
            {
                if (!Directory.Exists("Unposted"))
                    Directory.CreateDirectory("Unposted");

                var fileName = $"Unposted\\{site}{product}Unposted.txt";

                //if (File.Exists(fileName))
                //{
                //    var rows = File.ReadAllLines(fileName).;
                //    var rowsToWrite = 
                //}

                using (var sw = new StreamWriter(fileName, true))
                    sw.WriteLine(dicHolder.Row);
            }
        }

        public static bool DataError(string key, string value, string row, int lineNum, SiteEnum site, ProductEnum type)
        {
            var data = row.Split('\t');
            if (value != string.Empty)
                return false;

            switch (type)
            {
                case ProductEnum.Motorcycle:
                    Log.Warn($"{data[4]} {data[5]} {key} is not in DB", site,
                        type);
                    break;
                case ProductEnum.Spare:
                    Log.Warn($"{data[3]} {data[4]} {key} is not in DB", site,
                        type);
                    break;
                case ProductEnum.Equip:
                    Log.Warn($"{data[3]} {data[5]} {key} is not in DB", site,
                        type);
                    break;
            }

            Remove(new DicHolder {Row = row, LineNum = lineNum}, type, site);

            return true;
        }
    }
}