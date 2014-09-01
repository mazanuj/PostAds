using System.Collections.Generic;

namespace Motorcycle.Config.Data
{
    public class InfoHolder
    {
        public SiteEnum Site { get; set; }

        public ProductEnum Type { get; set; }

        public List<DicHolder> Data { get; set; }
    }
}