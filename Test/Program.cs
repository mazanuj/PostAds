using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Xml.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            GetManufacture();
        }

        static List<Item> GetManufacture()
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
