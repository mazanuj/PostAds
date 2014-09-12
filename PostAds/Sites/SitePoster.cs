namespace Motorcycle.Sites
{
    using Config.Data;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class SitePoster
    {
        public static async Task PostAdvertises(IEnumerable<InfoHolder> holders)
        {
            //var tasks = holders.Select(async holder =>
            //{
            //    var poster = PostOnSiteFactory.GetPostOnSite(holder.Site);

            //    foreach (var infoHolder in holder.Data)
            //    {
            //        switch (holder.Type)
            //        {
            //            case ProductEnum.Equip:
            //                await poster.PostEquip(infoHolder);
            //                break;

            //            case ProductEnum.Motorcycle:
            //                await poster.PostMoto(infoHolder);
            //                break;

            //            case ProductEnum.Spare:
            //                await poster.PostSpare(infoHolder);
            //                break;
            //        }

            //    }
            //}).ToList();

            var tasks = new List<Task<PostStatus>>();

            foreach (var infoHolder in holders)
            {
                var poster = PostOnSiteFactory.GetPostOnSite(infoHolder.Site);

                foreach (var dataDic in infoHolder.Data)
                {
                    switch (infoHolder.Type)
                    {
                        case ProductEnum.Equip:
                            tasks.Add(poster.PostEquip(dataDic));
                            break;

                        case ProductEnum.Motorcycle:
                            tasks.Add(poster.PostMoto(dataDic));
                            break;

                        case ProductEnum.Spare:
                            tasks.Add(poster.PostSpare(dataDic));
                            break;
                    }
                }
            }

            foreach (var task in tasks)
            {
                var res = await task;
            }
        }

        public enum PostStatus
        {
            OK,
            ERROR
        }
    }
}