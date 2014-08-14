using System.Collections.Generic;
using NLog;

namespace Motorcycle.Config
{
    internal static class Advertising
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private static List<Dictionary<string, string>> motoList;
        private static List<Dictionary<string, string>> spareList;
        private static List<Dictionary<string, string>> equipList;

        internal static void Initialize(string motoFile, string spareFile, string equipFile, byte[] flag)
        {
            if (motoFile != null) motoList = new List<Dictionary<string, string>>(Moto(motoFile));
            if (spareFile != null) spareList = new List<Dictionary<string, string>>(Spare(spareFile));
            if (equipFile != null) equipList = new List<Dictionary<string, string>>(Equip(equipFile));

            StartPosting(flag);
        }

        private static void StartPosting(byte[] flag)
        {
            if (flag[0] == 1)
            {
                
            }
        }

        private static IEnumerable<Dictionary<string, string>> Moto(string motoFile)
        {
            return null;
        }

        private static IEnumerable<Dictionary<string, string>> Spare(string spareFile)
        {
            return null;
        }

        private static IEnumerable<Dictionary<string, string>> Equip(string equip)
        {
            return null;
        }

        private static void PostMoto(IEnumerable<Dictionary<string, string>> data)
        {
            
        }

        private static void PostUsed(IEnumerable<Dictionary<string, string>> data)
        {

        }

        private static void PostKol(IEnumerable<Dictionary<string, string>> data)
        {

        }
    }
}