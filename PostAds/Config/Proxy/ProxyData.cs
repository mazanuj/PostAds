namespace Motorcycle.Config.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net;
    using System.Text;

    using NLog;

    internal static class ProxyData
    {
        private static int loopVar;
        private const string ErrorMsg = "Error in getting proxy list from";
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public const int CountOfProxySites = 5;

        public static List<ProxyAddressStruct> GetProxyData()
        {
            if (loopVar >= CountOfProxySites) return null; //loopVar = 0;

            //switch (loopVar)
            //{
            //    case 0:
            //        loopVar++;
            //        return MyIpTestCom();

            //    case 1:
            //        loopVar++;
            //        return SocksProxyNetData();

            //    case 2:
            //        loopVar++;
            //        return LetUsHideComData();

            //    case 3:
            //        loopVar++;
            //        return SpysRu();

            //    case 4:
            //        loopVar++;
            //        return XroxyComData();

            //    default:
            //        loopVar++;
            //        return SpysRu();
            //}
            return new List<ProxyAddressStruct>();
        }
    }
}