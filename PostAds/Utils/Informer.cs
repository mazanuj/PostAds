﻿namespace Motorcycle.Utils
{
    internal class Informer
    {
        public delegate void InformMethod(bool result);

        public static event InformMethod OnPostResultChanged;
        public static event InformMethod OnProxyListFromInternetUpdated;

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
    }
}