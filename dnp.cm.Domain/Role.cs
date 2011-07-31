using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dnp.cm.Domain
{
    /// <summary>
    /// Beschreibt die Rolle eines Schauspielers innerhalb der Serie Big Bing Theory.
    /// </summary>
    public class Role
    {
        #region Properties 

        /// <summary>
        /// Systemeigene Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Der Vorname des gespielten Charakters.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Der Nachname des gespielten Charakters.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Der Vorname des gespielten Charakters.
        /// </summary>
        public DateTime FirstSeenOnAirDate { get; set; }

        /// <summary>
        /// Das Alter des gespielten Charakters.
        /// </summary>
        public int Age
        {
            get { return AgeSimulator.GetAge(); }
        }

        /// <summary>
        /// Das Bild des Darstellers.
        /// </summary>
        public string Image
        {
            get
            {
                return string.Format("{0}_small", Id);
            }
        }

        #endregion
    }
}
