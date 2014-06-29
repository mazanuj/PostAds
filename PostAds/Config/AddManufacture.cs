namespace Motorcycle.Config
{
    struct AddManufacture
    {
        public AddManufacture(string id, string m, string p, string u)
        {
            _id = id;
            _m = m;
            _p = p;
            _u = u;
        }

        public AddManufacture(string id, string m)
        {
            _id = id;
            _m = m;
            _p = string.Empty;
            _u = string.Empty;
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _m;
        public string M
        {
            get { return _m; }
            set { _m = value; }
        }

        private string _p;
        public string P
        {
            get { return _p; }
            set { _p = value; }
        }

        private string _u;
        public string U
        {
            get { return _u; }
            set { _u = value; }
        }
    }
}