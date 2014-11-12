// --------------------------------
// <copyright file="Description.cs" company="Cubicsoft (www.cubicsoft.pl)">
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
    /// Description of the publication's content.
    /// </summary>
    public class Description : BasePurl
    {
        /// <summary>
        /// Initializes new instance of Description class.
        /// </summary>
        public Description()
        {
        }

        internal Description(IEnumerable<XElement> metadataXElements)
            : base("description", metadataXElements)
        {
        }
    }
}
