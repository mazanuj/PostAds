using System.Collections.Generic;
using System.IO;

namespace Motorcycle.Config.Data
{
    internal static class ReturnData
    {
        private static readonly List<InfoHolder> returnDataHolders = new List<InfoHolder>();

        internal static List<InfoHolder> GetData(string motoFile, string spareFile, string equipFile, byte[] flag)
        {
            //Проверка сайтов для постинга

            //MotoSale
            if (flag[0] == 1)
            {
                if (motoFile != null)
                {
                    var listFile = new List<string>(File.ReadAllLines(motoFile));
                    var infoHolder = new InfoHolder
                    {
                        Site = SiteEnum.MotoSale,
                        Type = ProductEnum.Motorcycle
                    };

                    foreach (var row in listFile)
                    {
                        infoHolder.Data.Add(MotosaleData.GetMoto(row));
                    }
                    returnDataHolders.Add(infoHolder);
                }
                if (spareFile != null)
                {
                    var listFile = new List<string>(File.ReadAllLines(spareFile));
                    var infoHolder = new InfoHolder
                    {
                        Site = SiteEnum.MotoSale,
                        Type = ProductEnum.Spare
                    };

                    foreach (var row in listFile)
                    {
                        infoHolder.Data.Add(MotosaleData.GetSpare(row));
                    }
                    returnDataHolders.Add(infoHolder);
                }
                if (equipFile != null)
                {
                    var listFile = new List<string>(File.ReadAllLines(equipFile));
                    var infoHolder = new InfoHolder
                    {
                        Site = SiteEnum.MotoSale,
                        Type = ProductEnum.Equip
                    };

                    foreach (var row in listFile)
                    {
                        infoHolder.Data.Add(MotosaleData.GetEquip(row));
                    }
                    returnDataHolders.Add(infoHolder);
                }
            }

            //UsedAuto
            if (flag[1] == 1)
            {
                if (motoFile != null)
                {
                    var listFile = new List<string>(File.ReadAllLines(motoFile));
                    var infoHolder = new InfoHolder
                    {
                        Site = SiteEnum.UsedAuto,
                        Type = ProductEnum.Motorcycle
                    };

                    foreach (var row in listFile)
                    {
                        infoHolder.Data.Add(UsedAutoData.GetMoto(row));
                    }
                    returnDataHolders.Add(infoHolder);
                }
                if (spareFile != null)
                {
                    var listFile = new List<string>(File.ReadAllLines(spareFile));
                    var infoHolder = new InfoHolder
                    {
                        Site = SiteEnum.UsedAuto,
                        Type = ProductEnum.Spare
                    };

                    foreach (var row in listFile)
                    {
                        infoHolder.Data.Add(UsedAutoData.GetSpare(row));
                    }
                    returnDataHolders.Add(infoHolder);
                }
                if (equipFile != null)
                {
                    var listFile = new List<string>(File.ReadAllLines(equipFile));
                    var infoHolder = new InfoHolder
                    {
                        Site = SiteEnum.UsedAuto,
                        Type = ProductEnum.Equip
                    };

                    foreach (var row in listFile)
                    {
                        infoHolder.Data.Add(UsedAutoData.GetEquip(row));
                    }
                    returnDataHolders.Add(infoHolder);
                }
            }

            //ProdayDvaKolesa
            if (flag[2] == 1)
            {
                if (motoFile != null)
                {
                    var listFile = new List<string>(File.ReadAllLines(motoFile));
                    var infoHolder = new InfoHolder
                    {
                        Site = SiteEnum.Proday2Kolesa,
                        Type = ProductEnum.Motorcycle
                    };

                    foreach (var row in listFile)
                    {
                        infoHolder.Data.Add(DvaKolesaData.GetMoto(row));
                    }
                    returnDataHolders.Add(infoHolder);
                }
                if (spareFile != null)
                {
                    var listFile = new List<string>(File.ReadAllLines(spareFile));
                    var infoHolder = new InfoHolder
                    {
                        Site = SiteEnum.Proday2Kolesa,
                        Type = ProductEnum.Spare
                    };

                    foreach (var row in listFile)
                    {
                        infoHolder.Data.Add(DvaKolesaData.GetSpare(row));
                    }
                    returnDataHolders.Add(infoHolder);
                }
                if (equipFile != null)
                {
                    var listFile = new List<string>(File.ReadAllLines(equipFile));
                    var infoHolder = new InfoHolder
                    {
                        Site = SiteEnum.Proday2Kolesa,
                        Type = ProductEnum.Equip
                    };

                    foreach (var row in listFile)
                    {
                        infoHolder.Data.Add(DvaKolesaData.GetEquip(row));
                    }
                    returnDataHolders.Add(infoHolder);
                }
            }

            return returnDataHolders;
        }
    }
}