using Motorcycle.Config.Data;
using Motorcycle.Interfaces;

namespace Motorcycle.Factories
{
    internal static class SiteDataFactory
    {
        public static ISiteData GetSiteData(SiteEnum site)
        {
            switch (site)
            {
                case SiteEnum.Proday2Kolesa:
                    return new DvaKolesaData();

                case SiteEnum.UsedAuto:
                    return new UsedAutoData();

                case SiteEnum.MotoSale:
                    return new MotosaleData();

                case SiteEnum.Olx:
                    return new OlxData();

                default:
                    return null;
            }
        }
    }
}