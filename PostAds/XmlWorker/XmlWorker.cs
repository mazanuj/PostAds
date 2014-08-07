using System;
using System.Collections.Generic;
using System.Linq;

namespace Motorcycle.XmlWorker
{
    using System.Xml.Linq;

    public static class XmlWorker
    {

        public static IEnumerable<Item> GetManufacture()
        {
            var xml = XDocument.Load("Main.config");

            var items = (from e in xml.Descendants("manufacture").Descendants("item")
<<<<<<< HEAD
                select new Item
                {
                    Id = (string) e.Attribute("id"),
                    M = (int) e.Attribute("m"),
                    P = (int) e.Attribute("p"),
                    U = (int) e.Attribute("u"),
                    Values = e.Descendants("value")
                        .Select(r => new Value
                        {
                            Name = (string) r.Attribute("name"),
                            Price = Convert.ToInt32(r.Value)
                        })
                        .ToList()
                }).ToList();
=======
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
>>>>>>> origin/mazanuj

            return items;
        }
    }
}
