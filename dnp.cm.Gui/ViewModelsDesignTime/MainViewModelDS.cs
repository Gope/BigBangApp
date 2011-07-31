using System;
using System.Collections.ObjectModel;
using System.Linq;
using CaliburnMicro.Framework;
using dnp.cm.ApplicationSupport;
using dnp.cm.ApplicationSupport.DialogService;
using dnp.cm.ApplicationSupport.NotificationSystem;
using dnp.cm.Domain;
using dnp.cm.Gui.ViewModels;

namespace dnp.cm.Gui.ViewModelsDesignTime
{
    /// <summary>
    /// Erstellt Daten zur Verwendung innerhalb der DesignTime Ansicht von VisualStudio oder Blend.
    /// </summary>
    public class MainViewModelDS : MainViewModel
    {
        public MainViewModelDS()
        {
            RoleRepository rep = new RoleRepository();
			
            RoleViewModel model1 = new RoleViewModel(null, rep, null);
            model1.LoadRoleById(123);
            RoleViewModel model2 = new RoleViewModel(null, rep, null);
            model2.LoadRoleById(124);
			RoleViewModel model3 = new RoleViewModel(null, rep, null);
            model3.LoadRoleById(125);
			
			Items.Add(model1);
			Items.Add(model2);
			Items.Add(model3);

            Notifications.Add(new ApplicationNotification("Dies ist der Design-Mode! Dies ist der Design-Mode!", NotificationType.Information));
        }
    }
}