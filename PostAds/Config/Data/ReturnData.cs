using System.Collections.Generic;
using System.Linq;

namespace Motorcycle.Config.Data
{
    internal static class ReturnData
    {
        internal static List<Dictionary<string, string>>[] Moto(IEnumerable<string> list, IList<byte> flag)
        {
            var returnArray = new List<Dictionary<string, string>>[3];
            if (flag[0] == 1)
            {
                var motoData = list.Select(row => MotosaleData.GetData(new List<string>(row.Split('\t')))).Select(t => (Dictionary<string, string>) t).ToList();
                returnArray[0] = motoData;
            }
            return returnArray;
        }
    }
}