using Motorcycle.Interfaces;
using NLog;

namespace Motorcycle.Config.Data
{
    internal class OlxData : ISiteData
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public DicHolder GetMoto(string row, int lineNum)
        {
            return new DicHolder();
        }

        public DicHolder GetSpare(string row, int lineNum)
        {
            return new DicHolder();
        }

        public DicHolder GetEquip(string row, int lineNum)
        {
            return new DicHolder();
        }
    }
}