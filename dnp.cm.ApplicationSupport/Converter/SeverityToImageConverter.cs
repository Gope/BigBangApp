using System;
using System.Globalization;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using dnp.cm.ApplicationSupport.NotificationSystem;

namespace dnp.cm.ApplicationSupport.Converter
{
    /// <summary>
    /// Konvertiert einen <see cref="NotificationType"/> zu einem passenden Icon.
    /// </summary>
    public class SeverityToImageConverter : IValueConverter
    {
        private Dictionary<NotificationType, string> _Icons = new Dictionary<NotificationType, string>(2);

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="SeverityToImageConverter"/> class.
        /// </summary>
        public SeverityToImageConverter()
        {
            _Icons.Add(NotificationType.Error, "exclamation-red.png");
            _Icons.Add(NotificationType.Information, "information.png");
        }

        #endregion

        #region Implementation of IValueConverter 

        /// <summary>
        /// Konvertiert einen Wert.
        /// </summary>
        /// <param name="value">Der von der Bindungsquelle erzeugte Wert.</param>
        /// <param name="targetType">Der Typ der Bindungsziel-Eigenschaft.</param>
        /// <param name="parameter">Der zu verwendende Konverterparameter.</param>
        /// <param name="culture">Die im Konverter zu verwendende Kultur.</param>
        /// <returns>
        /// Ein konvertierter Wert. Wenn die Methode null zurückgibt, wird der gültige NULL-Wert verwendet.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource source = null;

            var severity = (NotificationType)value;

            string localPathToIcon;
            if(_Icons.TryGetValue(severity,out localPathToIcon))
            {
                source = new BitmapImage(new Uri(string.Format("/dnp.cm.Gui;Component/Assets/{0}", localPathToIcon), UriKind.RelativeOrAbsolute));
            }

            return source;
        }

        /// <summary>
        /// Konvertiert einen Wert.
        /// </summary>
        /// <param name="value">Der Wert, der vom Bindungsziel erzeugt wird.</param>
        /// <param name="targetType">Der Typ, in den konvertiert werden soll.</param>
        /// <param name="parameter">Der zu verwendende Konverterparameter.</param>
        /// <param name="culture">Die im Konverter zu verwendende Kultur.</param>
        /// <returns>
        /// Ein konvertierter Wert. Wenn die Methode null zurückgibt, wird der gültige NULL-Wert verwendet.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}