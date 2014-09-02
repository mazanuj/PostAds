
namespace Motorcycle.XmlWorker
{
    public class CityItem
    {
        public CityItem()
        {

        }

        public CityItem(string cityName, string m, string p, string u)
        {
            this.CityName = cityName;
            this.M = m;
            this.P = p;
            this.U = u;
        }

        public string CityName { get; set; }
        public string M { get; set; }
        public string P { get; set; }
        public string U { get; set; }

    }
}
