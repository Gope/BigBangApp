using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace dnp.cm.Domain
{
    /// <summary>
    /// Verwaltet die Serienrollen.
    /// </summary>
    public class RoleRepository : IRepository<Role>
    {
        private ObservableCollection<Role> _Roles;

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        public RoleRepository()
        {
            _Roles = new ObservableCollection<Role>
                             {
                                 new Role{ Id = 123, FirstSeenOnAirDate = new DateTime(2011,1,1), FirstName = "Sheldon", LastName = "Cooper"},
                                 new Role{ Id = 124, FirstSeenOnAirDate = new DateTime(2010,1,1), FirstName = "Leonard", LastName = "Hofstadter"},
                                 new Role{ Id = 125, FirstSeenOnAirDate = new DateTime(2009,1,1), FirstName = "Raj", LastName = "Koothrappali"},
                                 new Role{ Id = 126, FirstSeenOnAirDate = new DateTime(2008,1,1), FirstName = "Howard", LastName = "Wolowitz"},
                                 new Role{ Id = 127, FirstSeenOnAirDate = new DateTime(2007,1,1), FirstName = "Penny", LastName = "Unknown"}
                             };
        }

        #endregion

        #region Suchvarianten 

        /// <summary>
        /// Findet Rollen anhand ihrer Id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Die gefunde Rolle.</returns>
        public Role GetById(int id)
        {
            return (from role in _Roles
                    where role.Id == id
                    select role).FirstOrDefault();
        }

        /// <summary>
        /// Gibt alle gespielten Rollen zurück.
        /// </summary>
        /// <returns>Eine Liste von allen gespielten Rollen.</returns>
        public ObservableCollection<Role> GetAll()
        {
            return _Roles;
        }

        #endregion
    }
}