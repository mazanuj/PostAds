using Motorcycle.Config.Confirm;

namespace Motorcycle.Config
{
    using Data;
    using Sites;
    using NLog;
    using System.Threading.Tasks;

    internal static class Advertising
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        internal static async Task Initialize(string motoFile, string spareFile, string equipFile, byte[] flag)
        {
            PostConfirm.GetMails("pop.mail.ru", 995, true, "mo-snikers@mail.ru", "Administr@t0r");
            //List<InfoHolder>
            var returnDataHolders = await ReturnData.GetData(motoFile, spareFile, equipFile, flag);

            await SitePoster.PostAdvertises(returnDataHolders);
        }
    }
}