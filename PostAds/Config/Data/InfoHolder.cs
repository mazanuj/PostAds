using System.Collections.Generic;

namespace Motorcycle.Config.Data
{
    public class InfoHolder
    {
        public InfoHolder()
        {
            Data = new List<DicHolder>();
        }
        public SiteEnum Site { get; set; }

        public ProductEnum Type { get; set; }

        public List<DicHolder> Data { get; set; }

        public InfoHolder()
        {
            Data = new List<DicHolder>();
        }
    }

    public class DicHolder
    {        
        public Dictionary<string, string> DataDictionary { get; set; }

        public Dictionary<string, string> FileDictionary { get; set; }
    }

    public enum ProductEnum
    {
        Motorcycle,
        Spare,
        Equip
    }

    public enum SiteEnum
    {
        MotoSale,
        Proday2Kolesa,
        UsedAuto
    }

    public class DicHolder
    {        
        public Dictionary<string, string> DataDictionary { get; set; }

        public Dictionary<string, string> FileDictionary { get; set; }
    }

    public enum ProductEnum
    {
        Motorcycle,
        Spare,
        Equip
    }

    public enum SiteEnum
    {
        MotoSale,
        Proday2Kolesa,
        UsedAuto
    }
}