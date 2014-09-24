namespace Motorcycle.Interfaces
{
    using Config.Data;

    public interface ISiteData
    {
        DicHolder GetMoto(string row, int lineNum);

        DicHolder GetSpare(string row, int lineNum);

        DicHolder GetEquip(string row, int lineNum);
    }
}