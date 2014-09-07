
namespace Motorcycle.Interfaces
{
    using System.Threading.Tasks;

    using Motorcycle.Config.Data;

    public interface IPostOnSite
    {
        Task PostMoto(DicHolder data);

        Task PostSpare(DicHolder data);

        Task PostEquip(DicHolder data);
    }
}
