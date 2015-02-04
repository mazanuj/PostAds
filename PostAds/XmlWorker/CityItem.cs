namespace Motorcycle.XmlWorker
{
    public class CityItem
    {
        public CityItem()
        {
        }

        public CityItem(string cityName, string m, string p, string u, string o)
        {
            CityName = cityName;
            M = m;
            P = p;
            U = u;
            O = o;
        }

        public string CityName { get; set; }
        public string M { get; set; }
        public string P { get; set; }
        public string U { get; set; }
        public string O { get; set; }
    }
}