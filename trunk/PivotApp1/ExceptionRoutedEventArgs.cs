// --------------------------------
// <copyright file="ExceptionRoutedEventArgs.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System;
using System.Windows;

namespace EPubReader
{
    /// <summary>
    /// Provides event data for exceptions that are raised as events by asynchronous.
    /// </summary>
    public class ExceptionRoutedEventArgs : RoutedEventArgs
    {
        private Exception _errorException;

        /// <summary>
        /// Gets the underlying exception or native-level error reported by the event.
        /// </summary>
        public Exception ErrorException
        {
            get { return _errorException; }
        }

        /// <summary>
        /// Initializes a new instance of the NextoControlExceptionRoutedEventArgs class.
        /// </summary>
        /// <param name="errorException"></param>
        public ExceptionRoutedEventArgs(Exception errorException)
        {
            _errorException = errorException;
        }
    }
}
