namespace Motorcycle.TimerScheduler
{
    using Motorcycle.Config.Data;
    using Motorcycle.Factories;
    using Motorcycle.Sites;
    using Motorcycle.Utils;
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

        private bool CheckTimeBounderies(byte fromHour, byte toHour)
        {
            return (fromHour < toHour && DateTime.Now.Hour >= fromHour && DateTime.Now.Hour < toHour)
                   || (fromHour > toHour && DateTime.Now.Hour >= fromHour && DateTime.Now.Hour > toHour)
                   || (fromHour > toHour && DateTime.Now.Hour <= fromHour && DateTime.Now.Hour < toHour);
        }

        private void PostOnSite(IList<DicHolder> dataList)
        {
            if (dataList.Count <= this.counter) return;

            var sitePoster = SitePosterFactory.GetSitePoster(Site);
            PostStatus postResult;

            switch (dataList[this.counter].Type)
            {
                case ProductEnum.Equip:
                    postResult = sitePoster.PostEquip(dataList[this.counter++]);
                    Informer.RaiseOnPostResultChangedEvent(postResult == PostStatus.OK);
                    if (postResult == PostStatus.ERROR)
                        PostOnSite(dataList);
                    break;

                case ProductEnum.Motorcycle:
                    postResult = sitePoster.PostMoto(dataList[this.counter++]);
                    Informer.RaiseOnPostResultChangedEvent(postResult == PostStatus.OK);
                    if (postResult == PostStatus.ERROR)
                        PostOnSite(dataList);
                    break;

                case ProductEnum.Spare:
                    postResult = sitePoster.PostSpare(dataList[this.counter++]);
                    Informer.RaiseOnPostResultChangedEvent(postResult == PostStatus.OK);
                    if (postResult == PostStatus.ERROR)
                        PostOnSite(dataList);
                    break;
            }
        }

        private void StopTimer()
        {
            if (this.timer != null)
            {
                this.timer.Dispose();
            }

            this.log.Info(string.Format("All posts to {0} are completed", this.Site), this.Site, null);
            this.RaiseOnSitePostsAreCompleted();
            this.SetFinishPostingStatus(true);

            lock (this.lockerForChecking)
            {
                if (FinishPosting.CheckIfPostingToAllSitesFinished() && !this.wasOnAllPostsAreCompletedEventAlreadyRaised)
                {
                    this.wasOnAllPostsAreCompletedEventAlreadyRaised = true;
                    //Notify UI that all posting were finished
                    Informer.RaiseOnAllPostsAreCompletedEvent();
                }
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
            var userInterval = interval == 0 ? 5000 : interval * 60000;

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
                            log.Info(string.Format("Can't post at this time on Proday2Kolesa", Site), Site, null);
                        }
                        if (this.timer != null)
                            this.timer.Change(60000, Timeout.Infinite);
                    }
                },
                null, 0, Timeout.Infinite);

        }
    }
}
