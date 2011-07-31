using System;
using System.Diagnostics;
using CaliburnMicro.Framework;

namespace dnp.cm.CaliburnExtensions
{
    /// <summary>
    /// Einfache Logger Implementierung für die direkte Ausgabe von Caliburn Micro Ereignissen im Output-Fenster von Visual Studio.
    /// </summary>
    public class DebugLogger : ILog
    {
        #region Member 

        private readonly Type _type;

        #endregion

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="DebugLogger"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public DebugLogger(Type type)
        {
            _type = type;
        }

        #endregion

        #region Ausgabe eines Ereignisses 

        private string CreateLogMessage(string format, params object[] args)
        {
            string message = "";
            if (args.Length == 1)
            {
                message = string.Format("[{0} - {1}]",
                                        string.Format(format, args[0]),
                                        DateTime.Now.ToString("T"));
            }
            else if (args.Length == 2)
            {
                message = string.Format("[{0} - {1}] {2}",
                                        args[1],
                                        DateTime.Now.ToString("T"),
                                        string.Format(format, ((object[])args[0])));
            }

            return message;
        }

        #endregion

        #region Konkrete Log-Stufen Methoden 

        /// <summary>
        /// Logs the message as info.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public void Info(string format, params object[] args)
        {
            Debug.WriteLine(CreateLogMessage(format, args, "INFO"));
        }

        /// <summary>
        /// Logs the message as a warning.
        /// </summary>
        /// <param name="format">A formatted message.</param>
        /// <param name="args">Parameters to be injected into the formatted message.</param>
        public void Warn(string format, params object[] args)
        {
            Debug.WriteLine(CreateLogMessage(format, args, "WARN"));
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Error(Exception exception)
        {
            Debug.WriteLine(CreateLogMessage(exception.ToString(), "ERROR"));
        }

        #endregion

    }
}