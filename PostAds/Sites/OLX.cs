using Motorcycle.Config.Data;
using Motorcycle.Interfaces;

namespace Motorcycle.Sites
{
    public class Olx : ISitePoster
    {
        public PostStatus PostMoto(DicHolder data)
        {
            return PostStatus.OK;
        }

        public PostStatus PostSpare(DicHolder data)
        {
            return PostStatus.OK;
        }

        public PostStatus PostEquip(DicHolder data)
        {
            return PostStatus.OK;
        }
    }
}