using System.Collections.Generic;

namespace Motorcycle.Config.Data
{
    internal class DvaKolesaData : ISiteData
    {
        public DicHolder GetMoto(string row)
        {
            var data = row.Split('\t');

            return new DicHolder
            {
                DataDictionary = new Dictionary<string, string>
                {
                    {"model", "620"},
                    {"modification", "BMW 745"},
                    {"color", "36"},
                    {"price", "345"},
                    {"currency", "1"},
                    {"year", "2014"},
                    {"bodytype", "48"},
                    {"year_real", "2014"},
                    {"engine", "1"},
                    {"mileage", "4500"},
                    {"mileage_unit", "0"},
                    {"volume", "1233"},
                    {"state", "10"},
                    {"choosen", "no"},
                    {"name", "maza"},
                    {"phone1", "0981474531"},
                    {"since1", "10"},
                    {"till1", "21"},
                    {"since2", "10"},
                    {"till2", "21"},
                    {"city", "22"},
                    {"during", "360"},
                    {"vendor", "620"},
                    {"category", "1"},
                    {"ip", "127.0.0.1"},
                    {"Itemid", "2"},
                    {"option", "com_autobb"},
                    {"task", "save"}
                },

                FileDictionary = new Dictionary<string, string> { { "photofile_0", "virginia.jpg" } }
            };
        }

        public DicHolder GetSpare(string row)
        {
            var data = row.Split('\t');

            return new DicHolder
            {
                DataDictionary = new Dictionary<string, string>
                {
                    {"", ""}
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"", ""}
                }
            };
        }

        public DicHolder GetEquip(string row)
        {
            var data = row.Split('\t');

            return new DicHolder
            {
                DataDictionary = new Dictionary<string, string>
                {
                    {"", ""}
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"", ""}
                }
            };
        }
    }
}