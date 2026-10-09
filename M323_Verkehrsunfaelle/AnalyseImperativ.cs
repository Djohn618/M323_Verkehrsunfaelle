// ============================================================================
// Datei:        AnalyseImperativ.cs
// Beschreibung: V1.0 - Alle Auswertungen der Verkehrsunfälle IMPERATIV gelöst,
//               also mit Schleifen (foreach), if-Abfragen und Variablen, die
//               Schritt für Schritt verändert werden.
// Autor:        Deshan John Kothalawalage
// Version:      1.0
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M323_Verkehrsunfaelle
{
    /// <summary>
    /// V1.0: Auswertungen der Unfalldaten im imperativen Stil (Schleifen und Zähler).
    /// </summary>
    public class AnalyseImperativ
    {
        // ------------------------------------------------------------------
        // 1. FILTER: Unfälle ab einem Jahr
        // ------------------------------------------------------------------

        /// <summary>
        /// Gibt nur die Unfälle zurück, die im angegebenen Jahr oder später passiert sind (Filter).
        /// </summary>
        /// <param name="unfaelle">Alle eingelesenen Unfälle.</param>
        /// <param name="abJahr">Das erste Jahr, das noch dazugehören soll (z.B. 2020).</param>
        /// <returns>Eine neue Liste mit den passenden Unfällen.</returns>
        public static List<Unfall> UnfaelleAbJahr(List<Unfall> unfaelle, int abJahr)
        {
            var ergebnis = new List<Unfall>();

            foreach (var u in unfaelle)
            {
                // Filter: nur Unfälle ab dem gewünschten Jahr übernehmen
                if (u.Jahr >= abJahr)
                    ergebnis.Add(u);
            }

            return ergebnis;
        }

        // ------------------------------------------------------------------
        // 2. MAP: Typ und Schweregrad in kurze Bezeichnungen umwandeln
        // ------------------------------------------------------------------

        /// <summary>
        /// Wandelt einen langen Unfalltyp in eine kurze Bezeichnung um,
        /// z.B. "Schleuder- oder Selbstunfall" wird zu "Selbstunfall".
        /// </summary>
        /// <param name="typ">Der Unfalltyp aus der CSV-Datei.</param>
        /// <returns>Die kurze Bezeichnung (oder der Originaltext, wenn der Typ unbekannt ist).</returns>
        public static string KurzerTyp(string typ)
        {
            switch (typ)
            {
                case "Abbiegeunfall": return "Abbiegen";
                case "Auffahrunfall": return "Auffahren";
                case "Einbiegeunfall": return "Einbiegen";
                case "Frontalkollision": return "Frontal";
                case "Fussgängerunfall": return "Fussgänger";
                case "Parkierunfall": return "Parkieren";
                case "Schleuder- oder Selbstunfall": return "Selbstunfall";
                case "Tierunfall": return "Tier";
                case "Überholunfall oder Fahrstreifenwechsel": return "Überholen";
                case "Überqueren der Fahrbahn": return "Überqueren";
                default: return typ;
            }
        }

        /// <summary>
        /// Wandelt einen langen Schweregrad in eine kurze Bezeichnung um,
        /// z.B. "3 Unfall mit Schwerverletzten" wird zu "Schwerverletzte".
        /// </summary>
        /// <param name="schweregrad">Der Schweregrad aus der CSV-Datei.</param>
        /// <returns>Die kurze Bezeichnung (oder der Originaltext, wenn der Schweregrad unbekannt ist).</returns>
        public static string KurzerSchweregrad(string schweregrad)
        {
            switch (schweregrad)
            {
                case "1 Unfall mit Sachschaden": return "Sachschaden";
                case "2 Unfall mit Leichtverletzten": return "Leichtverletzte";
                case "3 Unfall mit Schwerverletzten": return "Schwerverletzte";
                case "4 Unfall mit Getöteten": return "Getötete";
                default: return schweregrad;
            }
        }

        /// <summary>
        /// Macht aus jedem Unfall einen neuen Unfall mit kurzen Bezeichnungen
        /// für Typ und Schweregrad (Map). Das Jahr bleibt gleich.
        /// </summary>
        /// <param name="unfaelle">Alle eingelesenen Unfälle.</param>
        /// <returns>Eine neue Liste, gleich lang wie die Eingabe, mit kurzen Bezeichnungen.</returns>
        public static List<Unfall> KurzeBezeichnungen(List<Unfall> unfaelle)
        {
            var ergebnis = new List<Unfall>();

            foreach (var u in unfaelle)
            {
                // Map: jeden Unfall in einen "kurzen" Unfall umwandeln
                string typ = KurzerTyp(u.Typ);
                string schwere = KurzerSchweregrad(u.Schweregrad);
                ergebnis.Add(new Unfall(u.Jahr, typ, schwere));
            }

            return ergebnis;
        }

        // ------------------------------------------------------------------
        // 3. REDUCE: Anzahl Unfälle pro Jahr
        // ------------------------------------------------------------------

        /// <summary>
        /// Zählt, wie viele Unfälle es pro Jahr gab (Reduce).
        /// </summary>
        /// <param name="unfaelle">Alle eingelesenen Unfälle.</param>
        /// <returns>Dictionary: Jahr → Anzahl Unfälle.</returns>
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

        // ------------------------------------------------------------------
        // 4. REDUCE: Anzahl pro Typ und häufigster Typ
        // ------------------------------------------------------------------

        /// <summary>
        /// Zählt, wie viele Unfälle es pro Unfalltyp gab (Reduce).
        /// </summary>
        /// <param name="unfaelle">Alle eingelesenen Unfälle.</param>
        /// <returns>Dictionary: Unfalltyp → Anzahl Unfälle.</returns>
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

        /// <summary>
        /// Findet den Unfalltyp mit den meisten Unfällen (Reduce: viele Einträge → ein Ergebnis).
        /// </summary>
        /// <param name="proTyp">Das Ergebnis von <see cref="UnfaelleProTyp"/>.</param>
        /// <returns>Der häufigste Unfalltyp, oder "" wenn keine Daten vorhanden sind.</returns>
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

        // ------------------------------------------------------------------
        // 5. FILTER + REDUCE: Schwere Unfälle pro Jahr
        // ------------------------------------------------------------------

        /// <summary>
        /// Prüft, ob ein Unfall "schwer" ist: mit Schwerverletzten oder Getöteten.
        /// </summary>
        /// <param name="u">Der Unfall, der geprüft wird.</param>
        /// <returns>true, wenn der Unfall schwer ist, sonst false.</returns>
        public static bool IstSchwererUnfall(Unfall u)
        {
            if (u.Schweregrad.Contains("Schwerverletzten"))
                return true;

            if (u.Schweregrad.Contains("Getöteten"))
                return true;

            return false;
        }

        /// <summary>
        /// Zählt pro Jahr nur die schweren Unfälle (Filter + Reduce).
        /// </summary>
        /// <param name="unfaelle">Alle eingelesenen Unfälle.</param>
        /// <returns>Dictionary: Jahr → Anzahl schwere Unfälle.</returns>
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

        // ------------------------------------------------------------------
        // 6. MAP + REDUCE: Anteil schwerer Unfälle pro Jahr in Prozent
        // ------------------------------------------------------------------

        /// <summary>
        /// Berechnet pro Jahr, wie viel Prozent aller Unfälle schwer waren (Map + Reduce).
        /// Das Resultat wird auf eine Nachkommastelle gerundet.
        /// </summary>
        /// <param name="unfaelle">Alle eingelesenen Unfälle.</param>
        /// <returns>Dictionary: Jahr → Anteil schwerer Unfälle in Prozent (z.B. 12.3).</returns>
        public static Dictionary<int, double> AnteilSchwererUnfaelleProJahr(List<Unfall> unfaelle)
        {
            var gesamtProJahr = new Dictionary<int, int>();   // Jahr -> alle Unfälle
            var schwerProJahr = new Dictionary<int, int>();   // Jahr -> schwere Unfälle

            // Reduce: in einer Schleife beide Zähler hochzählen
            foreach (var u in unfaelle)
            {
                if (!gesamtProJahr.ContainsKey(u.Jahr))
                {
                    gesamtProJahr[u.Jahr] = 0;
                    schwerProJahr[u.Jahr] = 0;
                }

                gesamtProJahr[u.Jahr]++;

                if (IstSchwererUnfall(u))
                    schwerProJahr[u.Jahr]++;
            }

            // Map: aus den zwei Zahlen pro Jahr einen Prozentwert machen
            var ergebnis = new Dictionary<int, double>();
            foreach (var eintrag in gesamtProJahr)
            {
                int jahr = eintrag.Key;
                double prozent = schwerProJahr[jahr] * 100.0 / gesamtProJahr[jahr];
                ergebnis[jahr] = Math.Round(prozent, 1);
            }

            return ergebnis;
        }
    }
}
