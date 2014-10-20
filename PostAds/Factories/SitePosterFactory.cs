namespace Motorcycle.Factories
{
    using Motorcycle.Config.Data;
    using Motorcycle.Interfaces;
    using Motorcycle.Sites;

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

                default:
                    return null;
            }
        }
    }
}
