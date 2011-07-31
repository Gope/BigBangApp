using System;
using System.Linq;
using System.Text;
using CaliburnMicro.Framework;
using dnp.cm.ApplicationSupport.AppServices;
using dnp.cm.ApplicationSupport.Coroutines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dnp.cm.Tests
{
    /// <summary>
    /// Schmalspur Testfälle für Coroutines. Diese zeigen lediglich 
    /// die Möglichkeiten für das Testen von Coroutines in UnitTests.
    /// </summary>
    [TestClass]
    public class LongRunningBackupServiceFixture
    {
        /// <summary>
        /// Testet den Ablauf einer Coroutine explizit (alle Schritte müssen in der 
        /// korrekten Reihenfolge erfolgen).
        /// </summary>
        [TestMethod]
        public void TestLongRunningBackupServiceCoroutineFlow()
        {
            var enumerator = new CoroutineEnumerator(new LongRunningBackupService(null, new OkReturningDialogService()).Start());

            // kein Assert nötig, wenn nicht vorhanden erfolgt eine InvalidOperationException vom CoroutineEnumerator
            enumerator.Next<ShowDialogRoutine>();
            enumerator.Next<BackupRoutine>();
            enumerator.Next<ShowDialogRoutine>();
        }

        /// <summary>
        /// Testet den korrekten Aufbau einzelner Schritte.
        /// </summary>
        [TestMethod]
        public void TestShowDialogsOfLongRunningBackupServiceCoroutine()
        {
            var enumerator = new CoroutineEnumerator(new LongRunningBackupService(null, new OkReturningDialogService()).Start());

            ShowDialogRoutine showDialogRoutineStep1 = enumerator.FindNext<ShowDialogRoutine>();
            Assert.IsTrue(showDialogRoutineStep1.Title.Equals("Bestätigung"), "Schritt 1: ShowDialogRoutine gefunden, aber mit ungültiger Eigenschaft Title: {0}", showDialogRoutineStep1.Title);

            ShowDialogRoutine showDialogRoutineStep3 = enumerator.FindNext<ShowDialogRoutine>();
            Assert.IsTrue(showDialogRoutineStep3.Title.Equals("Erfolgreich"), "Schritt 3: ShowDialogRoutine gefunden, aber mit ungültiger Eigenschaft Title: {0}", showDialogRoutineStep3.Title);
        }

        [TestMethod]
        public void TestShowDialogRoutineWithPositiveFeedbackFromUser()
        {
            ResultCompletionEventArgs completionArgs = null;
            var routine = new ShowDialogRoutine(new OkReturningDialogService());
            routine.Completed += (sender, args) => { completionArgs = args; };
            
            routine.Execute(new ActionExecutionContext());

            Assert.IsNotNull(completionArgs);
            Assert.IsFalse(completionArgs.WasCancelled);
            Assert.IsNull(completionArgs.Error);
        }

        [TestMethod]
        public void TestShowDialogRoutineWithNegativeFeedbackFromUser()
        {
            ResultCompletionEventArgs completionArgs = null;
            var routine = new ShowDialogRoutine(new NoReturningDialogService());
            routine.Completed += (sender, args) => { completionArgs = args; };

            routine.Execute(new ActionExecutionContext());

            Assert.IsNotNull(completionArgs);
            Assert.IsTrue(completionArgs.WasCancelled);
            Assert.IsTrue(completionArgs.Error.Message.Equals("Das geht ja gaaar nicht..."));
        }
    }
}
