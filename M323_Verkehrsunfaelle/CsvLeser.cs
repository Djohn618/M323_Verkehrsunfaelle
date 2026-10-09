// ============================================================================
// Datei:        CsvLeser.cs
// Beschreibung: Liest die Unfalldaten aus der CSV-Datei (Trennzeichen ";")
//               und macht daraus eine Liste von Unfall-Objekten.
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
    /// Liest Unfalldaten aus einer CSV-Datei ein.
    /// </summary>
    public class CsvLeser
    {
        /// <summary>
        /// Liest die CSV-Datei ein und erstellt für jede Zeile ein Unfall-Objekt.
        /// Die erste Zeile (Spaltennamen) und leere Zeilen werden übersprungen.
        /// </summary>
        /// <param name="pfad">Pfad zur CSV-Datei, z.B. "unfaelle.csv".</param>
        /// <returns>Liste mit allen eingelesenen Unfällen.</returns>
        public static List<Unfall> Lies(string pfad)
        {
            var unfaelle = new List<Unfall>();
            string[] zeilen = File.ReadAllLines(pfad);

            for (int i = 1; i < zeilen.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(zeilen[i])) continue;

                string[] spalten = zeilen[i].Split(';');

                int jahr = int.Parse(spalten[5]);
                string typ = spalten[3];
                string schwere = spalten[4];

                unfaelle.Add(new Unfall(jahr, typ, schwere));
            }

            return unfaelle;
        }
    }
}
