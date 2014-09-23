using xNet.Net;

namespace Motorcycle.Config.Proxy
{
    internal class ProxyAddressStruct
    {
        public ProxyType Type { get; set; }
        public string ProxyAddresses { get; set; }

        public static ProxyType GetProxyTypeEnumFromString(string type)
        {
            switch (type)
            {
                case "Socks5":
                    return ProxyType.Socks5;

                case "Http":
                case "Ssl":
                    return ProxyType.Http;

                case "Socks4":
                    return ProxyType.Socks4;

                default:
                    return ProxyType.Http;
            }
        }
    }
}