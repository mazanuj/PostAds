namespace Motorcycle.Interfaces
{
    using System.Threading.Tasks;
    using Config.Data;

    public interface IPostOnSite
    {
        Task PostMoto(DicHolder data);

        Task PostSpare(DicHolder data);

        Task PostEquip(DicHolder data);
    }
}