
namespace Motorcycle.XmlWorker
{
    public class Value
    {
        public Value()
        {
        }

        public Value(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; set; }
        public int Price { get; set; }
    }
}