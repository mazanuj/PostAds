
namespace Motorcycle.Config.Data
{
    class SiteDataFactory
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

                default:
                    return new DvaKolesaData();
            }
        }
    }
}
