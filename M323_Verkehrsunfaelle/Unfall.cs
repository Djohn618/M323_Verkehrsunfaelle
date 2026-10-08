using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M323_Verkehrsunfaelle
{
    // Represents a traffic accident with its year, type, and severity.
    public record Unfall(int Jahr, string Typ, string Schweregrad);
}
