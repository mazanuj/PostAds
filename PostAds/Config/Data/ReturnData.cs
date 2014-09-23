using Motorcycle.Factories;

namespace Motorcycle.Config.Data
{
    using Interfaces;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    internal static class ReturnData
    {
        private static readonly List<InfoHolder> ReturnDataHolders = new List<InfoHolder>();

        private static string motoFile;
        private static string spareFile;
        private static string equipFile;

        internal static async Task<List<InfoHolder>> GetData(string motofile, string sparefile, string equipfile,
            byte[] flag)
        {
            motoFile = motofile;
            spareFile = sparefile;
            equipFile = equipfile;

            ReturnDataHolders.Clear();
            ////Проверка сайтов для постинга

            //MotoSale
            if (flag[0] == 1)
            {
                await OrganizeWorkWithDifferentFiles(SiteEnum.MotoSale);
            }

            //UsedAuto
            if (flag[1] == 1)
            {
                await OrganizeWorkWithDifferentFiles(SiteEnum.UsedAuto);
            }

            //ProdayDvaKolesa
            if (flag[2] == 1)
            {
                await OrganizeWorkWithDifferentFiles(SiteEnum.Proday2Kolesa);
            }

            return ReturnDataHolders;
        }

        private static async Task OrganizeWorkWithDifferentFiles(SiteEnum site)
        {
            var siteData = SiteDataFactory.GetSiteData(site);

            if (motoFile != null)
            {
                await FillReturnDataHoldersList(site, ProductEnum.Motorcycle, motoFile, siteData);
            }

            if (spareFile != null)
            {
                await FillReturnDataHoldersList(site, ProductEnum.Spare, spareFile, siteData);
            }

            if (equipFile != null)
            {
                await FillReturnDataHoldersList(site, ProductEnum.Equip, equipFile, siteData);
            }
        }

        private static async Task FillReturnDataHoldersList(SiteEnum site, ProductEnum product, string textFile,
            ISiteData siteData)
        {
            await Task.Factory.StartNew(
                () =>
                {
                    var listFile = new List<string>(File.ReadAllLines(textFile)); //TODO ?? Encoding.Unicode
                    var infoHolder = new InfoHolder {Site = site, Type = product};

                    switch (product)
                    {
                        case ProductEnum.Motorcycle:
                            foreach (var row in listFile)
                                infoHolder.Data.Add(siteData.GetMoto(row));
                            break;

                        case ProductEnum.Equip:
                            foreach (var row in listFile)
                                infoHolder.Data.Add(siteData.GetEquip(row));
                            break;

                        case ProductEnum.Spare:
                            foreach (var row in listFile)
                                infoHolder.Data.Add(siteData.GetSpare(row));
                            break;
                    }

                    ReturnDataHolders.Add(infoHolder);
                });
        }
    }
}