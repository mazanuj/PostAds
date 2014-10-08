using System.IO;
using System.Linq;
using Motorcycle.XmlWorker;

namespace Motorcycle.Config.Data
{
    internal static class FileCleaner
    {
        private static void RemoveEmptyLinesInFile(string purpose)
        {
            var filePath = FilePathXmlWorker.GetFilePath(purpose);

            if (string.IsNullOrEmpty(filePath)) return;

            var array = File.ReadAllLines(filePath)
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct().ToList();

            if (array.Count() != 0)
                File.WriteAllLines(filePath, array);
            else if (filePath.Contains("Unposted"))
                File.Delete(filePath);
        }

        public static void RemoveEmptyLinesFromAllFiles()
        {
            RemoveEmptyLinesInFile("moto");
            RemoveEmptyLinesInFile("spare");
            RemoveEmptyLinesInFile("equip");
        }
    }
}