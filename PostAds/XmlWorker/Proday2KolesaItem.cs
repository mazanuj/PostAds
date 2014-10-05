namespace Motorcycle.XmlWorker
{
    public class Proday2KolesaItem
    {
        public Proday2KolesaItem()
        {
        }

        public Proday2KolesaItem(string id, string s, string e)
        {
            Id = id;
            S = s;
            E = e;
        }

        public string Id { get; set; }
        public string S { get; set; }
        public string E { get; set; }
    }
}
