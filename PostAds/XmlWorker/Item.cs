
namespace Motorcycle.XmlWorker
{
    using System.Collections.Generic;

    public class Item
    {
        public Item()
        {
        }

        public Item(string id, string m, string p, string u)
        {
            Id = id;
            M = m;
            P = p;
            U = u;

            Values = new List<Value>();
        }


        public string Id { get; set; }
        public string M { get; set; }
        public string P { get; set; }
        public string U { get; set; }

        public List<Value> Values { get;  set; }
        
    }
}
