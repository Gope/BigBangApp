using System;
using CaliburnMicro.Framework;
using dnp.cm.ApplicationSupport.AppServices;
using dnp.cm.Messages;

namespace dnp.cm.ApplicationSupport.Coroutines
{
    /// <summary>
    /// Ein <see cref="IResult"/>, welches eine langlaufende Operation aufruft.
    /// </summary>
    public class BackupRoutine : IResult
    {
        private IEventAggregator _EventAggregator;

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="BackupRoutine"/> class.
        /// </summary>
        /// <param name="eventAggregator">Der EventAggregator fürs Messaging.</param>
        public BackupRoutine(IEventAggregator eventAggregator)
        {
            _EventAggregator = eventAggregator;
        }

        #endregion

        #region Implementierung des IResults 

        /// <summary>
        /// Eigentliche Aktion der <see cref="BackupRoutine"/>
        /// </summary>
        public void Backup()
        {
            LongRunningBackupService
                .SimulateLongRunningOperation(isValid =>
                        {
                            // Wechselt!!! Jeder 2. Aufruf schlägt absichtlich fehl um das Fehlerhandling der Coroutines zu demonstrieren.
                            if(!isValid)
                            {
                                Completed(this, new ResultCompletionEventArgs
                                                    {
                                                        Error = new Exception("Datenbank hat besseres zu tun.")
                                                    });
                                _EventAggregator.Publish(new NotifyOfErrorMessage("Backup fehlgeschlagen: Datenbank hat besseres zu tun."));
                            }
                            else
                            {
                                Completed(this, new ResultCompletionEventArgs());
                            }
                        });
        }

        #endregion

        #region IResult Implementation 

        /// <summary>
        /// Executes the result using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute(ActionExecutionContext context)
        {
            // Zugriff auf Parameter wie in folgenden Zeilen:
            // Parameter parameter = context.Message.Parameters[0];
            // Bakup(parameter.Value);
            Backup();
        }

        /// <summary>
        /// Occurs when execution has completed.
        /// </summary>
        public event EventHandler<ResultCompletionEventArgs> Completed;

        #endregion

    }
}