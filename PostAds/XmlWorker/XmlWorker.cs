using System;
using System.Collections.Generic;
using System.Linq;

namespace Motorcycle.XmlWorker
{
    using System.Xml.Linq;

    public class XmlWorker
    {

        public static List<Item> GetManufacture()
        {
            var xml = XDocument.Load("Main.config");

            var items = (from e in xml.Descendants("manufacture").Descendants("item")
                         select new Item
                         {
                             Id = (string)e.Attribute("id"),
                             M = (int)e.Attribute("m"),
                             P = (int)e.Attribute("p"),
                             U = (int)e.Attribute("u"),
                             Values = e.Descendants("value")
                                           .Select(r => new Value
                                           {
                                               Name = (string)r.Attribute("name"),
                                               Price = Convert.ToInt32(r.Value)
                                           })
                                           .ToList()
                         }).ToList();

            return items;


        }
    }
}
