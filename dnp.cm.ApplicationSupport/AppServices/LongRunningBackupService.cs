using System;
using System.Collections.Generic;
using System.Threading;
using CaliburnMicro.Framework;
using dnp.cm.ApplicationSupport.Coroutines;
using dnp.cm.ApplicationSupport.DialogService;

namespace dnp.cm.ApplicationSupport.AppServices
{
    /// <summary>
    /// Simuliert einen langlaufenden, blockierenden Service-Aufruf.
    /// </summary>
    public class LongRunningBackupService : IBackupService
    {
        #region Member 

        private readonly IEventAggregator _EventAggregator;
        private static bool _LastValue = false;
        private IDialogService _DialogService;

        #endregion

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="LongRunningBackupService"/> class.
        /// </summary>
        public LongRunningBackupService(IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _EventAggregator = eventAggregator;
            _DialogService = dialogService;
        }

        #endregion

        #region Service Logic 

        /// <summary>
        /// Simuliert einen dreisekündigen Aufruf. 
        /// </summary>
        /// <param name="callback">Die Callback Methode für den Rückruf zum Aufrufer.</param>
        public static void SimulateLongRunningOperation(Action<bool> callback)
        {
            Thread.Sleep(3000);
            callback(ReturnADifferentResultForEveryCall());
        }

        /// <summary>
        /// Gibt abwechselnd true und false zurück um verschiedene Aufrufvarianten zu simulieren.
        /// </summary>
        /// <returns></returns>
        private static bool ReturnADifferentResultForEveryCall()
        {
            _LastValue = !_LastValue;
            return _LastValue;
        }

        #endregion

        #region Coroutine 

        /// <summary>
        /// Dies ist eine Coroutine, welche eine logische Abfolge von Schritten repräsentiert. Diese werden vom 
        /// Caliburn Micro Framework abgearbeitet bis alle Schritte erledigt sind, oder ein Fehler aufgetreten ist.
        /// </summary>
        /// <returns>1 - n IResult Implementierungen.</returns>
        public IEnumerable<IResult> Start()
        {
            // 1) Nachfrage
            yield return new ShowDialogRoutine(_DialogService)
                             {
                                 Title = "Bestätigung",
                                 Message = "Möchten Sie das Backup starten? Dieser Vorgang dauert genau 3 Sekunden.",
                                 DialogButtons = DialogButton.OKCancel,
                                 DialogImage = DialogImage.Question
                             };

            // 2) Langlaufenden Service aufrufen
            yield return new BackupRoutine(_EventAggregator);

            // 3) Bestätigung
            yield return new ShowDialogRoutine(_DialogService)
                             {
                                 Title = "Erfolgreich",
                                 Message = "Das Backup ist geglückt!",
                                 DialogButtons = DialogButton.OK,
                                 DialogImage = DialogImage.Information
                             };
        }

        #endregion
    }
}