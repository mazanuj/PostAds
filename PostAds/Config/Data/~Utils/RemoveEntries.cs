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

        public static bool Remove(int lineNum, ProductEnum product)
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
                    return false;
            }

            lock (locker)
            {
                var rows = File.ReadAllLines(FilePathXmlWorker.GetFilePath(direction)).ToList();
                rows[lineNum] = string.Empty;
                File.WriteAllLines(FilePathXmlWorker.GetFilePath(direction), rows);
            }

            return true;
        }

        public static void Unposted(string row, ProductEnum product, SiteEnum site)
        {
            lock (locker)
            {
                if (!Directory.Exists("Unposted"))
                    Directory.CreateDirectory("Unposted");

                using (var sw = new StreamWriter(string.Format("Unposted\\{0}{1}Unposted.txt", site, product), true))
                    sw.WriteLine(row);
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
                    Log.Warn("{0} {1} {2} is no in DB ({2} {3})", data[4], data[5], key, site, type);
                    break;
                case ProductEnum.Spare:
                    Log.Warn("{0} {1} {2} is no in DB ({2} {3})", data[3], data[4], key, site, type);
                    break;
                case ProductEnum.Equip:
                    Log.Warn("{0} {1} {2} is no in DB ({2} {3})", data[3], data[5], key, site, type);
                    break;
            }

            Unposted(row, type, site);
            Remove(lineNum, type);

            return true;
        }
    }
}