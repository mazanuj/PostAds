namespace Motorcycle.TimerScheduler
{
    using Motorcycle.Config.Data;
    using Motorcycle.Utils;

    public class UsedAutoPostScheduler : PostSchedulerBase
    {
        protected override sealed SiteEnum Site { get; set; }

        public UsedAutoPostScheduler()
        {
            Site = SiteEnum.UsedAuto;
        }

        protected override void SetFinishPostingStatus(bool status)
        {
            FinishPosting.UsedAutoFinished = status;
        }

        protected override void RaiseOnSitePostsAreCompleted()
        {
            Informer.RaiseOnUsedAutoPostsAreCompletedEvent();
        }
    }
}
