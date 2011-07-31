using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using CaliburnMicro.Framework;
using dnp.cm.ApplicationSupport.AppServices;
using dnp.cm.ApplicationSupport.DialogService;
using dnp.cm.ApplicationSupport.MVVM_Support;
using dnp.cm.CaliburnExtensions;
using dnp.cm.Domain;
using dnp.cm.Gui.ViewModels;
using dnp.cm.Gui.Views;

namespace dnp.cm.Gui
{
    /// <summary>
    /// Der konkrete Anwendungsbootstrapper. Initialisiert und konfiguriert alle für die Anwendung wichtigen Objekte. Diese 
    /// werden später über den Container aufgelöst und innerhalb der Anwendung bereitgestellt.
    /// </summary>
    public class AppBootstrapper : StructureMapBootstrapper<MainViewModel>
    {
        /// <summary>
        /// Konfiguriert den Container Anwendungsspezifisch.
        /// </summary>
        protected override void Configure()
        {
            // Container konfigurieren
            Container.Configure(x =>
            {
                // Caliburn Klassen für Window-Management und EventAggregation
                x.ForSingletonOf<IWindowManager>().Use<WindowManager>().Named("WindowManager");
                x.ForSingletonOf<IEventAggregator>().Use<EventAggregator>().Named("EventAggregator");

                // Service zum Anzeigen von Dialogen innerhalb der Anwendung
                x.For<IDialogService>().Use<ModalDialogService>();
                // Service zum Simulieren einer langlaufenden Operation
                x.For<IBackupService>().Use<LongRunningBackupService>();

                // Builder für das dynamische Erstellen von ViewModels innerhalb der Anwendung
                x.For<IRoleViewModelBuilder<RoleViewModel>>()
                    .Use(() => new RoleViewModelBuilder((name, roleId) =>
                        {
                            var viewModel = Container.GetInstance<RoleViewModel>();
                            viewModel.DisplayName = name;
                            viewModel.LoadRoleById(roleId);
                            return viewModel;
                        })).Named("RoleViewModelBuilder");

                // ViewModels registrieren für ViewModelLocator
                x.ForSingletonOf<MainViewModel>().Use<MainViewModel>().Named("MainViewModel");
                x.For<RoleViewModel>().Use<RoleViewModel>().Named("RoleViewModel");
                x.For<RoleView>().Use<RoleView>().Named("RoleView");

                // Repository
                x.ForSingletonOf<IRepository<Role>>().Use<RoleRepository>().Named("RoleRepository");

                // Alle Handler (IHandle) beim Event Aggregator registrieren.
                x.For(typeof(IHandle<>)).EnrichWith((context, instance) =>
                {
                    context.GetInstance<IEventAggregator>().Subscribe(instance);
                    return instance;
                });

            });
        }

        /// <summary>
        /// Override to tell the framework where to find assemblies to inspect for views, etc.
        /// </summary>
        /// <returns>
        /// A list of assemblies to inspect.
        /// </returns>
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] 
            {
                Assembly.GetExecutingAssembly(),
                Assembly.Load("dnp.cm.Gui")
            };
        }
    }
}