using System;
using System.Collections.ObjectModel;
using dnp.cm.Domain;
using dnp.cm.Gui.ViewModels;

namespace dnp.cm.Gui.ViewModelsDesignTime
{
    public class RoleViewModelDS : RoleViewModel
    {
        /// <summary>
        /// Erstellt Daten zur Verwendung innerhalb der DesignTime Ansicht von VisualStudio oder Blend.
        /// </summary>
        public RoleViewModelDS()
        {
            CurrentRole = new Role
                                  {
                                      Id = 123, 
                                      FirstSeenOnAirDate = new DateTime(2011, 1, 1), 
                                      FirstName = "Sheldon", 
                                      LastName = "Cooper"
                                  };
        }
    }
}