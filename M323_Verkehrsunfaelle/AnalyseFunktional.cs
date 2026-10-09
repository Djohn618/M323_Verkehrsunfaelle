// ============================================================================
// Datei:        AnalyseFunktional.cs
// Beschreibung: V2.0 - Die gleichen Auswertungen wie in AnalyseImperativ.cs,
//               aber FUNKTIONAL mit LINQ gelöst. Statt Schleifen und Zählern
//               werden die Daten durch eine Kette von Funktionen geschickt:
//               Where = Filter, Select = Map, GroupBy + Count = Reduce.
// Autor:        Deshan John Kothalawalage
// Version:      2.0
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace M323_Verkehrsunfaelle
{
    /// <summary>
    /// V2.0: Auswertungen der Unfalldaten im funktionalen Stil mit LINQ.
    /// </summary>
    /// <remarks>
    /// Imperativ = WIE wird gerechnet (Schleife, Zähler, if, Variable verändern).
    /// Funktional = WAS soll herauskommen (Daten durch eine Kette von Funktionen schicken).
    /// </remarks>
    public class AnalyseFunktional
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
            // Imperativ: foreach + if + Add.
            // Funktional: Where lässt nur die Elemente durch, für die die Bedingung true ist.
            return unfaelle
                .Where(u => u.Jahr >= abJahr)   // Filter
                .ToList();
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
            // Imperativ: switch-Anweisung mit vielen "return".
            // Funktional: switch-Ausdruck, der direkt einen Wert ergibt.
            return typ switch
            {
                "Abbiegeunfall" => "Abbiegen",
                "Auffahrunfall" => "Auffahren",
                "Einbiegeunfall" => "Einbiegen",
                "Frontalkollision" => "Frontal",
                "Fussgängerunfall" => "Fussgänger",
                "Parkierunfall" => "Parkieren",
                "Schleuder- oder Selbstunfall" => "Selbstunfall",
                "Tierunfall" => "Tier",
                "Überholunfall oder Fahrstreifenwechsel" => "Überholen",
                "Überqueren der Fahrbahn" => "Überqueren",
                _ => typ   // unbekannt: Originaltext behalten
            };
        }

        /// <summary>
        /// Wandelt einen langen Schweregrad in eine kurze Bezeichnung um,
        /// z.B. "3 Unfall mit Schwerverletzten" wird zu "Schwerverletzte".
        /// </summary>
        /// <param name="schweregrad">Der Schweregrad aus der CSV-Datei.</param>
        /// <returns>Die kurze Bezeichnung (oder der Originaltext, wenn der Schweregrad unbekannt ist).</returns>
        public static string KurzerSchweregrad(string schweregrad)
        {
            return schweregrad switch
            {
                "1 Unfall mit Sachschaden" => "Sachschaden",
                "2 Unfall mit Leichtverletzten" => "Leichtverletzte",
                "3 Unfall mit Schwerverletzten" => "Schwerverletzte",
                "4 Unfall mit Getöteten" => "Getötete",
                _ => schweregrad
            };
        }

        /// <summary>
        /// Macht aus jedem Unfall einen neuen Unfall mit kurzen Bezeichnungen
        /// für Typ und Schweregrad (Map). Das Jahr bleibt gleich.
        /// </summary>
        /// <param name="unfaelle">Alle eingelesenen Unfälle.</param>
        /// <returns>Eine neue Liste, gleich lang wie die Eingabe, mit kurzen Bezeichnungen.</returns>
        public static List<Unfall> KurzeBezeichnungen(List<Unfall> unfaelle)
        {
            // Imperativ: foreach + neue Liste + Add.
            // Funktional: Select wandelt jedes Element in ein neues Element um.
            return unfaelle
                .Select(u => new Unfall(u.Jahr, KurzerTyp(u.Typ), KurzerSchweregrad(u.Schweregrad)))   // Map
                .ToList();
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
            // Imperativ: foreach + Dictionary von Hand befüllen und hochzählen.
            // Funktional: GroupBy bildet die Gruppen, Count zählt jede Gruppe.
            return unfaelle
                .GroupBy(u => u.Jahr)                      // Gruppen: ein Jahr -> alle Unfälle dieses Jahres
                .ToDictionary(g => g.Key, g => g.Count()); // Reduce: Jahr -> Anzahl
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
            // Gleiches Muster wie oben, nur nach Typ statt nach Jahr gruppiert.
            return unfaelle
                .GroupBy(u => u.Typ)                       // Gruppen: ein Typ -> alle Unfälle dieses Typs
                .ToDictionary(g => g.Key, g => g.Count()); // Reduce: Typ -> Anzahl
        }

        /// <summary>
        /// Findet den Unfalltyp mit den meisten Unfällen (Reduce: viele Einträge → ein Ergebnis).
        /// </summary>
        /// <param name="proTyp">Das Ergebnis von <see cref="UnfaelleProTyp"/>.</param>
        /// <returns>Der häufigste Unfalltyp, oder "" wenn keine Daten vorhanden sind.</returns>
        public static string HaeufigsterTyp(Dictionary<string, int> proTyp)
        {
            // Imperativ: Rekord-Variablen (maxAnzahl, besterTyp) in einer Schleife aktualisieren.
            // Funktional: nach Anzahl absteigend sortieren und den ersten nehmen.
            // (OrderByDescending ist stabil: bei Gleichstand gewinnt wie in V1.0 der zuerst gefundene Typ.)
            return proTyp
                .OrderByDescending(eintrag => eintrag.Value) // grösste Anzahl zuerst
                .Select(eintrag => eintrag.Key)              // Map: nur den Typ-Namen behalten
                .FirstOrDefault() ?? "";                     // den ersten nehmen ("" wenn leer, wie V1.0)
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
            // Imperativ: zwei if-Abfragen mit return.
            // Funktional: ein einziger Ausdruck, der true oder false ergibt.
            return u.Schweregrad.Contains("Schwerverletzten")
                || u.Schweregrad.Contains("Getöteten");
        }

        /// <summary>
        /// Zählt pro Jahr nur die schweren Unfälle (Filter + Reduce).
        /// </summary>
        /// <param name="unfaelle">Alle eingelesenen Unfälle.</param>
        /// <returns>Dictionary: Jahr → Anzahl schwere Unfälle.</returns>
        public static Dictionary<int, int> SchwereUnfaelleProJahr(List<Unfall> unfaelle)
        {
            // Imperativ: Filter mit "continue" und Reduce mit Zähler in derselben Schleife.
            // Funktional: Filter (Where) und Reduce (GroupBy + Count) sind getrennte, lesbare Schritte.
            return unfaelle
                .Where(IstSchwererUnfall)                  // Filter: nur schwere Unfälle
                .GroupBy(u => u.Jahr)                      // nach Jahr gruppieren
                .ToDictionary(g => g.Key, g => g.Count()); // Reduce: Jahr -> Anzahl
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
            // Imperativ: zwei Zähler-Dictionaries in einer Schleife, danach zweite Schleife für die Prozente.
            // Funktional: pro Jahr gruppieren, dann jede Gruppe mit Select in einen Prozentwert umwandeln.
            return unfaelle
                .GroupBy(u => u.Jahr)                                   // Reduce: Gruppen pro Jahr
                .Select(g => new
                {
                    Jahr = g.Key,
                    Prozent = g.Where(IstSchwererUnfall).Count() * 100.0 // schwere Unfälle ...
                              / g.Count()                               // ... geteilt durch alle Unfälle
                })                                                      // Map: Gruppe -> Prozentwert
                .ToDictionary(x => x.Jahr, x => Math.Round(x.Prozent, 1));
        }
    }
}
