
namespace Motorcycle.Config
{
    using Motorcycle.Config.Data;
    using Motorcycle.Sites;

    using NLog;
    using System.Threading.Tasks;

    internal static class Advertising
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        internal static async Task Initialize(string motoFile, string spareFile, string equipFile, byte[] flag)
        {
            //List<InfoHolder>
            var returnDataHolders = await ReturnData.GetData(motoFile, spareFile, equipFile, flag);

            await SitePoster.PostAdvertises(returnDataHolders);
        }
    }
}