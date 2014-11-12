// --------------------------------
// <copyright file="EPubReaderException.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System;

namespace EPubReader
{
    /// <summary>
    /// Represents errors that occur during application execution.
    /// </summary>
    public class EPubReaderException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the EPubReaderException class.
        /// </summary>
        public EPubReaderException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the EPubReaderException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EPubReaderException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the EPubReaderException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public EPubReaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
