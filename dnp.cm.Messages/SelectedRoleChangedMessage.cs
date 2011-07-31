using dnp.cm.Domain;

namespace dnp.cm.Messages
{
    /// <summary>
    /// Repräsentiert eine Benachrichtigung, dass sich die aktuell 
    /// ausgewählte Rolle geändert hat.
    /// </summary>
    public class SelectedRoleChangedMessage
    {
        /// <summary>
        /// Der vorhergehend ausgewählte Darsteller.
        /// </summary>
        public Role OldRole { get; set; }

        /// <summary>
        /// Der neu ausgewählte Darsteller.
        /// </summary>
        public Role NewRole { get; set; }
    }
}
