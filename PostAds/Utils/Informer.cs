namespace Motorcycle.Utils
{
    internal static class Informer
    {
        public delegate void InformMethod(bool result);
        public delegate void ParameterlessInformMethod();
        public static event InformMethod OnPostResultChanged;
        public static event InformMethod OnProxyListFromInternetUpdated;
        public static event ParameterlessInformMethod OnAllPostsAreCompleted;
        public static event ParameterlessInformMethod OnMotosalePostsAreCompleted;
        public static event ParameterlessInformMethod OnUsedAutoPostsAreCompleted;
        public static event ParameterlessInformMethod OnProdayPostsAreCompleted;

        public static void RaiseOnPostResultChangedEvent(bool post)
        {
            var handler = OnPostResultChanged;
            if (handler != null)
                handler(post);
        }

        public static void RaiseOnProxyListFromInternetUpdatedEvent(bool result)
        {
            var handler = OnProxyListFromInternetUpdated;
            if (handler != null)
                handler(result);
        }

        public static void RaiseOnAllPostsAreCompletedEvent()
        {
            var handler = OnAllPostsAreCompleted;
            if (handler != null)
                handler();
        }

        public static void RaiseOnMotosalePostsAreCompletedEvent()
        {
            var handler = OnMotosalePostsAreCompleted;
            if (handler != null)
                handler();
        }

        public static void RaiseOnUsedAutoPostsAreCompletedEvent()
        {
            var handler = OnUsedAutoPostsAreCompleted;
            if (handler != null)
                handler();
        }

        public static void RaiseOnProdayPostsAreCompletedEvent()
        {
            var handler = OnProdayPostsAreCompleted;
            if (handler != null)
                handler();
        }
    }
}