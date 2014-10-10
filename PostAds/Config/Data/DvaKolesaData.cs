using Motorcycle.XmlWorker;

namespace Motorcycle.Config.Data
{
    using System.Collections.Generic;
    using Interfaces;

    internal class DvaKolesaData : ISiteData
    {
        public DicHolder GetMoto(string row, int lineNum)
        {
            var data = row.Split('\t');

            //Check
            if (RemoveEntries.DataError("type", ManufactureXmlWorker.GetMotoType(data[10], "p"), row, lineNum,
                    SiteEnum.Proday2Kolesa, ProductEnum.Motorcycle) ||
                RemoveEntries.DataError("city", CityXmlWorker.GetItemSiteValueUsingCity(data[12], "p"), row, lineNum,
                    SiteEnum.Proday2Kolesa, ProductEnum.Motorcycle) ||
                RemoveEntries.DataError("manufacture", ManufactureXmlWorker.GetItemSiteValueUsingPlant(data[4], "p"),
                    row, lineNum, SiteEnum.Proday2Kolesa, ProductEnum.Motorcycle) ||
                RemoveEntries.DataError("color", ManufactureXmlWorker.GetMotoColor(data[16], "p"),
                    row, lineNum, SiteEnum.Proday2Kolesa, ProductEnum.Motorcycle) ||
                RemoveEntries.DataError("color", ManufactureXmlWorker.GetConditionState(data[17], "p"),
                    row, lineNum, SiteEnum.Proday2Kolesa, ProductEnum.Motorcycle))
                return new DicHolder {IsError = true};
            //==========================================================================================//

            //Photos
            var d = data[13].Split(',');
            var files = new string[5];
            for (var i = 0; i < 5; i++)
            {
                if (i < d.Length)
                    files[i] = FilePathXmlWorker.GetFilePath("photo") + d[i];
                else files[i] = string.Empty;
            }

            return new DicHolder
            {
                Site = SiteEnum.Proday2Kolesa,
                Type = ProductEnum.Motorcycle,
                Row = row,
                LineNum = lineNum,
                DataDictionary = new Dictionary<string, string>
                {
                    {"model", ManufactureXmlWorker.GetItemSiteValueUsingPlant(data[4], "p")}, //zavod BASA+
                    {"modification", data[5]}, //Vvod+
                    {"color", ManufactureXmlWorker.GetMotoColor(data[16], "p")}, //Create basa+
                    {"price", data[7]}, //vvod+
                    {"currency", "2"}, //$+
                    {"year", data[9]}, //vvod+
                    {"bodytype", ManufactureXmlWorker.GetMotoType(data[10], "p")}, //type basa+
                    {"year_real", data[9]}, //vvod+
                    {"engine", DocsXmlWorker.GetItemInfo(data[6], "p")}, //doki basa+
                    {"mileage", data[8]}, //probeg vvod+
                    {"mileage_unit", "0"}, //km+
                    {"volume", data[11]}, //objem vvod+
                    {"state", ManufactureXmlWorker.GetConditionState(data[17], "p")}, //sostoyanie basa+
                    {"choosen", "no"}, //obmen +
                    {"name", data[0]}, //name vvod+
                    {"phone1", data[2]}, //phone vvod+
                    {"since1", "10"}, //s 10+
                    {"till1", "21"}, //do 21+
                    {"wrangle", "0"}, //torg+
                    {"MAX_FILE_SIZE", "614400"}, //+
                    {"since2", "10"}, //+
                    {"till2", "21"}, //+
                    {"city", CityXmlWorker.GetItemSiteValueUsingCity(data[12], "p")}, //gorod basa+
                    {"during", "360"}, //publikovat' god+
                    {"vendor", ManufactureXmlWorker.GetItemSiteValueUsingPlant(data[4], "p")}, //zavod basa+
                    {"category", "1"}, //+
                    {"additional", data[14]},
                    {"ip", "127.0.0.1"}, //+
                    {"Itemid", "2"}, //+
                    {"option", "com_autobb"}, //+
                    {"task", "save"} //+
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"photofile_0", files[0]},
                    {"photofile_1", files[1]},
                    {"photofile_2", files[2]},
                    {"photofile_3", files[3]},
                    {"photofile_4", files[4]}
                }
            };
        }

        public DicHolder GetSpare(string row, int lineNum)
        {
            var data = row.Split('\t');

            //Check
            if (RemoveEntries.DataError("manufacture", SpareEquipXmlWorker.GetItemSiteValueUsingPlant(data[4], "pz"),
                    row, lineNum, SiteEnum.Proday2Kolesa, ProductEnum.Spare) ||
                RemoveEntries.DataError("type", SpareEquipXmlWorker.GetSpareType(data[5], "p"), row, lineNum,
                    SiteEnum.Proday2Kolesa, ProductEnum.Spare) ||
                RemoveEntries.DataError("condition", ManufactureXmlWorker.GetConditionState(data[10], "p"), row, lineNum,
                    SiteEnum.Proday2Kolesa, ProductEnum.Spare) ||
                RemoveEntries.DataError("city", CityXmlWorker.GetItemSiteValueUsingCity(data[7], "p"), row, lineNum,
                    SiteEnum.Proday2Kolesa, ProductEnum.Spare))
                return new DicHolder {IsError = true};
            //====================================================================================//

            //Photos
            var d = data[8].Split(',');
            var files = new string[5];
            for (var i = 0; i < 5; i++)
            {
                if (i < d.Length)
                    files[i] = FilePathXmlWorker.GetFilePath("photo") + d[i];
                else files[i] = string.Empty;
            }

            return new DicHolder
            {
                Site = SiteEnum.Proday2Kolesa,
                Type = ProductEnum.Spare,
                Row = row,
                LineNum = lineNum,
                DataDictionary = new Dictionary<string, string>
                {
                    {"model", SpareEquipXmlWorker.GetItemSiteValueUsingPlant(data[4], "pz")}, //zavod basa+
                    {"modification", data[3]}, //model vvod+
                    {"color", "0"}, //+
                    {"price", data[6]}, //cena vvod+
                    {"currency", "2"}, //$+
                    {"year", "-----"}, //+
                    {"bodytype", SpareEquipXmlWorker.GetSpareType(data[5], "p")}, //tip basa+
                    {"state", ManufactureXmlWorker.GetConditionState(data[10], "p")}, //sostoyanie basa+
                    {"choosen", "no"}, //+
                    {"vin", ""}, //+
                    {"wrangle", "0"}, //+
                    {"MAX_FILE_SIZE", "614400"}, //+
                    {"additional", data[9]}, //message vvod+
                    {"name", data[0]}, //vvod+
                    {"phone1", data[2]}, //vvod+
                    {"since1", "10"}, //+
                    {"till1", "21"}, //+
                    {"city", CityXmlWorker.GetItemSiteValueUsingCity(data[7], "p")}, //gorod basa+
                    {"during", "360"}, //+
                    {"id", ""}, //+
                    {"vendor", SpareEquipXmlWorker.GetItemSiteValueUsingPlant(data[4], "pz")}, //zavod basa+
                    {"category", "5"}, //+
                    {"ip", "127.0.0.1"}, //+
                    {"Itemid", "6"}, //+
                    {"option", "com_autobb"}, //+
                    {"task", "save"} //+
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"photofile_0", files[0]},
                    {"photofile_1", files[1]},
                    {"photofile_2", files[2]},
                    {"photofile_3", files[3]},
                    {"photofile_4", files[4]}
                }
            };
        }

        public DicHolder GetEquip(string row, int lineNum)
        {
            var data = row.Split('\t');

            //Check
            if (RemoveEntries.DataError("type", SpareEquipXmlWorker.GetEquipType(data[4], "p"), row, lineNum,
                SiteEnum.Proday2Kolesa, ProductEnum.Equip) ||
                RemoveEntries.DataError("city", CityXmlWorker.GetItemSiteValueUsingCity(data[7], "p"), row, lineNum,
                    SiteEnum.Proday2Kolesa, ProductEnum.Equip) ||
                RemoveEntries.DataError("manufacture", SpareEquipXmlWorker.GetItemSiteValueUsingPlant(data[5], "pe"),
                    row, lineNum, SiteEnum.Proday2Kolesa, ProductEnum.Equip) ||
                RemoveEntries.DataError("condition", ManufactureXmlWorker.GetConditionState(data[11], "p"), row, lineNum,
                    SiteEnum.Proday2Kolesa, ProductEnum.Equip))
                return new DicHolder {IsError = true};
            //====================================================================================//

            //Photos
            var d = data[8].Split(',');
            var files = new string[5];
            for (var i = 0; i < 5; i++)
            {
                if (i < d.Length)
                    files[i] = FilePathXmlWorker.GetFilePath("photo") + d[i];
                else files[i] = string.Empty;
            }

            return new DicHolder
            {
                Site = SiteEnum.Proday2Kolesa,
                Type = ProductEnum.Equip,
                Row = row,
                LineNum = lineNum,
                DataDictionary = new Dictionary<string, string>
                {
                    {"model", SpareEquipXmlWorker.GetItemSiteValueUsingPlant(data[5], "pe")}, //+
                    {"modification", data[3]}, //+
                    {"color", "0"}, //+
                    {"price", data[6]}, //+
                    {"currency", "2"}, //$+
                    {"year", data[10]}, //+
                    {"bodytype", SpareEquipXmlWorker.GetEquipType(data[4], "p")}, //+
                    {"state", ManufactureXmlWorker.GetConditionState(data[11], "p")}, //+
                    {"choosen", "no"}, //+
                    {"vin", ""}, //+
                    {"wrangle", "0"}, //+
                    {"MAX_FILE_SIZE", "614400"}, //+
                    {"additional", data[9]}, //+
                    {"name", data[0]}, //+
                    {"phone1", data[2]}, //+
                    {"since1", "10"}, //+
                    {"till1", "21"}, //+
                    {"city", CityXmlWorker.GetItemSiteValueUsingCity(data[7], "p")}, //+
                    {"during", "360"}, //+
                    {"id", ""}, //+
                    {"vendor", SpareEquipXmlWorker.GetItemSiteValueUsingPlant(data[5], "pe")}, //+
                    {"category", "4"}, //+
                    {"ip", "127.0.0.1"}, //+
                    {"Itemid", "5"}, //+
                    {"option", "com_autobb"}, //+
                    {"task", "save"} //+
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"photofile_0", files[0]},
                    {"photofile_1", files[1]},
                    {"photofile_2", files[2]},
                    {"photofile_3", files[3]},
                    {"photofile_4", files[4]}
                }
            };
        }
    }
}