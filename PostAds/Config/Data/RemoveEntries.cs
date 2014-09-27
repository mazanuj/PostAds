using System;
using System.IO;
using System.Linq;
using Motorcycle.XmlWorker;

namespace Motorcycle.Config.Data
{
    internal static class RemoveEntries
    {
        public static bool Remove(DicHolder dicHol, ProductEnum product)
        {
            var locker = new Object();
            var direction = string.Empty;

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
            }
            if (direction == string.Empty) return false;

            lock (locker)
            {
                var rows = File.ReadAllLines(FilePathXmlWorker.GetFilePath(direction)).ToList();
                rows[dicHol.LineNum] = string.Empty;
                File.WriteAllLines(FilePathXmlWorker.GetFilePath(direction), rows);
            }

            return true;
        }
    }
}