namespace Motorcycle.Utils
{
    using Motorcycle.Config.Data;
    using System.Collections.Generic;

    public class ListMixer
    {
        public static List<DicHolder> MixTwoLists(List<DicHolder> fList, List<DicHolder> sList)
        {
            var resultList = new List<DicHolder>(fList.Count + sList.Count);

            List<DicHolder> maxList;
            List<DicHolder> minList;

            if (fList.Count >= sList.Count)
            {
                maxList = fList;
                minList = sList;
            }
            else
            {
                maxList = sList;
                minList = fList;
            }

            for (var i = 0; i < maxList.Count; i++)
            {
                resultList.Add(maxList[i]);

                if (minList.Count > i)
                    resultList.Add(minList[i]);
            }

            return resultList;
        }

        public static List<DicHolder> MixThreeLists(List<DicHolder> fList, List<DicHolder> sList, List<DicHolder> tList)
        {
            var resultList = new List<DicHolder>(fList.Count + sList.Count + tList.Count);

            List<DicHolder> maxList;
            List<DicHolder> firstTempList;
            List<DicHolder> secondTempList;

            if (fList.Count >= sList.Count && fList.Count >= tList.Count)
            {
                maxList = fList;
                firstTempList = tList;
                secondTempList = sList;
            }

            else if (sList.Count >= fList.Count && sList.Count >= tList.Count)
            {
                maxList = sList;
                firstTempList = tList;
                secondTempList = fList;
            }

            else
            {
                maxList = tList;
                firstTempList = sList;
                secondTempList = fList;
            }

            for (var i = 0; i < maxList.Count; i++)
            {
                resultList.Add(maxList[i]);

                if (firstTempList.Count > i)
                    resultList.Add(firstTempList[i]);

                if (secondTempList.Count > i)
                    resultList.Add(secondTempList[i]);
            }

            return resultList;
        }
    }
}
