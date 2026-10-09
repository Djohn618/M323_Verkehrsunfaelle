// ============================================================================
// Datei:        Unfall.cs
// Beschreibung: Datentyp für einen einzelnen Verkehrsunfall (eine Zeile der CSV).
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
    /// Ein Verkehrsunfall mit Jahr, Unfalltyp und Schweregrad.
    /// Ein "record" kann nach dem Erstellen nicht mehr verändert werden (unveränderlich)
    /// und zwei Unfälle mit gleichen Werten gelten als gleich.
    /// </summary>
    /// <param name="Jahr">Das Unfalljahr, z.B. 2021.</param>
    /// <param name="Typ">Der Unfalltyp, z.B. "Auffahrunfall".</param>
    /// <param name="Schweregrad">Der Schweregrad, z.B. "2 Unfall mit Leichtverletzten".</param>
    public record Unfall(int Jahr, string Typ, string Schweregrad);
}
