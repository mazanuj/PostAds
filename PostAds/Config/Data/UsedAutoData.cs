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
                    {"act", "add"}, //+++
                    {"rid", "3"}, //city BASA
                    {"input[1]", "19"}, //Zavod  BASA
                    {"input[153]", "model"}, //Model вводится с файла
                    {"input[3]", "1"}, //Year
                    {"price", "1000"}, //Cena
                    {"currency", "1"}, //$
                    {"input[4]", "5555"}, //Probeg
                    {"input[200]", "600"}, //Objem
                    //{"input[8]", ""},//Moschnost' ne nuzhna
                    {"input[18]", "1"}, //tip dvigatelya  BASA
                    {"input[61]", "1"}, //ohlazhdenie  BASA
                    {"input[6]", "1"}, //tip transmissii  если механика == 3, в противном случае ничего
                    {"input[5]", "color"}, //color
                    //{"input[196]", ""},// max speed   ne nuzhna
                    {"input[194]", "1"}, //sostoyanie
                    //{"input[15]", "on"},//?
                    //{"input[151]", "on"},//?
                    //{"input[219]", "on"},//?
                    {"description", "message"}, //message
                    {"photos", ""}, //+
                    {"main_photo", ""}, //+
                    {"user[0]", "name"}, //name  
                    {"user[1]", "380676776767"}, //phone
                    //{"user[5]", ""}//mail ne nuzhen
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