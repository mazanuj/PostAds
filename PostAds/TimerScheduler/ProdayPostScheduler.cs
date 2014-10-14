namespace Motorcycle.TimerScheduler
{
    using System.Threading.Tasks;

    using Config.Data;
    using Sites;
    using Utils;
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.Timers;

    internal static class ProdayPostScheduler
    {
        //don't forget FinishPosting.ResetValues() higher!!!
        private static Timer timer = new Timer();
        private static int counter;
        private static readonly object Locker = new object();
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static async Task StartPostMsgWithTimer(
            List<DicHolder> dataList,
            byte fromHour,
            byte toHour,
            int interval)
        {
            FinishPosting.ProdayFinished = false;

            await CheckerAsync(dataList);
            if (dataList.Count == counter)
            {
                if (timer.Enabled)
                    timer.Stop();

                Log.Info("All posts to Proday2Kolesa are completed");

                Informer.RaiseOnProdayPostsAreCompletedEvent();

                FinishPosting.ProdayFinished = true;
                if (FinishPosting.CheckIfPostingToAllSitesFinished())
                {
                    //Notify UI that all posting were finished
                    Informer.RaiseOnAllPostsAreCompletedEvent();
                }
                return;
            }

            timer.Interval = interval != 0 ? interval * 60000 : 2000;
            timer.Elapsed += (s, e) =>
            {
                lock (Locker)
                {
                    if ((fromHour < toHour && DateTime.Now.Hour >= fromHour && DateTime.Now.Hour < toHour)
                        || (fromHour > toHour && DateTime.Now.Hour >= fromHour && DateTime.Now.Hour > toHour)
                        || (fromHour > toHour && DateTime.Now.Hour <= fromHour && DateTime.Now.Hour < toHour))
                    {
                        Checker(dataList);
                        if (dataList.Count == counter)
                        {
                            if (timer.Enabled) timer.Stop();

                            Log.Info("All posts to Proday2Kolesa are completed");

                            Informer.RaiseOnProdayPostsAreCompletedEvent();

                            FinishPosting.ProdayFinished = true;
                            if (FinishPosting.CheckIfPostingToAllSitesFinished())
                            {
                                //Notify UI that all posting were finished
                                Informer.RaiseOnAllPostsAreCompletedEvent();
                            }
                        }
                    }
                    else
                    {
                        //Not right time
                        Log.Info("Can't post at this time on Proday2Kolesa");
                    }
                }
            };

            timer.Start();
        }

        public static void StopPostMsgWithTimer()
        {
            timer.Stop();
        }

        public static void ResetValues()
        {
            timer = new Timer();
            counter = 0;
        }

        private static void Checker(IList<DicHolder> dataList)
        {
            if (dataList.Count <= counter) return;
            //Main work will be here
            switch (dataList[counter].Type)
            {
                case ProductEnum.Equip:
                    Informer.RaiseOnPostResultChangedEvent(
                        Proday2Kolesa.PostEquip(dataList[counter]) == PostStatus.OK);
                    break;
                case ProductEnum.Motorcycle:
                    Informer.RaiseOnPostResultChangedEvent(
                        Proday2Kolesa.PostMoto(dataList[counter]) == PostStatus.OK);
                    break;
                case ProductEnum.Spare:
                    Informer.RaiseOnPostResultChangedEvent(
                        Proday2Kolesa.PostSpare(dataList[counter]) == PostStatus.OK);
                    break;
            }

            counter++;
        }

        private static async Task CheckerAsync(IList<DicHolder> dataList)
        {
            await TaskEx.Run(
                () => Checker(dataList));
        }
    }
}