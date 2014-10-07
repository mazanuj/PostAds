namespace Motorcycle.Config.Data
{
    using Interfaces;
    using Motorcycle.Factories;
    using NLog;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using XmlWorker;

    internal static class ReturnData
    {
        private static readonly List<InfoHolder> ReturnDataHolders = new List<InfoHolder>();
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static string motoFile;
        private static string spareFile;
        private static string equipFile;

        internal static async Task<List<InfoHolder>> GetData(byte[] flag)
        {
            motoFile = FilePathXmlWorker.GetFilePath("moto");
            spareFile = FilePathXmlWorker.GetFilePath("spare");
            equipFile = FilePathXmlWorker.GetFilePath("equip");

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

            if (!string.IsNullOrEmpty(motoFile))
            {
                await FillReturnDataHoldersList(site, ProductEnum.Motorcycle, motoFile, siteData);
            }

            if (!string.IsNullOrEmpty(spareFile))
            {
                await FillReturnDataHoldersList(site, ProductEnum.Spare, spareFile, siteData);
            }

            if (!string.IsNullOrEmpty(equipFile))
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
                    var listFile = File.ReadAllLines(textFile, Encoding.GetEncoding(Ude(textFile))).Distinct().ToList();
                    if (listFile.Count == 0)
                    {
                        Log.Warn(textFile.Substring(textFile.LastIndexOf(@"\", System.StringComparison.Ordinal) + 1) +
                                 " is empty");
                        return;
                    }
                    var infoHolder = new InfoHolder {Site = site, Type = product};
                    var lineNum = 0;

                    switch (product)
                    {
                        case ProductEnum.Motorcycle:
                            foreach (var row in listFile.Where(row => !string.IsNullOrEmpty(row)))
                                infoHolder.Data.Add(siteData.GetMoto(row, lineNum++));
                            break;

                        case ProductEnum.Equip:
                            foreach (var row in listFile.Where(row => !string.IsNullOrEmpty(row)))
                                infoHolder.Data.Add(siteData.GetEquip(row, lineNum++));
                            break;

                        case ProductEnum.Spare:
                            foreach (var row in listFile.Where(row => !string.IsNullOrEmpty(row)))
                                infoHolder.Data.Add(siteData.GetSpare(row, lineNum++));
                            break;
                    }

                    ReturnDataHolders.Add(infoHolder);
                });
        }

        private static string Ude(string filename)
        {
            using (var fs = File.OpenRead(filename))
            {
                var cdet = new Ude.CharsetDetector();
                cdet.Feed(fs);
                cdet.DataEnd();

                return cdet.Charset ?? "windows-1251";
            }
        }
    }
}