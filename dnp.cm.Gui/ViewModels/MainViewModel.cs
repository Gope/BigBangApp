using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using CaliburnMicro.Framework;
using dnp.cm.ApplicationSupport.AppServices;
using dnp.cm.ApplicationSupport.DialogService;
using dnp.cm.ApplicationSupport.MVVM_Support;
using dnp.cm.ApplicationSupport.NotificationSystem;
using dnp.cm.CaliburnExtensions;
using dnp.cm.Domain;
using dnp.cm.Messages;
using Action = CaliburnMicro.Framework.Action;

namespace dnp.cm.Gui.ViewModels
{
    /// <summary>
    /// Haupt-ViewModel und Anwendungseinstiegspunkt.
    /// </summary>
    public class MainViewModel : Conductor<IScreen>.Collection.OneActive,
                                 IHandle<SelectedRoleChangedMessage>,
                                 IHandle<NotifyOfErrorMessage>
    {
        #region Member 

        private ObservableCollection<ApplicationNotification> _Notifications;
        public IRoleViewModelBuilder<RoleViewModel> RoleViewModelBuilder { get; set; }
        public EventAggregator EventAggregator { get; set; }
        private readonly IDialogService _DialogService;
        private readonly IBackupService _BackupService;

        #endregion

        #region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            // DesignTime Support
            _Notifications = new ObservableCollection<ApplicationNotification>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="dialogService">The dialog service.</param>
        /// <param name="roleViewModelBuilder">The view model builder.</param>
        public MainViewModel(IDialogService dialogService, IRoleViewModelBuilder<RoleViewModel> roleViewModelBuilder, IBackupService backupService) 
            : this()
        {
            RoleViewModelBuilder = roleViewModelBuilder;
            _DialogService = dialogService;
            _BackupService = backupService;
            DisplayName = "The MainView!";
        }

        #endregion

        #region Properties 

        /// <summary>
        /// Enthält alle systemweiten Nachrichten.
        /// </summary>
        public ObservableCollection<ApplicationNotification> Notifications
        {
            get { return _Notifications; }
        }

        /// <summary>
        /// DialogService für die Anzeige modaler Dialoge aus dem ViewModel heraus.
        /// </summary>
        public IDialogService DialogService
        {
            get { return _DialogService; }
        }

        /// <summary>
        /// Bild der gespielten Rolle
        /// </summary>
        public string RoleImage
        {
            get
            {
                if (CurrentRole != null)
                    return string.Format("/dnp.cm.Gui;component/Assets/{0}.jpg", CurrentRole.Id);
                else
                    return null;
            }
        }

        /// <summary>
        /// Die aktuell ausgewählte Rolle.
        /// </summary>
        public Role CurrentRole
        {
            get
            {
                if(ActiveItem != null)
                    return (ActiveItem as RoleViewModel).CurrentRole;

                return null;
            }
        }

        /// <summary>
        /// Convenience-Methode für einfaches Databinding
        /// </summary>
        /// <value>
        /// The full name of the role.
        /// </value>
        public string RoleFullName
        {
            get
            {
                if (CurrentRole != null)
                    return string.Format("{0}, {1}", CurrentRole.LastName, CurrentRole.FirstName);
                else
                    return null;
            }
        }

        #endregion

        #region ICommands + Actions

        /// <summary>
        /// Simuliert eine langlaufende Operation mit Coroutines
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IResult> BackupData()
        {
            return _BackupService.Start();
        }

        /// <summary>
        /// Der Guard für <see cref="BackupData"/>.
        /// </summary>
        /// <value>
        /// 	<c>true</c> wenn kein Reiter geöffnet ist; ansonsten, <c>false</c>.
        /// </value>
        public bool CanBackupData
        {
            get
            {
                return ActiveItem != null;
            }
        }

        /// <summary>
        /// Öffnet einen neuen Reiter mit angehängtem <see cref="RoleViewModel"/> durch Einfügen in die 
        /// ActiveItems Collection.
        /// </summary>
        /// <param name="RoleId">die Id der zu öffnenden Rolle.</param>
        public void OpenNewRoleView(string RoleId)
        {
            int result;
            if (int.TryParse(RoleId, out result) 
                    && result >= 123 
                    && result <= 127)
            {
                bool idAlreadyInOpenedItems = ItemsAsList.Any(viewModel => viewModel.CurrentRole.Id == result);

                if (!idAlreadyInOpenedItems)
                {
                    var model = RoleViewModelBuilder.BuildUpViewModel(RoleId, result);
                    ActivateItem(model);
                    NotifyOfPropertyChange(() => CanCloseActiveItem);
                    NotifyOfPropertyChange(() => CanBackupData);
                    NotifyOfPropertyChange(() => CanOpenNewRoleView);
                }
                else
                {
                    DialogService.ShowMessage(string.Format("Der Datensatz '{0}' ist bereits offen.", result),
                                              "Information", DialogButton.OK, DialogImage.Information);
                }

            }
            else
            {
                if (string.IsNullOrEmpty(RoleId))
                    RoleId = "[leer]";
                NewAppError(string.Format("{0} ist keine gültige Kundennummer (123 - 127)!", RoleId));
            }
        }

        /// <summary>
        /// Gibt an, ob noch weitere Roles geöffnet werden können.
        /// </summary>
        /// <value>
        /// 	<c>true</c> wenn max. 3 Roles geöffnet sind; andernfalls, <c>false</c>.
        /// </value>
        public bool CanOpenNewRoleView
        {
            get
            {
                return Items.Count <= 3;
            }
        }

        /// <summary>
        /// Schließt die geöffnete Role.
        /// </summary>
        public void CloseActiveItem()
        {
            if (ActiveItem != null)
                ((RoleViewModel)ActiveItem).TryClose(true);
            
            NotifyOfPropertyChange(() => CanCloseActiveItem);
            NotifyOfPropertyChange(() => CanBackupData);
            NotifyOfPropertyChange(() => CanOpenNewRoleView);
            NotifyOfPropertyChange(() => CurrentRole);
            NotifyOfPropertyChange(() => RoleImage);
            NotifyOfPropertyChange(() => RoleFullName);
        }

        /// <summary>
        /// Guard für das Schließen eines Items.
        /// </summary>
        /// <value>
        /// 	<c>true</c> wenn mindestens ein Reiter offen ist; andernfalls, <c>false</c>.
        /// </value>
        public bool CanCloseActiveItem
        {
            get
            {
                return Items.Count > 0;
            }
        }

        #endregion

        #region Privates  

        /// <summary>
        /// Alle Items als Liste von <see cref="RoleViewModel"/>s.
        /// </summary>
        private IEnumerable<RoleViewModel> ItemsAsList
        {
            get { return Items.Cast<RoleViewModel>(); }
        }

        /// <summary>
        /// Fügt eine neue Systemnachricht ins Log ein.
        /// </summary>
        /// <param name="message">Die Nachricht.</param>
        private void NewAppInformation(string message)
        {
            Notifications.Insert(0, new ApplicationNotification(message, NotificationType.Information));
        }

        /// <summary>
        /// Fügt eine neue Fehlernachricht ins Log ein.
        /// </summary>
        /// <param name="message">Die Nachricht.</param>
        private void NewAppError(string message)
        {
            Notifications.Insert(0, new ApplicationNotification(message, NotificationType.Error));
        }


        #endregion

        #region CM Screen Member Overrides

        protected override void OnInitialize()
        {
            // Initialisierungslogik
            base.OnInitialize();
        }

        protected override void OnActivate()
        {
            // Aktivierungslogik
            base.OnActivate();
        }

        /// <summary>
        /// Called when an attached view's Loaded event fires.
        /// </summary>
        /// <param name="view"></param>
        protected override void OnViewLoaded(object view)
        {
            // First Time init
            OpenNewRoleView("123");
            OpenNewRoleView("124");
            OpenNewRoleView("127");
        }

        /// <summary>
        /// Aktiviert ein neues ViewModel im TabControl.
        /// </summary>
        /// <param name="item">Das zu aktivierende item.</param>
        public override void ActivateItem(IScreen item)
        {
            base.ActivateItem(item);

            NotifyOfPropertyChange(() => CurrentRole);
            NotifyOfPropertyChange(() => RoleImage);
            NotifyOfPropertyChange(() => RoleFullName);
        }

        /// <summary>
        /// Schließen der Anwendung aus dem ViewModel heraus.
        /// </summary>
        public void CloseMainView()
        {
            // Caliburn Micro Funktionalität (Screen).
            TryClose();
        }

        /// <summary>
        /// Entscheidet, ob die Anwendung geschlossen werden kann durch Rückfrage beim Anwender.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public override void CanClose(Action<bool> callback)
        {
            callback(DialogResponse.Yes == DialogService.ShowMessage("Möchten Sie die Anwendung wirklich schließen?", 
                                                                        "End Application", 
                                                                        DialogButton.YesNo, 
                                                                        DialogImage.Information));

        }

        #endregion

        #region Event Aggregator Message Handling

        /// <summary>
        /// Verarbeitet auftretende <see cref="SelectedRoleChangedMessage"/>s.
        /// </summary>
        /// <param name="message">Die Nachricht.</param>
        public void Handle(SelectedRoleChangedMessage message)
        {
            if (message.NewRole != null)
            {
                NewAppInformation(string.Format("Kunde {0} wurde geladen.", message.NewRole.Id));
            }
        }

        /// <summary>
        /// Verarbeitet auftretende <see cref="NotifyOfErrorMessage"/>s.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(NotifyOfErrorMessage message)
        {
            if(message != null)
                NewAppError(message.Message);
        }

        #endregion
    }
}