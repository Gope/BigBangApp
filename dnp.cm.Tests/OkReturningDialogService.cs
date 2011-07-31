using System;
using dnp.cm.ApplicationSupport.DialogService;

namespace dnp.cm.Tests
{
    public class OkReturningDialogService : IDialogService
    {
        public DialogResponse ShowException(string message, DialogImage image)
        {
            return DialogResponse.OK;
        }

        public DialogResponse ShowMessage(string message, string caption, DialogButton button, DialogImage image)
        {
            return DialogResponse.OK;
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