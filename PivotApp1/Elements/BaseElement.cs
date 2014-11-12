// --------------------------------
// <copyright file="BaseElement.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Collections.ObjectModel;

namespace EPubReader.Elements
{
    /// <summary>
    /// Base element.
    /// </summary>
    public abstract class BaseElement
    {
        /// <summary>
        /// Standard margin.
        /// </summary>
        internal const double Margin = 12;

        /// <summary>
        /// Gets or sets the identifiers.
        /// </summary>
        public Collection<string> Identifiers
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of BaseElement class.
        /// </summary>
        protected BaseElement()
        {
            Identifiers = new Collection<string>();
        }
    }
}
