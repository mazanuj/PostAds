
namespace Motorcycle.XmlWorker
{
    public class Value
    {
        public Value()
        {

        }

        public Value(string name, string val)
        {
            this.Name = name;
            this.Val = val;
        }

        public string Name { get; set; }
        public string Val { get; set; }
    }
}
