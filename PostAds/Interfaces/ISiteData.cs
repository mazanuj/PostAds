﻿namespace Motorcycle.Interfaces
{
    using Config.Data;

    public interface ISiteData
    {
        DicHolder GetMoto(string row);

        DicHolder GetSpare(string row);

        DicHolder GetEquip(string row);
    }
}