namespace Motorcycle.Interfaces
{
    using Config.Data;

    public interface IPostOnSite
    {
        void PostMoto(DicHolder data);

        void PostSpare(DicHolder data);

        void PostEquip(DicHolder data);
    }
}