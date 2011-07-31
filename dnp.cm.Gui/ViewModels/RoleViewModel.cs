using System;
using CaliburnMicro.Framework;
using dnp.cm.ApplicationSupport.DialogService;
using dnp.cm.Domain;
using dnp.cm.Gui.Views;
using dnp.cm.Messages;

namespace dnp.cm.Gui.ViewModels
{
    /// <summary>
    /// ViewModel für <see cref="RoleView"/>. Hauptaufgabe ist die Verwaltung des (einen) gekapselten <see cref="Role"/>-Objekts. 
    /// </summary>
    public class RoleViewModel : Screen
    {
        #region Member 

        private readonly IDialogService _DialogService;
        private readonly IRepository<Role> _Repository;
        private Role _CurrentRole;
        private bool _IsDirty;

        #endregion

        #region Constructors 

        public RoleViewModel()
        {
            // DesignTime support
            EventAggregator = new EventAggregator();
        }

        public RoleViewModel(IDialogService dialogService, IRepository<Role> repository,
                             IEventAggregator eventAggregator)
        {
            _DialogService = dialogService;
            _Repository = repository;
            EventAggregator = eventAggregator ?? new EventAggregator();
        }

        #endregion

        #region Properties 

        /// <summary>
        /// Bild des Darstellers in seiner Rolle.
        /// </summary>
        public string RoleImage
        {
            get
            {
                if (CurrentRole != null)
                    return string.Format("/dnp.cm.Gui;component/Assets/{0}_small.jpg", CurrentRole.Id);
                else
                    return null;
            }
        }

        /// <summary>
        /// Gibt an, ob Änderungen am aktuellen Datensatz vorgenommen wurde. Dies dient lediglich 
        /// der Vorstellung der CaliburnMicro Conductor-Abläufe.
        /// </summary>
        /// <value>
        ///   <c>true</c> wenn der Datensatz verändert wurde; ansonsten, <c>false</c>.
        /// </value>
        public bool IsDirty
        {
            get { return _IsDirty; }
            set
            {
                _IsDirty = value;
                NotifyOfPropertyChange(() => IsDirty);
            }
        }

        /// <summary>
        /// Ein <see cref="EventAggregtor" /> für message-basierte Kommunikation.
        /// </summary>
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// Model Implententierung eines <see cref="IDialogService"/> für das Anzeigen von MessageBoxen.
        /// </summary>
        public IDialogService DialogService
        {
            get { return _DialogService; }
        }

        /// <summary>
        /// Das vom ViewModel gekapselte Role-Objekt (Entität)
        /// </summary>
        /// <value>
        /// The current role.
        /// </value>
        public Role CurrentRole
        {
            get { return _CurrentRole; }
            set
            {
                var oldRole = _CurrentRole;
                _CurrentRole = value;
                NotifyOfPropertyChange(() => CurrentRole);
                EventAggregator.Publish(new SelectedRoleChangedMessage {OldRole = oldRole, NewRole = _CurrentRole});
            }
        }

        public IRepository<Role> Repository
        {
            get { return _Repository; }
        }

        #endregion

        #region Public Methods 

        /// <summary>
        /// Lädt eine Role anahand seiner Id aus dem Repository.
        /// </summary>
        /// <param name="id">The id.</param>
        public void LoadRoleById(int id)
        {
            CurrentRole = Repository.GetById(id);
            if (CurrentRole == null)
            {
                EventAggregator.Publish(
                    new NotifyOfErrorMessage(string.Format("Rolle mit Id: {0} wurde nicht gefunden.", id)));
            }
        }

        #endregion

        #region CM Screen Member Overrides 

        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected override void OnInitialize()
        {
            // Hier kann eine Initialisierung des ViewModels durchgeführt werden.    
        }

        /// <summary>
        /// Called when activating.
        /// </summary>
        protected override void OnActivate()
        {
            // Aktives Nachladen einer Rolle bei Aktivierung des zugehörigen Tabs. Hier sollte in einer produktiven Anwendung 
            // natürlich nur mit Zustimmung des Users nachgeladen werden.
            if (CurrentRole != null)
            {
                Repository.GetById(CurrentRole.Id);
            }
        }

        public override void TryClose(bool? dialogResult)
        {
            if (IsDirty)
            {
                if (DialogResponse.Yes != DialogService.ShowMessage("Es liegt eine Änderung im aktuellen Datensatz vor. Möchten Sie diese übernehmen?",
                                            "Änderungen verwerfen?",
                                            DialogButton.YesNo,
                                            DialogImage.Question))
                {
                    return;
                }
            }

            base.TryClose();
        }

        #endregion
    }
}