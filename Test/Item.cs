
namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Item
    {
        public Item()
        {
            
        }

        public Item(string id, int m, int p, int u)
        {
            this.Id = id;
            this.M = m;
            this.P = p;
            this.U = u;

            this.Values = new List<Value>();
            this.Values.Add(new Value(Guid.NewGuid().ToString(),11));
            this.Values.Add(new Value(Guid.NewGuid().ToString(), 12));
        }

        public string Id { get;  set; }
        public int M { get;  set; }
        public int P { get;  set; }
        public int U { get;  set; }

        public List<Value> Values { get;  set; }

    }

    public class Value
    {
        public Value()
        {
            
        }

        public Value(string name, int price)
        {
            this.Name = name;
            this.Price = price;
        }

        public string Name { get;  set; }
        public int Price { get;  set; }
    }
}
