namespace Motorcycle.Config
{
    using Data;
    using TimerScheduler;
    using Utils;
    using System.Linq;
    using System.Threading.Tasks;

    internal static class Advertising
    {
        internal static async Task Initialize(byte[] flag, TimerSchedulerParams timerParams)
        {
            //List<DicHolder>
            var returnDataHolders = await ReturnData.GetData(flag);

            FinishPosting.ResetValues();

            #region Post on Motosale

            if (flag[0] > 0)
            {
                var motoList =
                    returnDataHolders.Where(
                        holder => holder.Site == SiteEnum.MotoSale && holder.Type == ProductEnum.Motorcycle).ToList();

                var spareList = returnDataHolders.Where(
                    holder => holder.Site == SiteEnum.MotoSale && holder.Type == ProductEnum.Spare).ToList();

                var equipList = returnDataHolders.Where(
                    holder => holder.Site == SiteEnum.MotoSale && holder.Type == ProductEnum.Equip).ToList();

                var resultList = ListMixer.MixThreeLists(motoList, spareList, equipList);

                if (resultList.Count > 0)
                {
                    MotosalePostScheduler.ResetValues();
                    MotosalePostScheduler.StartPostMsgWithTimer(
                        resultList,
                        timerParams.MotosaleFrom,
                        timerParams.MotosaleTo,
                        timerParams.MotosaleInterval);

                }
            }

            #endregion

            #region Post on UsedAuto

            if (flag[1] > 0)
            {
                var motoList =
                    returnDataHolders.Where(
                        holder => holder.Site == SiteEnum.UsedAuto && holder.Type == ProductEnum.Motorcycle).ToList();

                var spareList = returnDataHolders.Where(
                    holder => holder.Site == SiteEnum.UsedAuto && holder.Type == ProductEnum.Spare).ToList();

                var resultList = ListMixer.MixTwoLists(motoList, spareList);

                if (resultList.Count > 0)
                {
                    UsedAutoPostScheduler.ResetValues();
                    UsedAutoPostScheduler.StartPostMsgWithTimer(
                        resultList,
                        timerParams.UsedAutoFrom,
                        timerParams.UsedAutoTo,
                        timerParams.UsedAutoInterval);
                }
            }

            #endregion

            #region Post on Proday2Kolesa

            if (flag[2] > 0)
            {
                var motoList =
                    returnDataHolders.Where(
                        holder => holder.Site == SiteEnum.Proday2Kolesa && holder.Type == ProductEnum.Motorcycle)
                        .ToList();

                var spareList = returnDataHolders.Where(
                    holder => holder.Site == SiteEnum.Proday2Kolesa && holder.Type == ProductEnum.Spare).ToList();

                var equipList = returnDataHolders.Where(
                    holder => holder.Site == SiteEnum.Proday2Kolesa && holder.Type == ProductEnum.Equip).ToList();

                var resultList = ListMixer.MixThreeLists(motoList, spareList, equipList);

                if (resultList.Count > 0)
                {
                    ProdayPostScheduler.ResetValues();
                    ProdayPostScheduler.StartPostMsgWithTimer(
                        resultList,
                        timerParams.ProdayFrom,
                        timerParams.ProdayTo,
                        timerParams.ProdayInterval);
                }
            }

            #endregion
        }
    }
}