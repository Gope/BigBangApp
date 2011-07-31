using System.Collections.Generic;
using CaliburnMicro.Framework;

namespace dnp.cm.ApplicationSupport.AppServices
{
    /// <summary>
    /// Vorgaben für einen Backup Service
    /// </summary>
    public interface IBackupService
    {
        /// <summary>
        /// Eine Sammlung von IResults für die Nutzung von Coroutines.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IResult> Start();
    }
}