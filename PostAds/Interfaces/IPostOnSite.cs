namespace Motorcycle.Interfaces
{
    using Motorcycle.Config.Data;

    public interface IPostOnSite
    {
        void PostMoto(DicHolder data);

        void PostSpare(DicHolder data);

        void PostEquip(DicHolder data);
    }
}