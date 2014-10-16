namespace Motorcycle.TimerScheduler
{
    using System.Security.Policy;

    using Config.Data;
    using NLog;
    using Sites;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Utils;
    using System.Timers;

    internal static class MotosalePostScheduler
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
            FinishPosting.MotosaleFinished = false;

            await CheckerAsync(dataList);
            if (dataList.Count == counter)
            {
                if (timer.Enabled)
                    timer.Stop();

                Log.Info("All posts to MotoSale are completed", SiteEnum.MotoSale, null);

                Informer.RaiseOnMotosalePostsAreCompletedEvent();

                FinishPosting.MotosaleFinished = true;
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
                            if (timer.Enabled)
                                timer.Stop();

                            Log.Info("All posts to MotoSale are completed", SiteEnum.MotoSale, null);

                            Informer.RaiseOnMotosalePostsAreCompletedEvent();

                            FinishPosting.MotosaleFinished = true;
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
                        Log.Info("Can't post at this time on MotoSale", SiteEnum.MotoSale, null);
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
            PostStatus postResult;
            switch (dataList[counter].Type)
            {
                case ProductEnum.Equip:
                    postResult = MotoSale.PostEquip(dataList[counter++]);
                    Informer.RaiseOnPostResultChangedEvent(postResult == PostStatus.OK);
                    if(postResult == PostStatus.ERROR) 
                        Checker(dataList);
                    break;

                case ProductEnum.Motorcycle:
                    postResult = MotoSale.PostMoto(dataList[counter++]);
                    Informer.RaiseOnPostResultChangedEvent(postResult == PostStatus.OK);
                    if(postResult == PostStatus.ERROR) 
                        Checker(dataList);
                    break;

                case ProductEnum.Spare:
                    postResult = MotoSale.PostSpare(dataList[counter++]);
                    Informer.RaiseOnPostResultChangedEvent(postResult == PostStatus.OK);
                    if(postResult == PostStatus.ERROR) 
                        Checker(dataList);
                    break;
            }
        }

        private static async Task CheckerAsync(IList<DicHolder> dataList)
        {
            await TaskEx.Run(
                () => Checker(dataList));
        }
    }
}