using Motorcycle.Config.Data;
using Motorcycle.Utils;

namespace Motorcycle.TimerScheduler
{
    public class OlxPostScheduler : PostSchedulerBase
    {
        protected override sealed SiteEnum Site { get; set; }

        public OlxPostScheduler()
        {
            Site = SiteEnum.Olx;
        }

        protected override void SetFinishPostingStatus(bool status)
        {
            FinishPosting.OlxFinished = status;
        }

        protected override void RaiseOnSitePostsAreCompleted()
        {
            Informer.RaiseOnOlxPostsAreCompletedEvent();
        }
    }
}