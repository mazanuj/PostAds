namespace Motorcycle.Config
{
    using Motorcycle.XmlWorker;
    using System.IO;
    using System.Linq;

    internal class FileCleaner
    {
        private static void RemoveEmptyLinesInFile(string purpose)
        {
            var filePath = FilePathXmlWorker.GetFilePath(purpose);

            if (!string.IsNullOrEmpty(filePath))
            {
                File.WriteAllLines(
                    filePath,
                    File.ReadAllLines(filePath)
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Distinct());
            }
        }

        public static void RemoveEmptyLinesFromAllFiles()
        {
            RemoveEmptyLinesInFile("moto");
            RemoveEmptyLinesInFile("spare");
            RemoveEmptyLinesInFile("equip");
        }
    }
}
