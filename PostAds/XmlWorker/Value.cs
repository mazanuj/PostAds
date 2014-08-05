
namespace Motorcycle.XmlWorker
{
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

        public string Name { get; set; }
        public int Price { get; set; }
    }
}
