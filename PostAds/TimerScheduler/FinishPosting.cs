namespace Motorcycle.TimerScheduler
{
    public static class FinishPosting
    {
        public static bool MotosaleFinished { get; set; }
        public static bool UsedAutoFinished { get; set; }
        public static bool ProdayFinished { get; set; }
        public static bool OlxFinished { get; set; }

        public static bool CheckIfPostingToAllSitesFinished()
        {
            return MotosaleFinished && UsedAutoFinished && ProdayFinished && OlxFinished;
        }

        public static void ResetValues()
        {
            MotosaleFinished = true;
            UsedAutoFinished = true;
            ProdayFinished = true;
            OlxFinished = true;
        }
    }
}