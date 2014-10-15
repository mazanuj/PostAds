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
                var resultList =
                    returnDataHolders.Where(
                        holder => holder.IsError == false && holder.Site == SiteEnum.MotoSale &&
                            (holder.Type == ProductEnum.Motorcycle || holder.Type == ProductEnum.Spare || holder.Type == ProductEnum.Equip)).ToList();

                resultList.Shuffle();

                if (resultList.Count > 0)
                {
                    MotosalePostScheduler.ResetValues();

                    await MotosalePostScheduler.StartPostMsgWithTimer(
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
                var resultList =
                    returnDataHolders.Where(
                        holder => holder.IsError == false && holder.Site == SiteEnum.UsedAuto &&
                            (holder.Type == ProductEnum.Motorcycle || holder.Type == ProductEnum.Spare)).ToList();

                resultList.Shuffle();

                if (resultList.Count > 0)
                {
                    UsedAutoPostScheduler.ResetValues();
                    await UsedAutoPostScheduler.StartPostMsgWithTimer(
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
                var resultList =
                    returnDataHolders.Where(
                        holder => holder.IsError == false && holder.Site == SiteEnum.Proday2Kolesa &&
                            (holder.Type == ProductEnum.Motorcycle || holder.Type == ProductEnum.Spare || holder.Type == ProductEnum.Equip)).ToList();

                resultList.Shuffle();

                if (resultList.Count > 0)
                {
                    ProdayPostScheduler.ResetValues();
                    await ProdayPostScheduler.StartPostMsgWithTimer(
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