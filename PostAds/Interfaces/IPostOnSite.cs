using Motorcycle.Sites;

namespace Motorcycle.Interfaces
{
    using System.Threading.Tasks;
    using Config.Data;

    public interface IPostOnSite
    {
        Task<SitePoster.PostStatus> PostMoto(DicHolder data);

        Task<SitePoster.PostStatus> PostSpare(DicHolder data);

        Task<SitePoster.PostStatus> PostEquip(DicHolder data);
    }
}