using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace M323_Verkehrsunfaelle
{
    public class CsvLeser
    {
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
