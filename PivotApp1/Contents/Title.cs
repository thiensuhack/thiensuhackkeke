// --------------------------------
// <copyright file="Title.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Collections.Generic;
using System.Xml.Linq;

namespace EPubReader.Contents
{
    /// <summary>
    /// The title of the publication.
    /// </summary>
    public class Title : BasePurl
    {
        /// <summary>
        /// Initializes new instance of Title class.
        /// </summary>
        public Title()
        {
        }

        internal Title(IEnumerable<XElement> metadataXElements)
            : base("title", metadataXElements)
        {
        }
    }
}
