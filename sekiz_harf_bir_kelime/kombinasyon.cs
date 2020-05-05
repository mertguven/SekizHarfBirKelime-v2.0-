using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sekiz_harf_bir_kelime
{
    class kombinasyon
    {
        public void listele(List<string> harflerq, List<string> sekizli, List<string> yedili, List<string> altili, List<string> besli, List<string> dortlu, List<string> uclu)
        {
            List<string> possibleCombination = GetCombination(harflerq, new List<string>(), "");
            foreach (string item in possibleCombination)
            {
                if (item.Count() == 8)
                {
                    sekizli.Add(item);
                }
                if (item.Count() == 7)
                {
                    yedili.Add(item);
                }
                if (item.Count() == 6)
                {
                    altili.Add(item);
                }
                if (item.Count() == 5)
                {
                    besli.Add(item);
                }
                if (item.Count() == 4)
                {
                    dortlu.Add(item);
                }
                if (item.Count() == 3)
                {
                    uclu.Add(item);
                }
            }
        }
        static List<string> GetCombination(List<string> list, List<string> combinations, string sumNum, bool addNumberToResult = false)//Kombinasyon işleminin yapıldığı yer
        {
            if (list.Count == 0)
            {
                return combinations;
            }
            string tmp;
            for (int i = 0; i <= list.Count - 1; i++)
            {
                tmp = string.Concat(sumNum, list[i]);
                if (addNumberToResult)
                {
                    combinations.Add(tmp);
                }
                List<string> tmp_list = new List<string>(list);
                tmp_list.RemoveAt(i);
                GetCombination(tmp_list, combinations, tmp, true);
            }
            return combinations;
        }
    }
}
