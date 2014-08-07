
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

<<<<<<< HEAD
        public string Id { get; set; }
        public int M { get; set; }
        public int P { get; set; }
        public int U { get; set; }
=======
        public string Id { get;  set; }
        public string M { get;  set; }
        public string P { get;  set; }
        public string U { get;  set; }

        public List<Value> Values { get;  set; }
        
<<<<<<< HEAD
=======
>>>>>>> origin/mazanuj

        public List<Value> Values { get; set; }
>>>>>>> origin/mazanuj
    }
}
