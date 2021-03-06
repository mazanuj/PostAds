﻿using System.IO;
using NLog;

namespace Motorcycle.Config.Data
{
    using System.Collections.Generic;
    using XmlWorker;
    using Interfaces;

    internal class MotosaleData : ISiteData
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public DicHolder GetMoto(string row, int lineNum)
        {
            var data = row.Split('\t');

            //Check
            if (RemoveEntries.DataError("type", ManufactureXmlWorker.GetMotoType(data[10], "m"), row, lineNum,
                SiteEnum.MotoSale, ProductEnum.Motorcycle) ||
                RemoveEntries.DataError("city", CityXmlWorker.GetItemSiteValueUsingCity(data[12], "m"), row, lineNum,
                    SiteEnum.MotoSale, ProductEnum.Motorcycle) ||
                RemoveEntries.DataError("manufacture", ManufactureXmlWorker.GetItemSiteValueUsingPlant(data[4], "m"),
                    row, lineNum, SiteEnum.MotoSale, ProductEnum.Motorcycle))
                return new DicHolder {IsError = true};
            //==========================================================================================//

            //Photos
            var d = data[13].Split(',');
            var files = new string[10];
            for (var i = 0; i < 10; i++)
            {
                if (i < d.Length)
                {
                    files[i] = FilePathXmlWorker.GetFilePath("photo") + d[i];
                    if (File.Exists(files[i])) continue;

                    Log.Warn(d[i] + " not exists", SiteEnum.MotoSale, ProductEnum.Motorcycle);
                    files[i] = string.Empty;
                }

                else files[i] = string.Empty;
            }

            var model = ManufactureXmlWorker.GetItemValueUsingPlantAndName(data[4], data[5]);
            var customModel = model != string.Empty ? string.Empty : data[5];

            return new DicHolder
            {
                Site = SiteEnum.MotoSale,
                Type = ProductEnum.Motorcycle,
                Row = row,
                LineNum = lineNum,
                DataDictionary = new Dictionary<string, string>
                {
                    {"name", data[0]}, //+
                    {"mail", data[1]}, //+
                    {"phone", data[2]}, //+
                    {"header", data[3]}, //+
                    {"type_obj", "1"}, //Sell|Buy+
                    {"model", ManufactureXmlWorker.GetItemSiteValueUsingPlant(data[4], "m")}, //Proizvoditel'+
                    {"manufactured_model", model}, //Model+
                    {"custom_model", customModel}, //Custom Model+
                    {"docum", DocsXmlWorker.GetItemInfo(data[6], "m")}, //Документы+
                    {"price", data[7]}, //цена+
                    {"run", data[8]}, //пробег+
                    {"in", data[9]}, //год выпуска+
                    {"moto", ManufactureXmlWorker.GetMotoType(data[10], "m")}, //тип транс средства+
                    {"objem", data[11]}, //+
                    {"param[tip_transmissii][]", data[18]}, //+
                    {"param[transnision]", data[19]}, //+
                    {"city", CityXmlWorker.GetItemSiteValueUsingCity(data[12], "m")}, //+
                    {"youtube", data[15]}, //+
                    {"text", data[14]}, //+
                    {"date_delete", "60"}, //+
                    {"fConfirmationCode", ""}, //captcha+
                    {"insert", ""} //+
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"filename", files[0]},
                    {"filename2", files[1]},
                    {"filename3", files[2]},
                    {"filename4", files[3]},
                    {"filename5", files[4]},
                    {"filename6", files[5]},
                    {"filename7", files[6]},
                    {"filename8", files[7]},
                    {"filename9", files[8]},
                    {"filename10", files[9]}
                }
            };
        }

        public DicHolder GetSpare(string row, int lineNum)
        {
            var data = row.Split('\t');

            //Check
            if (
                //RemoveEntries.DataError("manufacture", ManufactureXmlWorker.GetItemSiteValueUsingPlant(data[4], "m"),
                //    row, lineNum, SiteEnum.MotoSale, ProductEnum.Spare) ||
                RemoveEntries.DataError("type", SpareEquipXmlWorker.GetSpareType(data[5], "m"), row, lineNum,
                    SiteEnum.MotoSale, ProductEnum.Spare) ||
                RemoveEntries.DataError("city", CityXmlWorker.GetItemSiteValueUsingCity(data[7], "m"), row, lineNum,
                    SiteEnum.MotoSale, ProductEnum.Spare))
                return new DicHolder {IsError = true};
            //====================================================================================//

            //Photos
            var d = data[8].Split(',');
            var files = new string[10];
            for (var i = 0; i < 10; i++)
            {
                if (i < d.Length)
                {
                    files[i] = FilePathXmlWorker.GetFilePath("photo") + d[i];
                    if (File.Exists(files[i])) continue;

                    Log.Warn(d[i] + " not exists", SiteEnum.MotoSale, ProductEnum.Spare);
                    files[i] = string.Empty;
                }
                else files[i] = string.Empty;
            }

            return new DicHolder
            {
                Site = SiteEnum.MotoSale,
                Type = ProductEnum.Spare,
                Row = row,
                LineNum = lineNum,
                DataDictionary = new Dictionary<string, string>
                {
                    {"name", data[0]}, //+
                    {"mail", data[1]}, //+
                    {"phone", data[2]}, //+
                    {"type_obj", "1"}, //+
                    {"model_zap", ManufactureXmlWorker.GetItemSiteValueUsingPlant(data[4], "m")}, //+
                    {"type", SpareEquipXmlWorker.GetSpareType(data[5], "m")}, //+
                    {"header", data[3]}, //+
                    {"text", data[9]}, //+
                    {"price", data[6]}, //+
                    {"city", CityXmlWorker.GetItemSiteValueUsingCity(data[7], "m")}, //+
                    {"date_delete", "60"}, //+
                    {"fConfirmationCode", ""}, //+
                    {"ok", "Отправить"}, //+
                    {"insert", ""} //+
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"filename", files[0]},
                    {"filename2", files[1]},
                    {"filename3", files[2]},
                    {"filename4", files[3]},
                    {"filename5", files[4]},
                    {"filename6", files[5]},
                    {"filename7", files[6]},
                    {"filename8", files[7]},
                    {"filename9", files[8]},
                    {"filename10", files[9]}
                }
            };
        }

        public DicHolder GetEquip(string row, int lineNum)
        {
            var data = row.Split('\t');

            //Check
            if (RemoveEntries.DataError("type", SpareEquipXmlWorker.GetEquipType(data[4], "m"), row, lineNum,
                SiteEnum.MotoSale, ProductEnum.Equip) ||
                RemoveEntries.DataError("city", CityXmlWorker.GetItemSiteValueUsingCity(data[7], "m"), row, lineNum,
                    SiteEnum.MotoSale, ProductEnum.Equip))
                return new DicHolder {IsError = true};
            //====================================================================================//

            //Photos
            var d = data[8].Split(',');
            var files = new string[10];
            for (var i = 0; i < 10; i++)
            {
                if (i < d.Length)
                {
                    files[i] = FilePathXmlWorker.GetFilePath("photo") + d[i];
                    if (File.Exists(files[i])) continue;

                    Log.Warn(d[i] + " not exists", SiteEnum.MotoSale, ProductEnum.Equip);
                    files[i] = string.Empty;
                }
                else files[i] = string.Empty;
            }

            var model = string.IsNullOrEmpty(data[5]) ? "другой.." : data[5].ToUpper();

            return new DicHolder
            {
                Site = SiteEnum.MotoSale,
                Type = ProductEnum.Equip,
                Row = row,
                LineNum = lineNum,
                DataDictionary = new Dictionary<string, string>
                {
                    {"name", data[0]}, //+
                    {"mail", data[1]}, //+
                    {"phone", data[2]}, //+
                    {"type_obj", "1"}, //+
                    {"type", SpareEquipXmlWorker.GetEquipType(data[4], "m")}, //vid+
                    {"brand", model}, //proizvoditel'+
                    {"header", data[3]}, //+
                    {"text", data[9]}, //+
                    {"price", data[6]}, //+
                    {"city", CityXmlWorker.GetItemSiteValueUsingCity(data[7], "m")}, //+
                    {"date_delete", "60"}, //+
                    {"fConfirmationCode", ""}, //+
                    {"insert", ""} //+
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"filename", files[0]},
                    {"filename2", files[1]},
                    {"filename3", files[2]},
                    {"filename4", files[3]},
                    {"filename5", files[4]},
                    {"filename6", files[5]},
                    {"filename7", files[6]},
                    {"filename8", files[7]},
                    {"filename9", files[8]},
                    {"filename10", files[9]}
                }
            };
        }
    }
}