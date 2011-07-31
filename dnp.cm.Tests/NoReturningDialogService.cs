using System;
using dnp.cm.ApplicationSupport.DialogService;

namespace dnp.cm.Tests
{
    /// <summary>
    /// Testunterstützung: Dialog mit negativer Rückgabe.
    /// </summary>
    public class NoReturningDialogService : IDialogService
    {
        public DialogResponse ShowException(string message, DialogImage image)
        {
            return DialogResponse.No;
        }

        public DialogResponse ShowMessage(string message, string caption, DialogButton button, DialogImage image)
        {
            return DialogResponse.No;
        }

        public void CloseSplashScreen()
        {
            throw new NotImplementedException();
        }

        public void ShowSplashScreen()
        {
            throw new NotImplementedException();
        }
    }
}