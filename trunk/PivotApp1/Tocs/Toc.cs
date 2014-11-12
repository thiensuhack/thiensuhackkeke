// --------------------------------
// <copyright file="Toc.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EPubReader.Tocs
{
    /// <summary>
    /// Table o contents.
    /// </summary>
    public class Toc
    {
        internal const string DaisyOrgNcxNameSpace = "{http://www.daisy.org/z3986/2005/ncx/}";

        /// <summary>
        /// Gets or sets the nav map.
        /// </summary>
        public NavMap NavMap
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of Toc class.
        /// </summary>
        public Toc()
        {
        }

        internal Toc(IEnumerable<XElement> tocXElements)
        {
            // NavMap
            NavMap = new NavMap(tocXElements.Elements(DaisyOrgNcxNameSpace + "navMap").SingleOrDefault());
        }
    }
}
