﻿namespace Motorcycle.Config.Data
{
    using System.Collections.Generic;

    public class DicHolder
    {
        public SiteEnum Site { get; set; }

        public ProductEnum Type { get; set; }

        public Dictionary<string, string> DataDictionary { get; set; }

        public Dictionary<string, string> FileDictionary { get; set; }

        public int LineNum { get; set; }

        public string Row { get; set; }

        public bool IsError { get; set; }
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
        UsedAuto,
        Olx
    }
}