using Motorcycle.Config.Proxy;

namespace Motorcycle.Config
{
    using Data;
    using Sites;
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