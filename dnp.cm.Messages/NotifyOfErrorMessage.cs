namespace dnp.cm.Messages
{
    /// <summary>
    /// Repräsentiert einen Fehlerbenachrichtigung.
    /// </summary>
    public class NotifyOfErrorMessage
    {
        private readonly string _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotifyOfErrorMessage"/> class.
        /// </summary>
        /// <param name="message">Die Nachricht.</param>
        public NotifyOfErrorMessage(string message)
        {
            _message = message;
        }

        /// <summary>
        /// Die Nachricht, welche den Fehler enthält.
        /// </summary>
        public string Message
        {
            get { return _message; }
        }
    }
}