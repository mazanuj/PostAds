namespace Motorcycle.XmlWorker
{
    public class ProxyAddressItem
    {
        public ProxyAddressItem()
        {
        }

        public ProxyAddressItem(string proxyAddress, string type)
        {
            ProxyAddress = proxyAddress;
            Type = type;
        }

        public string ProxyAddress { get; set; }
        public string Type { get; set; }
    }
}