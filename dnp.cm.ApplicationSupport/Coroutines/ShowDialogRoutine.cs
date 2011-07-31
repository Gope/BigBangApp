using System;
using CaliburnMicro.Framework;
using dnp.cm.ApplicationSupport.DialogService;

namespace dnp.cm.ApplicationSupport.Coroutines
{
    /// <summary>
    /// IResult zum Anzeigen eines Dialogs.
    /// </summary>
    public class ShowDialogRoutine : IResult
    {
        #region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowDialogRoutine"/> class.
        /// </summary>
        /// <param name="dialogService">The dialog service.</param>
        public ShowDialogRoutine(IDialogService dialogService)
        {
            DialogService = dialogService;
        }

        #endregion
        
        #region Properties 

        public DialogImage DialogImage { get; set; }

        public DialogButton DialogButtons { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public IDialogService DialogService { get; set; }

        #endregion

        #region IResult Implementiations 

        /// <summary>
        /// Executes the result using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute(ActionExecutionContext context)
        {
            if(DialogResponse.OK == DialogService.ShowMessage(Message, Title, DialogButtons , DialogImage))
            {
                Completed(this, new ResultCompletionEventArgs{ WasCancelled = false });
            }
            else
            {
                Completed(this, new ResultCompletionEventArgs(){ WasCancelled = true, Error = new InvalidOperationException("Das geht ja gaaar nicht...") });
                DialogService.ShowMessage("Der Vorgang wurde abgebrochen", "Benutzerabruch", DialogButton.OK, DialogImage.Information);
            }
        }

        public event EventHandler<ResultCompletionEventArgs> Completed;

        #endregion
    }
}