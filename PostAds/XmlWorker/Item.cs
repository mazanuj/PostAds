
namespace Motorcycle.XmlWorker
{
    using System.Collections.Generic;

    public class Item
    {
        public Item()
        {
        }

        public Item(string id, int m, int p, int u)
        {
            Id = id;
            M = m;
            P = p;
            U = u;

            Values = new List<Value>();
        }

        public string Id { get; set; }
        public int M { get; set; }
        public int P { get; set; }
        public int U { get; set; }

        public List<Value> Values { get; set; }
    }
}