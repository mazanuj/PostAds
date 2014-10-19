namespace Motorcycle.TimerScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Motorcycle.Config.Data;
    using Motorcycle.Sites;
    using Motorcycle.Utils;

    using NLog;

    class ProdayPostSchedulerNew
    {
        private static Timer timer;
        private static int counter;
        private static bool wasTimeBoundariesMsgAlreadyShowen;
        private static bool wasOnAllPostsAreCompletedEventAlreadyRaised;
        private static readonly object LockerForPost = new object();
        private static readonly object LockerForChecking = new object();
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static void StartPostMsgWithTimer(List<DicHolder> dataList, byte fromHour, byte toHour, int interval)
        {
            wasOnAllPostsAreCompletedEventAlreadyRaised = false;
            counter = 0;
            var userInterval = interval != 0 ? TimeSpan.FromMinutes(interval) : TimeSpan.FromSeconds(5);

            FinishPosting.ProdayFinished = false;

            timer = new Timer(
                state =>
                    {
                        #region timer lambda
                        if (CheckTimeBounderies(fromHour, toHour))
                        {
                            wasTimeBoundariesMsgAlreadyShowen = false;

                            if (timer != null)
                            {
                                timer.Change(userInterval, userInterval);
                            }

                            lock (LockerForPost)
                            {
                                PostOnSite(dataList);
                            }

                            if (dataList.Count == counter)
                            {
                                //Stop
                                if (timer != null)
                                {
                                    timer.Dispose();
                                }

                                Log.Info("All posts to Proday2Kolesa are completed", SiteEnum.Proday2Kolesa, null);
                                Informer.RaiseOnProdayPostsAreCompletedEvent();
                                FinishPosting.ProdayFinished = true;

                                lock (LockerForChecking)
                                {
                                    if (FinishPosting.CheckIfPostingToAllSitesFinished()
                                        && !wasOnAllPostsAreCompletedEventAlreadyRaised)
                                    {
                                        wasOnAllPostsAreCompletedEventAlreadyRaised = true;
                                        //Notify UI that all posting were finished
                                        Informer.RaiseOnAllPostsAreCompletedEvent();
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Not right time
                            if (!wasTimeBoundariesMsgAlreadyShowen)
                            {
                                wasTimeBoundariesMsgAlreadyShowen = true;

                                if (timer != null)
                                {
                                    timer.Change(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
                                }
                                Log.Info("Can't post at this time on Proday2Kolesa", SiteEnum.Proday2Kolesa, null);
                            }
                        }
                    }
                #endregion
                , null,
                TimeSpan.Zero, userInterval);
        }

        public static void StopPostMsgWithTimer()
        {
            timer.Dispose();
        }

        private static bool CheckTimeBounderies(byte fromHour, byte toHour)
        {
            return (fromHour < toHour && DateTime.Now.Hour >= fromHour && DateTime.Now.Hour < toHour)
                   || (fromHour > toHour && DateTime.Now.Hour >= fromHour && DateTime.Now.Hour > toHour)
                   || (fromHour > toHour && DateTime.Now.Hour <= fromHour && DateTime.Now.Hour < toHour);
        }

        private static void PostOnSite(IList<DicHolder> dataList)
        {
            if (dataList.Count <= counter) return;
            //Main work will be here
            PostStatus postResult;
            switch (dataList[counter].Type)
            {
                case ProductEnum.Equip:
                    postResult = Proday2Kolesa.PostEquip(dataList[counter++]);
                    Informer.RaiseOnPostResultChangedEvent(postResult == PostStatus.OK);
                    if (postResult == PostStatus.ERROR)
                        PostOnSite(dataList);
                    break;

                case ProductEnum.Motorcycle:
                    postResult = Proday2Kolesa.PostMoto(dataList[counter++]);
                    Informer.RaiseOnPostResultChangedEvent(postResult == PostStatus.OK);
                    if (postResult == PostStatus.ERROR)
                        PostOnSite(dataList);
                    break;

                case ProductEnum.Spare:
                    postResult = Proday2Kolesa.PostSpare(dataList[counter++]);
                    Informer.RaiseOnPostResultChangedEvent(postResult == PostStatus.OK);
                    if (postResult == PostStatus.ERROR)
                        PostOnSite(dataList);
                    break;
            }
        }
    }
}
