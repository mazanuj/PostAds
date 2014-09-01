using System.Collections.Generic;

namespace Motorcycle.Config.Data
{
    static class UsedAutoData
    {
        internal static ReturnData GetData(List<string> data)
        {
            #region usedAuto

            var dataUsedAuto = new Dictionary<string, string>
            {
                {"cid", "83"},
                {"act", "add"},
                {"rid", "3"},
                {"input[1]", "19"},
                {"input[153]", "model"},
                {"input[3]", "1"},
                {"price", "1000"},
                {"currency", "1"},
                {"input[4]", "5555"},
                {"input[200]", "600"},
                {"input[8]", "110"},
                {"input[18]", "1"},
                {"input[61]", "1"},
                {"input[6]", "1"},
                {"input[5]", "color"},
                {"input[194]", "1"},
                {"input[15]", "on"},
                {"input[151]", "on"},
                {"input[219]", "on"},
                {"description", "message"},
                {"photos", ""},
                {"main_photo", ""},
                {"user[0]", "name"},
                {"user[1]", "380676776767"},
                {"user[2]", ""},
                {"user[3]", ""},
                {"user[4]", ""},
                {"user[5]", "mail@ma.ma"}
            };
            var filesUsedAuto = new Dictionary<string, string> { { "file", "virginia.jpg" } };

            #endregion

            return new ReturnData(dataUsedAuto, filesUsedAuto);
        }
    }
}