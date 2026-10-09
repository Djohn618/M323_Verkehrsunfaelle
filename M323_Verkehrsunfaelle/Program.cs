// ============================================================================
// Datei:        Program.cs
// Beschreibung: Startpunkt des Programms. Liest die Unfalldaten ein, gibt die
//               Auswertungen von V1.0 (imperativ) aus und vergleicht danach
//               ALLE 6 Produktfunktionen von V1.0 mit V2.0 (funktional).
// Autor:        Deshan John Kothalawalage
// Version:      2.0
// ============================================================================

namespace M323_Verkehrsunfaelle
{
    /// <summary>
    /// Hauptprogramm: Einlesen, Auswerten und Vergleichen von V1.0 und V2.0.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Startpunkt des Programms.
        /// </summary>
        /// <param name="args">Kommandozeilen-Argumente (werden nicht verwendet).</param>
        static void Main(string[] args)
        {
            List<Unfall> unfaelle = CsvLeser.Lies("unfaelle.csv");

            Console.WriteLine($"Eingelesen: {unfaelle.Count} Unfaelle\n");

            //Die ersten 5 zur Kontrolle ausgeben
            for (int i = 0; i < 5; i++)
            {
                Unfall u = unfaelle[i];
                Console.WriteLine($"{u.Jahr} | {u.Typ} | {u.Schweregrad}");
            }

            Console.WriteLine("\n--- Unfalle pro Jahr (v1.0 imperativ)\n");

            var proJahr = AnalyseImperativ.UnfaelleProJahr(unfaelle);

            foreach (var eintrag in proJahr)
            {
                // eintrag.Key = Jahr, eintrag.Value = Anzahl
                Console.WriteLine($"{eintrag.Key}: {eintrag.Value}");
            }

            Console.WriteLine("\n--- Unfälle pro Typ (V1.0 imperativ) ---\n");

            var proTyp = AnalyseImperativ.UnfaelleProTyp(unfaelle);

            foreach (var eintrag in proTyp)
            {
                Console.WriteLine($"{eintrag.Key}: {eintrag.Value}");
            }

            string haeufigster = AnalyseImperativ.HaeufigsterTyp(proTyp);
            Console.WriteLine($"\nHäufigster Unfalltyp: {haeufigster}");

            Console.WriteLine("\n--- Schwere Unfälle pro Jahr (V1.0 imperativ) ---\n");

            var schwereProJahr = AnalyseImperativ.SchwereUnfaelleProJahr(unfaelle);

            foreach (var eintrag in schwereProJahr)
            {
                // eintrag.Key = Jahr, eintrag.Value = Anzahl schwere Unfälle
                Console.WriteLine($"{eintrag.Key}: {eintrag.Value}");
            }

            // ===== Restliche Funktionen V1.0 (imperativ) =====
            int abJahr = 2020;
            var abJahrListe = AnalyseImperativ.UnfaelleAbJahr(unfaelle, abJahr);
            var kurzListe = AnalyseImperativ.KurzeBezeichnungen(unfaelle);
            var anteilSchwer = AnalyseImperativ.AnteilSchwererUnfaelleProJahr(unfaelle);

            // ===== V2.0 funktional (LINQ) =====
            var abJahrListeF = AnalyseFunktional.UnfaelleAbJahr(unfaelle, abJahr);
            var kurzListeF = AnalyseFunktional.KurzeBezeichnungen(unfaelle);
            var proJahrF = AnalyseFunktional.UnfaelleProJahr(unfaelle);
            var proTypF = AnalyseFunktional.UnfaelleProTyp(unfaelle);
            string haeufigsterF = AnalyseFunktional.HaeufigsterTyp(proTypF);
            var schwereProJahrF = AnalyseFunktional.SchwereUnfaelleProJahr(unfaelle);
            var anteilSchwerF = AnalyseFunktional.AnteilSchwererUnfaelleProJahr(unfaelle);

            // ===== Vergleich aller 6 Produktfunktionen =====
            Console.WriteLine("\n===== Vergleich V1.0 (imperativ) vs. V2.0 (funktional) =====");

            VergleicheListen($"1. Unfälle ab {abJahr} (filter)", abJahrListe, abJahrListeF);
            VergleicheListen("2. Kurze Bezeichnungen (map)", kurzListe, kurzListeF);
            Vergleiche("3. Unfälle pro Jahr (reduce)", proJahr, proJahrF);
            Vergleiche("4a. Unfälle pro Typ (reduce)", proTyp, proTypF);

            Console.WriteLine("\n--- 4b. Häufigster Unfalltyp (reduce) ---\n");
            Console.WriteLine($"V1.0: {haeufigster}");
            Console.WriteLine($"V2.0: {haeufigsterF}");
            Console.WriteLine(haeufigster == haeufigsterF ? "=> identisch" : "=> UNTERSCHIED!");

            Vergleiche("5. Schwere Unfälle pro Jahr (filter + reduce)", schwereProJahr, schwereProJahrF);
            Vergleiche("6. Anteil schwerer Unfälle pro Jahr in % (map + reduce)", anteilSchwer, anteilSchwerF);
        }

        /// <summary>
        /// Gibt zwei Dictionary-Ergebnisse (V1.0 und V2.0) als Tabelle nebeneinander aus
        /// und prüft, ob alle Werte gleich sind.
        /// </summary>
        /// <typeparam name="TKey">Typ des Schlüssels, z.B. int (Jahr) oder string (Typ).</typeparam>
        /// <typeparam name="TValue">Typ des Werts, z.B. int (Anzahl) oder double (Prozent).</typeparam>
        /// <param name="titel">Überschrift der Tabelle.</param>
        /// <param name="v1">Ergebnis von V1.0 (imperativ).</param>
        /// <param name="v2">Ergebnis von V2.0 (funktional).</param>
        static void Vergleiche<TKey, TValue>(string titel, Dictionary<TKey, TValue> v1, Dictionary<TKey, TValue> v2)
            where TKey : notnull
        {
            Console.WriteLine($"\n--- {titel} ---\n");
            Console.WriteLine($"{"Schlüssel",-45} {"V1.0",6} {"V2.0",6}  gleich?");

            bool allesGleich = v1.Count == v2.Count;

            foreach (var eintrag in v1)
            {
                bool vorhanden = v2.TryGetValue(eintrag.Key, out TValue? wertV2);
                bool gleich = vorhanden && Equals(eintrag.Value, wertV2);
                if (!gleich) allesGleich = false;

                string anzeigeV2 = vorhanden ? $"{wertV2}" : "fehlt";
                Console.WriteLine($"{eintrag.Key,-45} {eintrag.Value,6} {anzeigeV2,6}  {(gleich ? "ja" : "NEIN")}");
            }

            Console.WriteLine(allesGleich ? "=> identisch" : "=> UNTERSCHIED!");
        }

        /// <summary>
        /// Vergleicht zwei Unfall-Listen (V1.0 und V2.0): gibt die Anzahl und die ersten
        /// Einträge beider Listen aus und prüft, ob die Listen Element für Element gleich sind.
        /// </summary>
        /// <param name="titel">Überschrift des Vergleichs.</param>
        /// <param name="v1">Ergebnis von V1.0 (imperativ).</param>
        /// <param name="v2">Ergebnis von V2.0 (funktional).</param>
        static void VergleicheListen(string titel, List<Unfall> v1, List<Unfall> v2)
        {
            Console.WriteLine($"\n--- {titel} ---\n");
            Console.WriteLine($"Anzahl V1.0: {v1.Count}   Anzahl V2.0: {v2.Count}\n");

            // Zur Kontrolle die ersten 3 Einträge beider Versionen anzeigen
            for (int i = 0; i < 3 && i < v1.Count && i < v2.Count; i++)
            {
                Console.WriteLine($"V1.0: {v1[i].Jahr} | {v1[i].Typ} | {v1[i].Schweregrad}");
                Console.WriteLine($"V2.0: {v2[i].Jahr} | {v2[i].Typ} | {v2[i].Schweregrad}");
            }

            // SequenceEqual prüft: gleich lang und jedes Element an derselben Stelle gleich
            bool gleich = v1.SequenceEqual(v2);
            Console.WriteLine(gleich ? "=> identisch" : "=> UNTERSCHIED!");
        }
    }
}
