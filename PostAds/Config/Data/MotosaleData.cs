using System.Collections.Generic;

namespace Motorcycle.Config.Data
{
    internal class MotosaleData : ISiteData
    {
        public DicHolder GetMoto(string row)
        {
            var data = row.Split('\t');

            return new DicHolder
            {
                DataDictionary = new Dictionary<string, string>
                {
                    {"name", data[0]},
                    {"mail", data[1]},
                    {"phone", data[2]},
                    {"header", data[3]},
                    {"type_obj", "1"}, //Sell|Buy
                    {"model", ""}, //Proizvoditel'
                    {"manufactured_model", "41"}, //Model
                    {"custom_model", ""}, //Custom Model
                    {"custom_model", ""},
                    {"docum", "1"},//Без документов
                    {"price", ""},//цена
                    {"run",""},//пробег
                    {"in",""},//год выпуска
                    {"fConfirmationCode", "3582"},
                    {"insert", ""}
                },
                FileDictionary = new Dictionary<string, string>
                {
                    {"filename", "virginia.jpg"}
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