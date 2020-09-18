using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SingleNodeUserControl
{
    /// <summary>
    /// Funktion: ValueConverter, wandelt einen Prozentwert (int: 0-n) in Farben:
    /// 0 - 99: rot, 100 - 700: rot-orange bis grün fließend, > 700: grün.
    /// </summary>
    /// <remarks>
    /// File: PercentToBrush.cs
    /// Autor: Erik Nagel
    ///
    /// 28.12.2014 Erik Nagel: erstellt
    /// </remarks>
    [ValueConversion(typeof(int), typeof(SolidColorBrush))]
    public class PercentToBrush : IValueConverter
    {
        /// <summary>
        /// Wandelt einen Prozentwert (int: 0-n) in Farben:
        /// 0 - 99: rot, 100 - 700: rot-orange bis grün fließend, > 700: grün.
        /// </summary>
        /// <param name="value">Prozentwert.</param>
        /// <param name="targetType">Brush-Typ</param>
        /// <param name="parameter">Konvertierparameter</param>
        /// <param name="culture">Kultur</param>
        /// <returns>SolidColorBrush (Farbe zwischen rot und Grün)</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return new SolidColorBrush(Colors.Orange);
            }
            try
            {
                if ((int)value < 100)
                {
                    return new SolidColorBrush(Colors.DarkRed);
                }
                else
                {
                    if ((int)value > 700)
                    {
                        return new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        int normVal = ((int)value - 100) * 255 / 600;
                        return new SolidColorBrush(Color.FromArgb(255, (byte)(255 - normVal), (byte)(normVal), 110));
                    }
                }
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                return new SolidColorBrush(Colors.LightGray);
            }
        }

        /// <summary>
        /// Wandelt eine Farbe (SolidColorBrush)
        /// in einen Prozentwert (int).
        /// </summary>
        /// <param name="value">SolidColorBrush</param>
        /// <param name="targetType">Prozentwert (int)</param>
        /// <param name="parameter">Konvertierparameter</param>
        /// <param name="culture">Kultur</param>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
