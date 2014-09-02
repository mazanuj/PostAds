using Motorcycle.Config.Data;
using NLog;

namespace Motorcycle.Config
{
    internal static class Advertising
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        internal static void Initialize(string motoFile, string spareFile, string equipFile, byte[] flag)
        {
            //List<InfoHolder>
            var returnDataHolders = ReturnData.GetData(motoFile, spareFile, equipFile, flag);
        }
    }
}