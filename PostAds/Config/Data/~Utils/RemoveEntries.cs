﻿using Motorcycle.XmlWorker;
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
                var rows = File.ReadAllLines(FilePathXmlWorker.GetFilePath(direction)).ToList();
                rows[lineNum] = string.Empty;
                File.WriteAllLines(FilePathXmlWorker.GetFilePath(direction), rows);
            }
        }

        /// <summary>
        /// Move to unposted + remove row from input file
        /// </summary>
        /// <param name="lineNum"></param>
        /// <param name="row"></param>
        /// <param name="product"></param>
        /// <param name="site"></param>
        public static void Remove(int lineNum, ProductEnum product, string row, SiteEnum site)
        {
            Remove(lineNum, product);
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
                    Log.Warn(string.Format("{0} {1} {2} is not in DB ({2} {3})", data[4], data[5], key, site), site,
                        type);
                    break;
                case ProductEnum.Spare:
                    Log.Warn(string.Format("{0} {1} {2} is not in DB ({2} {3})", data[3], data[4], key, site), site,
                        type);
                    break;
                case ProductEnum.Equip:
                    Log.Warn(string.Format("{0} {1} {2} is not in DB ({2} {3})", data[3], data[5], key, site), site,
                        type);
                    break;
            }

            Remove(lineNum, type, row, site);

            return true;
        }
    }
}