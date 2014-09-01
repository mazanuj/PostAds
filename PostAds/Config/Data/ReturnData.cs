using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace Motorcycle.Config.Data
{
    internal static class ReturnData
    {
        internal static List<Dictionary<string, string>> Moto(List<string> list, IList<byte> flag)
        {
            if (flag[0] == 1)
            {
                var motoData = new List<Dictionary<string, string>>();
                foreach (var row in list)
                {
                    var t = MotosaleData.GetData(new List<string>(row.Split('\t')));
                    motoData.Add(t);
                }
            }
        }
    }
}