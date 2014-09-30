namespace Motorcycle.Config.Data
{
    using System.Collections.Generic;

    public class InfoHolder
    {
        public InfoHolder()
        {
            Data = new List<DicHolder>();
        }
        public SiteEnum Site { get; set; }

        public ProductEnum Type { get; set; }

        public List<DicHolder> Data { get; set; }
    }

    public class DicHolder
    {        

        public Dictionary<string, string> DataDictionary { get; set; }

        public Dictionary<string, string> FileDictionary { get; set; }

        public int LineNum { get; set; }

        public string Row { get; set; }
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