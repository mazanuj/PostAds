using Motorcycle.Config.Confirm;

namespace Motorcycle.Config
{
    using System.Collections.Generic;
    using Data;
    using TimerScheduler;
    using Utils;
    using System.Linq;
    using System.Threading.Tasks;

    internal static class Advertising
    {
        internal static async Task Initialize(byte[] flag, TimerSchedulerParams timerParams)
        {
            //Check Olx mail with confirmation all ads
           await PostConfirm.ConfirmAllOlxAdv();

           //List<DicHolder>
           var returnDataHolders = await ReturnData.GetData(flag);

           FinishPosting.ResetValues();

           #region Post on Motosale

           if (flag[0] > 0)
           {
               var resultList = GetResultList(returnDataHolders, SiteEnum.MotoSale);

               //if (resultList.Count > 0)
               //{
                   var motosalePostScheduler = new MotosalePostScheduler();
                   motosalePostScheduler.StartPostMsgWithTimer(
                       resultList,
                       timerParams.MotosaleFrom,
                       timerParams.MotosaleTo,
                       timerParams.MotosaleInterval);
               //}
           }

           #endregion

           #region Post on UsedAuto

           if (flag[1] > 0)
           {
               var resultList = GetResultList(returnDataHolders, SiteEnum.UsedAuto);

               //if (resultList.Count > 0)
               //{
                   var usedAutoPostScheduler = new UsedAutoPostScheduler();
                   usedAutoPostScheduler.StartPostMsgWithTimer(
                       resultList,
                       timerParams.UsedAutoFrom,
                       timerParams.UsedAutoTo,
                       timerParams.UsedAutoInterval);
               //}
           }

           #endregion

           #region Post on Proday2Kolesa

           if (flag[2] > 0)
           {
               var resultList = GetResultList(returnDataHolders, SiteEnum.Proday2Kolesa);

               //if (resultList.Count > 0)
               //{
                   var prodayPostScheduler = new ProdayPostScheduler();
                   prodayPostScheduler.StartPostMsgWithTimer(
                       resultList,
                       timerParams.ProdayFrom,
                       timerParams.ProdayTo,
                       timerParams.ProdayInterval);
               //}
           }

           #endregion

           #region Post on Olx

           if (flag[3] > 0)
           {
               var resultList = GetResultList(returnDataHolders, SiteEnum.Olx);

               //if (resultList.Count > 0)
               //{
                   var olxPostScheduler = new OlxPostScheduler();
                   olxPostScheduler.StartPostMsgWithTimer(
                       resultList,
                       timerParams.OlxFrom,
                       timerParams.OlxTo,
                       timerParams.OlxInterval);
               //}
           }

           #endregion
        }

        private static List<DicHolder> GetResultList(IEnumerable<DicHolder> returnDataHolders, SiteEnum site)
        {
            List<DicHolder> resultList;

            if (site == SiteEnum.UsedAuto)
                resultList = returnDataHolders.Where(
                    holder =>
                        holder.IsError == false && holder.Site == site
                        && (holder.Type == ProductEnum.Motorcycle || holder.Type == ProductEnum.Spare)).ToList();
            else
                resultList = returnDataHolders.Where(
                    holder =>
                        holder.IsError == false && holder.Site == site
                        && (holder.Type == ProductEnum.Motorcycle || holder.Type == ProductEnum.Spare
                            || holder.Type == ProductEnum.Equip)).ToList();

            resultList.Shuffle();
            return resultList;
        }
    }
}