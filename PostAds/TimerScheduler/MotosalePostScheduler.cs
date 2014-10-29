namespace Motorcycle.TimerScheduler
{
    using Config.Data;
    using Utils;

    public class MotosalePostScheduler : PostSchedulerBase
    {
        protected override sealed SiteEnum Site { get; set; }

        public MotosalePostScheduler()
        {
            Site = SiteEnum.MotoSale;
        }

        protected override void SetFinishPostingStatus(bool status)
        {
            FinishPosting.MotosaleFinished = status;
        }

        protected override void RaiseOnSitePostsAreCompleted()
        {
            Informer.RaiseOnMotosalePostsAreCompletedEvent();
        }
    }
}