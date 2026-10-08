namespace M323_Verkehrsunfaelle
{
    internal class Program
    {
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
        }
    }
}
