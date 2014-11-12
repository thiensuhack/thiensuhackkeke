// --------------------------------
// <copyright file="Publisher.cs" company="Cubicsoft (www.cubicsoft.pl)">
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
    /// The publisher of the publication's content.
    /// </summary>
    public class Publisher : BasePurl
    {
        /// <summary>
        /// Initializes new instance of Publisher class.
        /// </summary>
        public Publisher()
        {
        }

        internal Publisher(IEnumerable<XElement> metadataXElements)
            : base("publisher", metadataXElements)
        {
        }
    }
}
