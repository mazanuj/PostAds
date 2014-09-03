using System.Collections.Generic;

namespace Motorcycle.Config.Data
{
    internal class UsedAutoData : ISiteData
    {
        public DicHolder GetMoto(string row)
        {
            var data = row.Split('\t');
            var d = data[13].Split(',');
            var files = new string[8];
            for (var i = 0; i < 8; i++)
            {
                if (i < d.Length)
                    files[i] = d[i];
                else files[i] = string.Empty;
            }

            return new DicHolder
            {
                DataDictionary = new Dictionary<string, string>
                {
                    {"cid", "83"},
                    {"act", "add"},
                    {"rid", "3"},//city
                    {"input[1]", "19"},//Zavod
                    {"input[153]", "model"},//Model
                    {"input[3]", "1"},//Year
                    {"price", "1000"},//Cena
                    {"currency", "1"},//$
                    {"input[4]", "5555"},//Probeg
                    {"input[200]", "600"},//Objem
                    {"input[8]", ""},//Moschnost' ne nuzhna
                    {"input[18]", "1"},
                    {"input[61]", "1"},
                    {"input[6]", "1"},
                    {"input[5]", "color"},
                    {"input[194]", "1"},
                    {"input[15]", "on"},
                    {"input[151]", "on"},
                    {"input[219]", "on"},
                    {"description", "message"},
                    {"photos", ""},
                    {"main_photo", ""},
                    {"user[0]", "name"},
                    {"user[1]", "380676776767"},
                    {"user[2]", ""},
                    {"user[3]", ""},
                    {"user[4]", ""},
                    {"user[5]", "mail@ma.ma"}
                },
                FileDictionary = new Dictionary<string, string> {{"file", "virginia.jpg"}}
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