using System;

namespace dnp.cm.Domain
{
    /// <summary>
    /// Erstellt "zufällige" Werte für ein Alter um
    ///  das Nachladen / Aktualisieren eines Datensatzes 
    /// innerhalb der Oberfläche zu zeigen.
    /// </summary>
    public static class AgeSimulator
    {
        private static Random _Random = new Random();

        /// <summary>
        /// Gibt ein zufälliges Alter zwischen 25 und 35 Jahren zurück.
        /// </summary>
        /// <returns>Das ermittelte Alter.</returns>
        public static int GetAge()
        {
            return _Random.Next(25, 35);
        }
    }
}