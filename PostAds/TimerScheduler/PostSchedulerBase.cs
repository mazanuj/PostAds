﻿namespace Motorcycle.TimerScheduler
{
    using Config.Data;
    using Factories;
    using Sites;
    using Utils;
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public abstract class PostSchedulerBase
    {
        private Timer timer;
        private int counter;
        private bool wasTimeBoundariesMsgAlreadyShowen;
        private bool wasOnAllPostsAreCompletedEventAlreadyRaised;
        private readonly object lockerForPost = new object();
        private readonly object lockerForChecking = new object();
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        protected abstract SiteEnum Site { get; set; }

        private static bool CheckTimeBounderies(byte fromHour, byte toHour)
        {
            return (fromHour < toHour && DateTime.Now.Hour >= fromHour && DateTime.Now.Hour < toHour)
                   || (fromHour > toHour && DateTime.Now.Hour >= fromHour && DateTime.Now.Hour > toHour)
                   || (fromHour > toHour && DateTime.Now.Hour <= fromHour && DateTime.Now.Hour < toHour);
        }

        private void PostOnSite(IList<DicHolder> dataList)
        {
            while (true)
            {
                if (dataList.Count <= counter) return;

                var sitePoster = SitePosterFactory.GetSitePoster(Site);
                PostStatus postResult;

                switch (dataList[counter].Type)
                {
                    case ProductEnum.Equip:
                        postResult = sitePoster.PostEquip(dataList[counter++]);
                        Informer.RaiseOnPostResultChangedEvent(postResult == PostStatus.OK);
                        if (postResult == PostStatus.ERROR)
                            continue;
                        break;

                    case ProductEnum.Motorcycle:
                        postResult = sitePoster.PostMoto(dataList[counter++]);
                        Informer.RaiseOnPostResultChangedEvent(postResult == PostStatus.OK);
                        if (postResult == PostStatus.ERROR)
                            continue;
                        break;

                    case ProductEnum.Spare:
                        postResult = sitePoster.PostSpare(dataList[counter++]);
                        Informer.RaiseOnPostResultChangedEvent(postResult == PostStatus.OK);
                        if (postResult == PostStatus.ERROR)
                            continue;
                        break;
                }
                break;
            }
        }

        private void StopTimer()
        {
            if (timer != null)
            {
                timer.Dispose();
            }

            log.Info(string.Format("All posts to {0} are completed", Site), Site, null);
            RaiseOnSitePostsAreCompleted();
            SetFinishPostingStatus(true);

            lock (lockerForChecking)
            {
                if (!FinishPosting.CheckIfPostingToAllSitesFinished() || wasOnAllPostsAreCompletedEventAlreadyRaised)
                    return;
                wasOnAllPostsAreCompletedEventAlreadyRaised = true;
                //Notify UI that all posting were finished
                Informer.RaiseOnAllPostsAreCompletedEvent();
            }
        }

        protected abstract void SetFinishPostingStatus(bool status);

        protected abstract void RaiseOnSitePostsAreCompleted();

        protected PostSchedulerBase()
        {
            Informer.OnStopTimerClicked += () =>
            {
                if (timer != null)
                    timer.Dispose();
            };
        }

        public void StartPostMsgWithTimer(List<DicHolder> dataList, byte fromHour, byte toHour, int interval)
        {
            var userInterval = interval == 0 ? 5000 : interval*60000;

            counter = 0;
            wasOnAllPostsAreCompletedEventAlreadyRaised = false;

            SetFinishPostingStatus(false);

            timer = new Timer(
                state =>
                {
                    if (CheckTimeBounderies(fromHour, toHour))
                    {
                        wasTimeBoundariesMsgAlreadyShowen = false;

                        lock (lockerForPost)
                        {
                            PostOnSite(dataList);
                        }

                        if (dataList.Count <= counter)
                        {
                            //Stop
                            StopTimer();
                            return;
                        }

                        if (timer != null)
                            timer.Change(userInterval, Timeout.Infinite);
                    }
                    else
                    {
                        //Not right time
                        if (!wasTimeBoundariesMsgAlreadyShowen)
                        {
                            wasTimeBoundariesMsgAlreadyShowen = true;
                            log.Info(string.Format("Can't post at this time on {0}", Site), Site, null);
                        }
                        if (timer != null)
                            timer.Change(60000, Timeout.Infinite);
                    }
                },
                null, 0, Timeout.Infinite);
        }
    }
}