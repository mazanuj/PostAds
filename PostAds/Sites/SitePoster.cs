namespace Motorcycle.Sites
{
    using Config.Data;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal static class SitePoster
    {
        public static async Task PostAdvertises(IEnumerable<InfoHolder> holders)
        {
            var tasks = holders.Select(async holder =>
            {
                var poster = PostOnSiteFactory.GetPostOnSite(holder.Site);

                foreach (var infoHolder in holder.Data)
                {
                    switch (holder.Type)
                    {
                        case ProductEnum.Equip:
                            await poster.PostEquip(infoHolder);
                            break;

                        case ProductEnum.Motorcycle:
                            await poster.PostMoto(infoHolder);
                            break;

                        case ProductEnum.Spare:
                            await poster.PostSpare(infoHolder);
                            break;
                    }

                }
            }).ToList();

            foreach (var task in tasks)
            {
                await task;
            }
        }
    }
}