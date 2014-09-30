using Motorcycle.XmlWorker;
using System.IO;
using System.Linq;

namespace Motorcycle.Config.Data
{
    internal static class RemoveEntries
    {
        private static readonly object locker = new object();

        public static bool Remove(DicHolder dicHol, ProductEnum product)
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
                rows[dicHol.LineNum] = string.Empty;
                File.WriteAllLines(FilePathXmlWorker.GetFilePath(direction), rows);
            }

            return true;
        }

        public static void Unposted(DicHolder dicHol, ProductEnum product, SiteEnum site)
        {
            lock (locker)
            {
                if (!Directory.Exists("Unposted"))
                    Directory.CreateDirectory("Unposted");

                using (var sw = new StreamWriter(string.Format("Unposted\\{0}{1}Unposted.txt", site, product), true))
                    sw.WriteLine(dicHol.Row);
            }
        }
    }
}