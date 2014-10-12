namespace Motorcycle.XmlWorker
{
    public class SpareEquipItem
    {
        public SpareEquipItem(string id, string pz, string pe)
        {
            Id = id;
            Pz = pz;
            Pe = pe;
        }

        public string Id { get; set; }
        public string Pz { get; set; }
        public string Pe { get; set; }
    }
}