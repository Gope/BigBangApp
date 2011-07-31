namespace dnp.cm.ApplicationSupport.MVVM_Support
{
    /// <summary>
    /// Der ViewModelBuilder existiert aus verschiedenen Gründen. Innerhalb der Anwendung ist es 
    /// notwendig ViewModels dynamisch zu erstellen. Hierzu soll jedoch nicht der new-Operator verwendet 
    /// werden, sondern ein Dependency-Injection Container um die Kontrolle über die notwendingen 
    /// Abhängigkeiten zu verwalten und den Lebenszyklus des Objekts zu steuern. Um den Container 
    /// nicht als Service-Locator zu missbrauchen, bietet der <see cref="IRoleViewModelBuilder{T}"/> die 
    /// Möglichkeit innerhalb der Container-Konfiguration einen Delegate zu nutzen um die Erstellung 
    /// des ViewModels zu übernehmen, ohne das die RRR (Register, Resolve, Release) Empfehlung 
    /// verletzt wird.
    /// 
    /// </summary>
    /// <typeparam name="T">Typ des zu bauenden ViewModels</typeparam>
    public interface IRoleViewModelBuilder<T>
    {
        /// <summary>
        /// Erstellt ein ViewModel anhand der vorgegebenen Methodik.
        /// </summary>
        /// <param name="displayName">Der Anzeigename</param>
        /// <param name="roleId">Die Kundennummer</param>
        /// <returns></returns>
        T BuildUpViewModel(string displayName, int roleId);
    }
}