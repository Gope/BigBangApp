using System;

namespace dnp.cm.ApplicationSupport.NotificationSystem
{
    /// <summary>
    /// Repräsentiert verschiedene Anwendungsmitteilungen (Warning, Info, Fehler).
    /// </summary>
    public class ApplicationNotification
    {
        #region Properties 

        /// <summary>
        /// Schweregrad der Mitteilung
        /// </summary>
        public NotificationType Severity { get; set; }

        /// <summary>
        /// Der Nachrichteninhalt.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Zeitpunkt der Nachrichtenerstellung
        /// </summary>
        public DateTime TimeStamp { get; private set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationNotification"/> class.
        /// </summary>
        /// <param name="message">Die Nachricht dieser Meldung.</param>
        /// <param name="severity">Spezifiziert die Art der Meldung.</param>
        public ApplicationNotification(string message, NotificationType severity)
        {
            Message = message;
            Severity = severity;
            TimeStamp = DateTime.Now;
        }
    }
}