using System;
using dnp.cm.ApplicationSupport;
using dnp.cm.ApplicationSupport.MVVM_Support;

namespace dnp.cm.Gui.ViewModels
{
    /// <summary>
    /// Der ViewModelBuilder existiert aus verschiedenen Gründen. Innerhalb der Anwendung ist es 
    /// notwendig ViewModels dynamisch zu erstellen. Hierzu soll jedoch nicht der new-Operator verwendet 
    /// werden, sondern ein Dependency-Injection Container um die Kontrolle über die notwendingen 
    /// Abhängigkeiten zu verwalten und den Lebenszyklus des Objekts zu steuern. Um den Container 
    /// nicht als Service-Locator zu missbrauchen, bietet der <see cref="IRoleViewModelBuilder{T}<T>"/> die 
    /// Möglichkeit innerhalb der Container-Konfiguration einen Delegate zu nutzen um die Erstellung 
    /// des ViewModels zu übernehmen, ohne das die RRR (Register, Resolve, Release) Empfehlung 
    /// verletzt wird.
    /// 
    /// </summary>
    /// <typeparam name="T">Typ des zu bauenden ViewModels</typeparam>
    public class RoleViewModelBuilder : IRoleViewModelBuilder<RoleViewModel>
    {
        private readonly Func<string, int, RoleViewModel> _buildObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleViewModelBuilder"/> class.
        /// </summary>
        /// <param name="buildObject">The build object.</param>
        public RoleViewModelBuilder(Func<string, int, RoleViewModel> buildObject)
        {
            _buildObject = buildObject;
        }

        /// <summary>
        /// Builds up view model.
        /// </summary>
        /// <param name="displayName">Anzeigename des ViewModels.</param>
        /// <param name="roleId">RoleId</param>
        /// <returns></returns>
        public RoleViewModel BuildUpViewModel(string displayName, int roleId)
        {
            return _buildObject.Invoke(displayName, roleId);
        }
    }
}