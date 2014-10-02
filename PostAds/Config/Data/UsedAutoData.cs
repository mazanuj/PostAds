using Motorcycle.XmlWorker;

namespace Motorcycle.Config.Data
{
    using System.Collections.Generic;
    using Interfaces;

    internal class UsedAutoData : ISiteData
    {
        public DicHolder GetMoto(string row, int lineNum)
        {
            var data = row.Split('\t');

            //Photos
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
            if (data[18].ToLower().StartsWith("Мех"))
                transType = "3";

            return new DicHolder
            {
                Row = row,
                LineNum = lineNum,
                DataDictionary = new Dictionary<string, string>
                {
                    {"cid", ManufactureXmlWorker.GetMotoType(data[10], "u")}, //+
                    {"act", "add"}, //+++
                    {"rid", CityXmlWorker.GetItemSiteValueUsingCity(data[12], "u")}, //city+
                    {"input[1]", ManufactureXmlWorker.GetItemSiteValueUsingPlant(data[4], "u")}, //Zavod+
                    {"input[153]", data[5]}, //Model вводится с файла+
                    {"input[3]", ManufactureXmlWorker.GetMadeYear(data[9],"u")}, //Year+
                    {"price", data[7]}, //Cena+
                    {"currency", "1"}, //$+
                    {"input[4]", data[8]}, //Probeg+
                    {"input[200]", data[11]}, //Objem+
                    {"input[18]", engine}, //tip dvigatelya+
                    {"input[61]", cooling}, //ohlazhdenie+
                    {"input[6]", transType}, //tip transmissii+
                    {"input[5]", data[16]}, //color+
                    {"input[194]", ManufactureXmlWorker.GetConditionState(data[17],"u")}, //sostoyanie+
                    {"description", data[14]}, //message+
                    {"photos", ""}, //+
                    {"main_photo", ""}, //+
                    {"user[0]", data[0]}, //name+
                    {"user[1]", data[2]} //phone+
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"file", files[0]},
                    {"file1", files[1]},
                    {"file2", files[2]},
                    {"file3", files[3]},
                    {"file4", files[4]},
                    {"file5", files[5]},
                    {"file6", files[6]},
                    {"file7", files[7]}
                }
            };
        }

        public DicHolder GetSpare(string row, int lineNum)
        {
            var data = row.Split('\t');

            var condition = data[10].ToLower() == "новый" ? "new" : "used";

            return new DicHolder
            {
                Row = row,
                LineNum = lineNum,
                DataDictionary = new Dictionary<string, string>
                {
                    {"hash", "d8d4c90043a5a7fc299fc47610c59184"},//+
                    {"category_id", "80"},//+
                    {"parentId", "27"},//+
                    {"make", ManufactureXmlWorker.GetItemSiteValueUsingPlant(data[4],"u")},//zavod+
                    {"model", data[3]},//+
                    {"year", ""},//+
                    {"part_category_id[]", SpareEquipXmlWorker.GetSpareType(data[5],"u")},//create basa+
                    {"part_description[]", data[3]},//+
                    {"part_condition[]", condition},//+`
                    {"part_price[]", data[6]},//+
                    {"part_currency[]", "1"},//$+
                    {"description", data[9]},//+
                    {"region_id", CityXmlWorker.GetItemSiteValueUsingCity(data[7],"u")},//+
                    {"phone1", data[2]},//+
                    {"fio", data[0]}//+
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"part_photo[]", data[8].Split(',')[0]}//+
                }
            };
        }

        public DicHolder GetEquip(string row, int lineNum)
        {
            var data = row.Split('\t');

            return new DicHolder
            {
                Row = row,
                LineNum = lineNum,
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