using System.Collections.Generic;
using System.IO;
using Motorcycle.Interfaces;
using Motorcycle.XmlWorker;
using NLog;

namespace Motorcycle.Config.Data
{
    internal class OlxData : ISiteData
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public DicHolder GetMoto(string row, int lineNum)
        {
            var data = row.Split('\t');

            //Check
            if (RemoveEntries.DataError("city", CityXmlWorker.GetItemSiteValueUsingCity(data[12], "o"), row, lineNum,
                SiteEnum.Olx, ProductEnum.Motorcycle))
                return new DicHolder {IsError = true};
            //====================================================================================//

            //Photos
            var d = data[13].Split(',');
            var files = new string[8];
            for (var i = 0; i < 8; i++)
            {
                if (i < d.Length)
                {
                    files[i] = FilePathXmlWorker.GetFilePath("photo") + d[i];
                    if (File.Exists(files[i])) continue;

                    Log.Warn(d[i] + " not exists", SiteEnum.Olx, ProductEnum.Motorcycle);
                    files[i] = string.Empty;
                }
                else files[i] = string.Empty;
            }

            var city = CityXmlWorker.GetItemSiteValueUsingCity(data[12], "o").Split(':');
            var manufacture = ManufactureXmlWorker.GetItemSiteValueUsingPlant(data[4], "o");
            var type = ManufactureXmlWorker.GetMotoType(data[10], "o");

            return new DicHolder
            {
                Site = SiteEnum.Olx,
                Type = ProductEnum.Motorcycle,
                Row = row,
                LineNum = lineNum,
                DataDictionary = new Dictionary<string, string>
                {
                    {"data[title]", data[3]}, //=+
                    {"data[category_id]", "264"}, //=+
                    {"data[offer_seek]", string.Empty}, //=+
                    {"data[param_price][0]", "price"}, //=+
                    {"data[param_price][1]", data[7]}, //=+
                    {"data[param_price][currency]", "USD"}, //=+
                    {"data[param_bike_manufacturer]", string.IsNullOrEmpty(manufacture) ? "766" : manufacture}, //=+
                    {"data[param_mileage]", data[8]}, //+
                    {"data[param_year_of_manufacture]", data[9]}, //+
                    {"data[param_engine_volume]", data[11]}, //+
                    {"data[param_currency]", string.Empty}, //=+
                    {"data[param_body_type]", string.IsNullOrEmpty(type) ? "627" : type}, //=+
                    {"data[private_business]", "private"}, //=+
                    {"data[description]", data[14]}, //=+
                    {"data[region_id]", city[0]}, //=+
                    {"data[person]", data[0]}, //=+
                    {"data[riak_key]", ""}, //=+
                    {"data[adding_key]", ""}, //=+
                    {"data[subregion_id]", city[1]}, //=+
                    {"data[contactform]", "on"}, //TODO Не получать ответы на email
                    {"data[phone]", data[2]}, //=+
                    {"data[suggested_categories][]", "96"} //?
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"myfile", files[0]},
                    {"myfile2", files[1]},
                    {"myfile3", files[2]},
                    {"myfile4", files[3]},
                    {"myfile5", files[4]},
                    {"myfile6", files[5]},
                    {"myfile7", files[6]},
                    {"myfile8", files[7]}
                }
            };
        }

        public DicHolder GetSpare(string row, int lineNum)
        {
            var data = row.Split('\t');

            //Check
            if ( /*RemoveEntries.DataError("type", SpareEquipXmlWorker.GetEquipType(data[4], "m"), row, lineNum,
                SiteEnum.MotoSale, ProductEnum.Equip) ||*/
                RemoveEntries.DataError("city", CityXmlWorker.GetItemSiteValueUsingCity(data[7], "o"), row, lineNum,
                    SiteEnum.Olx, ProductEnum.Spare))
                return new DicHolder {IsError = true};
            //====================================================================================//

            //Photos
            var d = data[8].Split(',');
            var files = new string[8];
            for (var i = 0; i < 8; i++)
            {
                if (i < d.Length)
                {
                    files[i] = FilePathXmlWorker.GetFilePath("photo") + d[i];
                    if (File.Exists(files[i])) continue;

                    Log.Warn(d[i] + " not exists", SiteEnum.Olx, ProductEnum.Spare);
                    files[i] = string.Empty;
                }
                else files[i] = string.Empty;
            }

            var state = data[10].ToLower() == "новый" ? "new" : "used";
            var city = CityXmlWorker.GetItemSiteValueUsingCity(data[7], "o").Split(':');

            return new DicHolder
            {
                Site = SiteEnum.Olx,
                Type = ProductEnum.Spare,
                Row = row,
                LineNum = lineNum,
                DataDictionary = new Dictionary<string, string>
                {
                    {"data[title]", data[3]}, //=
                    {"data[category_id]", "311"}, //=
                    {"data[offer_seek]", string.Empty}, //=
                    {"data[param_price][0]", "price"}, //=
                    {"data[param_price][1]", data[6]}, //=
                    {"data[param_price][currency]", "USD"}, //=
                    {"data[param_state]", state}, //=
                    {"data[param_currency]", string.Empty}, //=
                    {"data[private_business]", "private"}, //=
                    {"data[description]", data[9]}, //=
                    {"data[region_id]", city[0]}, //=
                    {"data[person]", data[0]}, //=
                    {"data[riak_key]", ""}, //=
                    {"data[adding_key]", ""}, //=
                    {"data[subregion_id]", city[1]}, //=
                    {"data[contactform]", "on"}, //TODO Не получать ответы на email
                    {"data[phone]", data[2]}, //=
                    //{"data[suggested_categories][]", "70"},//?
                    //{"data[suggested_categories][]", "541"},//?
                    //{"data[suggested_categories][]", "540"},//?
                    {"data[suggested_categories][]", "96"} //?
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"myfile", files[0]},
                    {"myfile2", files[1]},
                    {"myfile3", files[2]},
                    {"myfile4", files[3]},
                    {"myfile5", files[4]},
                    {"myfile6", files[5]},
                    {"myfile7", files[6]},
                    {"myfile8", files[7]}
                }
            };
        }

        public DicHolder GetEquip(string row, int lineNum)
        {
            var data = row.Split('\t');

            //Check
            if ( /*RemoveEntries.DataError("type", SpareEquipXmlWorker.GetEquipType(data[4], "m"), row, lineNum,
                SiteEnum.MotoSale, ProductEnum.Equip) ||*/
                RemoveEntries.DataError("city", CityXmlWorker.GetItemSiteValueUsingCity(data[7], "o"), row, lineNum,
                    SiteEnum.Olx, ProductEnum.Equip))
                return new DicHolder {IsError = true};
            //====================================================================================//

            //Photos
            var d = data[8].Split(',');
            var files = new string[8];
            for (var i = 0; i < 8; i++)
            {
                if (i < d.Length)
                {
                    files[i] = FilePathXmlWorker.GetFilePath("photo") + d[i];
                    if (File.Exists(files[i])) continue;

                    Log.Warn(d[i] + " not exists", SiteEnum.Olx, ProductEnum.Equip);
                    files[i] = string.Empty;
                }
                else files[i] = string.Empty;
            }

            var model = ManufactureXmlWorker.GetItemSiteValueUsingPlant(data[5], "o");
            var state = data[11].ToLower() == "новый" ? "new" : "used";
            var city = CityXmlWorker.GetItemSiteValueUsingCity(data[7], "o").Split(':');

            return new DicHolder
            {
                Site = SiteEnum.Olx,
                Type = ProductEnum.Equip,
                Row = row,
                LineNum = lineNum,
                DataDictionary = new Dictionary<string, string>
                {
                    {"data[title]", data[3]}, //+
                    {"data[category_id]", "318"}, //+
                    {"data[offer_seek]", string.Empty}, //+
                    {"data[param_price][0]", "price"}, //+
                    {"data[param_price][1]", data[6]}, //+
                    {"data[param_price][currency]", "USD"}, //+
                    {"data[param_state]", state}, //+
                    {"data[param_bike_manufacturer]", string.IsNullOrEmpty(model) ? "766" : model}, //+
                    {"data[param_currency]", string.Empty}, //+
                    {"data[private_business]", "private"}, //+
                    {"data[description]", data[9]}, //+
                    {"data[region_id]", city[0]}, //TODO+
                    {"data[person]", data[0]}, //+
                    {"data[riak_key]", ""},
                    {"data[adding_key]", ""},
                    {"data[subregion_id]", city[1]},
                    {"data[contactform]", "on"}, //TODO Не получать ответы на email
                    {"data[phone]", data[2]},
                    //{"data[suggested_categories][]", "70"},//?
                    //{"data[suggested_categories][]", "541"},//?
                    //{"data[suggested_categories][]", "540"},//?
                    {"data[suggested_categories][]", "96"} //?
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"myfile", files[0]},
                    {"myfile2", files[1]},
                    {"myfile3", files[2]},
                    {"myfile4", files[3]},
                    {"myfile5", files[4]},
                    {"myfile6", files[5]},
                    {"myfile7", files[6]},
                    {"myfile8", files[7]}
                }
            };
        }
    }
}