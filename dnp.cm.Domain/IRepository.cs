using System.Collections.ObjectModel;

namespace dnp.cm.Domain
{
    /// <summary>
    /// Verwaltet die gespielten Serienrollen.
    /// </summary>
    /// <typeparam name="T">Typ der verwalteten Entität</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Findet Rollen anhand ihrer Id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Die gefunde Rolle.</returns>
        T GetById(int id);

        /// <summary>
        /// Gibt alle gespielten Rollen zurück.
        /// </summary>
        /// <returns>Eine Liste von allen gespielten Rollen.</returns>
        ObservableCollection<T> GetAll();
    }
}