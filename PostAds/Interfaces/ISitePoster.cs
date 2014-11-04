namespace Motorcycle.Interfaces
{
    using Config.Data;
    using Sites;

    public interface ISitePoster
    {
        PostStatus PostMoto(DicHolder data);

        PostStatus PostEquip(DicHolder data);

        PostStatus PostSpare(DicHolder data);
    }
}
