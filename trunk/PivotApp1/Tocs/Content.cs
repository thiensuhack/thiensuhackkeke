// --------------------------------
// <copyright file="Content.cs" company="Cubicsoft (www.cubicsoft.pl)">
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
    /// TOC content.
    /// </summary>
    public class Content
    {
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        public string Src
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of Content class.
        /// </summary>
        public Content()
        {
        }

        internal Content(XElement contentXElement)
        {
            // Src
            Src = contentXElement.Attribute("src").Value;            
        }
    }
}
