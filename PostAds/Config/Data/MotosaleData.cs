﻿using System.Collections.Generic;

namespace Motorcycle.Config.Data
{
    internal static class MotosaleData
    {
        internal static List<Dictionary<string, string>> GetData(List<string> data)
        {
            #region motosale

            var dataMotoSale = new Dictionary<string, string>
            {
                {"name", data[0]},
                {"mail", data[1]},
                {"phone", data[2]},
                {"header", data[3]},
                {"type_obj", "1"}, //Sell|Buy
                {"model", ""}, //Parameters.GetManufacture("motosale",data[4])},//Proizvoditel'
                {"manufactured_model", "41"}, //Model
                {"custom_model", ""},
                {"docum", "1"},
                {"fConfirmationCode", "3582"},
                {"insert", ""}
            };
            var filesMotoSale = new Dictionary<string, string>
            {
                {"filename", "virginia.jpg"}
            };

            #endregion

            return null;
        }
    }
}