namespace Motorcycle.TimerScheduler
{
    public class TimerSchedulerParams
    {
        public byte MotosaleFrom { get; set; }
        public byte MotosaleTo { get; set; }
        public int MotosaleInterval { get; set; }

        public byte UsedAutoFrom { get; set; }
        public byte UsedAutoTo { get; set; }
        public int UsedAutoInterval { get; set; }

        public byte ProdayFrom { get; set; }
        public byte ProdayTo { get; set; }
        public int ProdayInterval { get; set; }
    }
}
