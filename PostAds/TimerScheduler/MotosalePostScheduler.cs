namespace Motorcycle.TimerScheduler
{
    using Config.Data;
    using Sites;
    using Utils;
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.Timers;

    internal static class MotosalePostScheduler
    {
        //don't forget FinishPosting.ResetValues() higher!!!
        private static Timer timer = new Timer();
        private static int counter;
        private static readonly object Locker = new object();
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static void StartPostMsgWithTimer(
            List<DicHolder> dataList,
            byte fromHour,
            byte toHour,
            int interval)
        {
            FinishPosting.MotosaleFinished = false;

            Checker(dataList);
            if (dataList.Count == counter)
            {
                if (timer.Enabled)
                    timer.Stop();

                Log.Info("All posts to MotoSale are completed");

                Informer.RaiseOnMotosalePostsAreCompletedEvent();

                FinishPosting.MotosaleFinished = true;
                if (FinishPosting.CheckIfPostingToAllSitesFinished())
                {
                    //Notify UI that all posting were finished
                    Informer.RaiseOnAllPostsAreCompletedEvent();
                }
                return;
            }

            timer.Interval = interval != 0 ? interval*60000 : 2000;
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

                            Log.Info("All posts to MotoSale are completed");

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
                        Log.Info("Can't post at this time on MotoSale");
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
                        MotoSale.PostEquip(dataList[counter]) == PostStatus.OK);
                    break;
                case ProductEnum.Motorcycle:
                    Informer.RaiseOnPostResultChangedEvent(
                        MotoSale.PostMoto(dataList[counter]) == PostStatus.OK);
                    break;
                case ProductEnum.Spare:
                    Informer.RaiseOnPostResultChangedEvent(
                        MotoSale.PostSpare(dataList[counter]) == PostStatus.OK);
                    break;
            }
            counter++;
        }
    }
}