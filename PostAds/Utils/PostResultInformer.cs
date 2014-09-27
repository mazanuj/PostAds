namespace Motorcycle.Utils
{
    internal class PostResultInformer
    {
        public delegate void InformMethod(bool result);

        public static event InformMethod InformPostResultEvent;

        public static void RaiseEvent(bool post)
        {
            var handler = InformPostResultEvent;
            if (handler != null)
                handler(post);
        }
    }
}
