using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M323_Verkehrsunfaelle
{
    public class AnalyseImperativ
    {
        public static Dictionary<int, int> UnfaelleProJahr(List<Unfall> unfaelle)
        {
            var ergebnis = new Dictionary<int, int>();

            foreach (var u in unfaelle)
            {
                if (!ergebnis.ContainsKey(u.Jahr))
                    ergebnis[u.Jahr] = 0;
                ergebnis[u.Jahr]++;
            }
            return ergebnis;
        }
    }
}
