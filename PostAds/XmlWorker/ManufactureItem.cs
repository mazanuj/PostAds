namespace Motorcycle.XmlWorker
{
    using System.Collections.Generic;

    public class ManufactureItem
    {
        public ManufactureItem()
        {
        }

        public ManufactureItem(string id, string m, string p, string u, string o)
        {
            Id = id;
            M = m;
            P = p;
            U = u;
            O = o;

            Values = new List<ManufactureValue>();
        }

        public string Id { get; set; }
        public string M { get; set; }
        public string P { get; set; }
        public string U { get; set; }
        public string O { get; set; }

        public List<ManufactureValue> Values { get; set; }
    }
}