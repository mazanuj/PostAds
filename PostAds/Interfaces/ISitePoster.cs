namespace Motorcycle.Interfaces
{
    using Motorcycle.Config.Data;
    using Motorcycle.Sites;

    public interface ISitePoster
    {
        PostStatus PostMoto(DicHolder data);

        PostStatus PostEquip(DicHolder data);

        PostStatus PostSpare(DicHolder data);
    }
}
