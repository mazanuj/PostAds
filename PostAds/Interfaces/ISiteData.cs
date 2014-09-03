
namespace Motorcycle.Interfaces
{
    using Motorcycle.Config.Data;

    public interface ISiteData
    {
        DicHolder GetMoto(string row);

        DicHolder GetSpare(string row);

        DicHolder GetEquip(string row);
    }
}
