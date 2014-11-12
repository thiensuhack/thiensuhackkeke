// --------------------------------
// <copyright file="IsolatedStorageFileToImageSourceConverter.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace EPubReader.Converters
{
    /// <summary>
    /// Isolated storage file to image source converter.
    /// </summary>
    public class IsolatedStorageFileToImageSourceConverter : IValueConverter
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
            Elements.Image image = (Elements.Image)value;
            Book book = (Book)parameter;

            Stream stream;
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                stream = isf.ReadStream(Path.Combine(book.GetImagesDirectory(), image.FileName));
            }
            
            BitmapImage bi = new BitmapImage();
            bi.SetSource(stream);

            stream.Close();

            return bi;
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
