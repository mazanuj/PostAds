namespace Motorcycle.TimerScheduler
{
    using Config.Data;
    using Utils;

    public class ProdayPostScheduler : PostSchedulerBase
    {
        protected override sealed SiteEnum Site { get; set; }

        public ProdayPostScheduler()
        {
            Site = SiteEnum.Proday2Kolesa;
        }

        protected override void SetFinishPostingStatus(bool status)
        {
            FinishPosting.ProdayFinished = status;
        }

        protected override void RaiseOnSitePostsAreCompleted()
        {
            Informer.RaiseOnProdayPostsAreCompletedEvent();
        }
    }
}
