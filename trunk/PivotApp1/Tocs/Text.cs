// --------------------------------
// <copyright file="Text.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Xml.Linq;

namespace EPubReader.Tocs
{
    /// <summary>
    /// TOC text.
    /// </summary>
    public class Text
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of Text class.
        /// </summary>
        public Text()
        {
        }

        internal Text(XElement textXElement)
        {
            // Value
            Value = textXElement.Value;            
        }
    }
}
