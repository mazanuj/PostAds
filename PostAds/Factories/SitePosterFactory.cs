namespace Motorcycle.Factories
{
    using Config.Data;
    using Interfaces;
    using Sites;

    public static class SitePosterFactory
    {
        public static ISitePoster GetSitePoster(SiteEnum site)
        {
            switch (site)
            {
                case SiteEnum.Proday2Kolesa:
                    return new Proday2Kolesa();

                case SiteEnum.UsedAuto:
                    return new UsedAuto();

                case SiteEnum.MotoSale:
                    return new MotoSale();

                case SiteEnum.Olx:
                    return new Olx();

                default:
                    return null;
            }
        }
    }
}
