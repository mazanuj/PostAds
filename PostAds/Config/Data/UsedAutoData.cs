using Motorcycle.XmlWorker;

namespace Motorcycle.Config.Data
{
    using System.Collections.Generic;
    using Interfaces;

    internal class UsedAutoData : ISiteData
    {
        public DicHolder GetMoto(string row)
        {
            //Photos
            var data = row.Split('\t');
            var d = data[13].Split(',');
            var files = new string[8];
            for (var i = 0; i < 8; i++)
            {
                if (i < d.Length)
                    files[i] = d[i];
                else files[i] = string.Empty;
            }

            //Engine type
            var engine = string.Empty;
            switch (data[20].ToLower())
            {
                case "к2":
                    engine = "1";
                    break;
                case "к4":
                    engine = "2";
                    break;
                case "и4":
                    engine = "3";
                    break;
            }

            //Cooling
            var cooling = string.Empty;
            switch (data[21].ToLower())
            {
                case "воздушное":
                    cooling = "1";
                    break;
                case "жидкостное":
                    cooling = "2";
                    break;
            }

            //Transmition type
            var transType = string.Empty;
            if (data[18].ToLower().StartsWith("мех"))
                transType = "3";

            return new DicHolder
            {
                DataDictionary = new Dictionary<string, string>
                {
                    {"cid", ManufactureXmlWorker.GetMotoType(data[10], "u")}, //+
                    {"act", "add"}, //+++
                    {"rid", CityXmlWorker.GetItemSiteValueUsingCity(data[12], "u")}, //city+
                    {"input[1]", ManufactureXmlWorker.GetItemSiteValueUsingPlant(data[4], "u")}, //Zavod+
                    {"input[153]", data[5]}, //Model вводится с файла+
                    {"input[3]", data[9]}, //Year+
                    {"price", data[7]}, //Cena+
                    {"currency", "1"}, //$+
                    {"input[4]", data[8]}, //Probeg+
                    {"input[200]", data[11]}, //Objem+
                    //{"input[8]", ""},//Moschnost' ne nuzhna
                    {"input[18]", engine}, //tip dvigatelya+
                    {"input[61]", cooling}, //ohlazhdenie+
                    {"input[6]", transType}, //tip transmissii+
                    {"input[5]", data[16]}, //color+
                    //{"input[196]", ""},// max speed   ne nuzhna
                    {"input[194]", ManufactureXmlWorker.GetConditionState(data[17],"u")}, //sostoyanie+
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