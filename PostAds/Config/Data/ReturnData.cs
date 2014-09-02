using System.Collections.Generic;
using System.IO;

namespace Motorcycle.Config.Data
{
    internal static class ReturnData
    {
        private static readonly List<InfoHolder> ReturnDataHolders = new List<InfoHolder>();

        private static string motoFile;
        private static string spareFile;
        private static string equipFile;

        internal static List<InfoHolder> GetData(string motofile, string sparefile, string equipfile, byte[] flag)
        {
            motoFile = motofile;
            spareFile = sparefile;
            equipFile = equipfile;

            ////Проверка сайтов для постинга

            //MotoSale
            if (flag[0] == 1)
            {
                OrganizeWorkWithDifferentFiles(SiteEnum.MotoSale);
            }

            //UsedAuto
            if (flag[1] == 1)
            {
                OrganizeWorkWithDifferentFiles(SiteEnum.UsedAuto);
            }

            //ProdayDvaKolesa
            if (flag[2] == 1)
            {
                OrganizeWorkWithDifferentFiles(SiteEnum.Proday2Kolesa);
            }

            return ReturnDataHolders;
        }

        private static void OrganizeWorkWithDifferentFiles(SiteEnum site)
        {
            var siteData = SiteDataFactory.GetSiteData(site);

            if (motoFile != null)
            {
                FillReturnDataHoldersList(site, ProductEnum.Motorcycle, motoFile, siteData);
            }

            if (spareFile != null)
            {
                FillReturnDataHoldersList(site, ProductEnum.Spare, spareFile, siteData);
            }

            if (equipFile != null)
            {
                FillReturnDataHoldersList(site, ProductEnum.Equip, equipFile, siteData);
            }
        }

        private static void FillReturnDataHoldersList(SiteEnum site, ProductEnum product, string textFile,
            ISiteData siteData)
        {
            var listFile = new List<string>(File.ReadAllLines(textFile));
            var infoHolder = new InfoHolder {Site = site, Type = product};

            foreach (var row in listFile)
            {
                infoHolder.Data.Add(siteData.GetMoto(row));
            }
            ReturnDataHolders.Add(infoHolder);
        }
    }
}