using System.IO;
using System.Linq;
using Motorcycle.XmlWorker;

namespace Motorcycle.Config
{
    using Data;
    using Sites;
    using NLog;
    using System.Threading.Tasks;

    internal static class Advertising
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        internal static async Task Initialize(byte[] flag)
        {
            //List<InfoHolder>
            var returnDataHolders = await ReturnData.GetData(flag);

            await SitePoster.PostAdvertises(returnDataHolders);

            File.WriteAllLines(FilePathXmlWorker.GetFilePath("moto"),
                File.ReadAllLines(FilePathXmlWorker.GetFilePath("moto"))
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct());

            File.WriteAllLines(FilePathXmlWorker.GetFilePath("spare"),
                File.ReadAllLines(FilePathXmlWorker.GetFilePath("spare"))
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct());

            File.WriteAllLines(FilePathXmlWorker.GetFilePath("equip"),
                File.ReadAllLines(FilePathXmlWorker.GetFilePath("equip"))
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct());
        }
    }
}