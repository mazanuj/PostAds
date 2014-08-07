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
                             M = (string)e.Attribute("m"),
                             P = (string)e.Attribute("p"),
                             U = (string)e.Attribute("u"),
                             Values = e.Descendants("value")
                                           .Select(r => new Value
                                           {
                                               Name = (string)r.Attribute("name"),
                                               Val = r.Value
                                           })
                                           .ToList()
                         }).ToList();

            return items;


        }
    }
}
