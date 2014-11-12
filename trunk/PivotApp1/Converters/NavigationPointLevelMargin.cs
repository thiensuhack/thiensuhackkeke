// --------------------------------
// <copyright file="NavigationPointLevelMargin.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EPubReader.Converters
{
    /// <summary>
    /// NavigationPoint.Level Margin converter.
    /// </summary>
    public class NavigationPointLevelMargin : IValueConverter
    {
        /// <summary>
        /// Converts NavigationPoint.Level to Thickness.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The System.Type of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness(12 + ((int)value * 48), 0, 12, 17);
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The System.Type of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
