
namespace Motorcycle.Config.Data
{
    using System.Collections.Generic;
    using Motorcycle.XmlWorker;
    using Motorcycle.Interfaces;

    internal class MotosaleData : ISiteData
    {
        public DicHolder GetMoto(string row)
        {
            var data = row.Split('\t');
            var d = data[13].Split(',');
            var files = new string[5];
            for (var i = 0; i < 5; i++)
            {
                if (i < d.Length)
                    files[i] = d[i];
                else files[i] = string.Empty;
            }

            var model = ManufactureXmlWorker.GetItemValueUsingPlantAndName(data[4], data[5]);
            var customModel = model != string.Empty ? string.Empty : data[5];

            return new DicHolder
            {
                DataDictionary = new Dictionary<string, string>
                {
                    {"name", data[0]}, //+
                    {"mail", data[1]}, //+
                    {"phone", data[2]}, //+
                    {"header", data[3]}, //+
                    {"type_obj", "1"}, //Sell|Buy+
                    {"model", ManufactureXmlWorker.GetItemSiteValueUsingPlant(data[4], "m")}, //Proizvoditel'+
                    {"manufactured_model", model},//Model+
                    {"custom_model", customModel}, //Custom Model+
                    {"docum", data[6]}, //Без документов+ ////BASA
                    {"price", data[7]}, //цена+
                    {"run", data[8]}, //пробег+
                    {"in", data[9]}, //год выпуска+
                    {"moto", data[10]}, //тип транс средства+!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    {"objem", data[11]}, //+
                    {"param[tip_transmissii][]", data[18]}, //+
                    {"param[transnision]", data[19]}, //+
                    {"city", CityXmlWorker.GetItemSiteValueUsingCity(data[12], "m")}, //+
                    {"youtube", data[15]}, //+
                    {"date_delete", "14"}, //+
                    {"fConfirmationCode", ""}, //captcha+
                    {"insert", ""} //+
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"filename", files[0]},
                    {"filename2", files[1]},
                    {"filename3", files[2]},
                    {"filename4", files[3]},
                    {"filename5", files[4]}
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