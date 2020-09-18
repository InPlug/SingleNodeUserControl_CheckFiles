using System;
using System.Windows.Data;

namespace SingleNodeUserControl_CheckFiles
{
    /// <summary>
    /// Funktion: Dummy ValueConverter für Debugging-Zwecke.
    /// </summary>
    /// <remarks>
    /// File: NotEmptyToTrue.cs
    /// Autor: Erik Nagel
    ///
    /// 30.05.2013 Erik Nagel: erstellt
    /// </remarks>
    public class ObjectToObject : IValueConverter
    {
        /// <summary>
        /// Wandelt ein Object in ein Object.
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="targetType">object</param>
        /// <param name="parameter">Konvertierparameter</param>
        /// <param name="culture">Kultur</param>
        /// <returns>True, wenn der String nicht leer ist.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        /// <summary>
        /// Wandelt ein Object in ein Object.
        /// </summary>
        /// <param name="value">object</param>
        /// <param name="targetType">object</param>
        /// <param name="parameter">Konvertierparameter</param>
        /// <param name="culture">Kultur</param>
        /// <returns>Orientation (Horizontal, Vertical)</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
