
namespace Motorcycle.Config.Data
{
    using System.Collections.Generic;
    using Motorcycle.Interfaces;

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
                    {"cid", "83"}, //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    {"act", "add"}, //+++
                    {"rid", XmlWorker.CityXmlWorker.GetItemSiteValueUsingCity(data[12], "u")}, //city+
                    {"input[1]", XmlWorker.ManufactureXmlWorker.GetItemSiteValueUsingPlant(data[4], "u")}, //Zavod+
                    {"input[153]", data[5]}, //Model вводится с файла+
                    {"input[3]", data[9]}, //Year+
                    {"price", data[7]}, //Cena+
                    {"currency", "1"}, //$+
                    {"input[4]", data[8]}, //Probeg+
                    {"input[200]", data[11]}, //Objem+
                    //{"input[8]", ""},//Moschnost' ne nuzhna
                    {"input[18]", "1"},
                    //tip dvigatelya  BASA!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    {"input[61]", "1"},
                    //ohlazhdenie  BASA!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    {"input[6]", "1"},
                    //tip transmissii  если механика == 3, в противном случае ничего!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    {"input[5]", data[16]}, //color+
                    //{"input[196]", ""},// max speed   ne nuzhna
                    {"input[194]", "1"},
                    //sostoyanie BASA!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    //{"input[15]", "on"},//?ABS
                    //{"input[151]", "on"},//?Torg
                    //{"input[219]", "on"},//?Exchange
                    {"description", data[14]}, //message+
                    {"photos", ""}, //+
                    {"main_photo", ""}, //+
                    {"user[0]", data[0]}, //name+
                    {"user[1]", data[2]} //phone+
                    //{"user[5]", ""}//mail ne nuzhen
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"file", files[0]},
                    {"file", files[1]},
                    {"file", files[2]},
                    {"file", files[3]},
                    {"file", files[4]},
                    {"file", files[5]},
                    {"file", files[6]},
                    {"file", files[7]}
                }
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