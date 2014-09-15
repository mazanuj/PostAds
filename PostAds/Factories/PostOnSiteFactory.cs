using Motorcycle.Config.Data;
using Motorcycle.Interfaces;
using Motorcycle.Sites;

namespace Motorcycle.Factories
{
    internal static class PostOnSiteFactory
    {
        public static IPostOnSite GetPostOnSite(SiteEnum site)
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
                    return new Proday2Kolesa();
            }
        }
    }
}