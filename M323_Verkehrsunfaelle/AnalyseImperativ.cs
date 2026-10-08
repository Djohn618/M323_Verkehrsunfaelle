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

        // Zählt, wie viele Unfälle es pro Unfalltyp gab.
        public static Dictionary<string, int> UnfaelleProTyp(List<Unfall> unfaelle)
        {
            var ergebnis = new Dictionary<string, int>();   // Typ -> Anzahl

            foreach (var u in unfaelle)
            {
                if (!ergebnis.ContainsKey(u.Typ))
                    ergebnis[u.Typ] = 0;

                ergebnis[u.Typ]++;
            }

            return ergebnis;
        }

        // Findet den Unfalltyp mit den meisten Unfällen.
        public static string HaeufigsterTyp(Dictionary<string, int> proTyp)
        {
            string besterTyp = "";
            int maxAnzahl = -1;

            foreach (var eintrag in proTyp)
            {
                if (eintrag.Value > maxAnzahl)   // mehr als der bisherige Rekord?
                {
                    maxAnzahl = eintrag.Value;   // neuen Rekord merken
                    besterTyp = eintrag.Key;     // und welcher Typ es war
                }
            }

            return besterTyp;
        }

        // Prüft, ob ein Unfall "schwer" ist: mit Schwerverletzten oder Getöteten.
        // Gibt true zurück, wenn der Schweregrad-Text eines der beiden Wörter enthält.
        public static bool IstSchwererUnfall(Unfall u)
        {
            if (u.Schweregrad.Contains("Schwerverletzten"))
                return true;

            if (u.Schweregrad.Contains("Getöteten"))
                return true;

            return false;
        }

        // Zählt pro Jahr nur die schweren Unfälle (Schwerverletzte oder Getötete).
        // Filter: Unfälle, die nicht schwer sind, werden übersprungen.
        // Reduce: die übrigen Unfälle werden pro Jahr zusammengezählt.
        public static Dictionary<int, int> SchwereUnfaelleProJahr(List<Unfall> unfaelle)
        {
            var ergebnis = new Dictionary<int, int>();   // Jahr -> Anzahl schwere Unfälle

            foreach (var u in unfaelle)
            {
                // Filter: nur schwere Unfälle weiterverarbeiten
                if (!IstSchwererUnfall(u))
                    continue;

                // Reduce: pro Jahr zählen
                if (!ergebnis.ContainsKey(u.Jahr))
                    ergebnis[u.Jahr] = 0;

                ergebnis[u.Jahr]++;
            }

            return ergebnis;
        }
    }
}
