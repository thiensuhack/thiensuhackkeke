// --------------------------------
// <copyright file="ItemRef.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Xml.Linq;

namespace EPubReader.Contents
{
    /// <summary>
    /// Spine itemRef.
    /// </summary>
    public class ItemRef
    {
        /// <summary>
        /// Gets or sets the id of reference.
        /// </summary>
        public string Idref
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of ItemRef class.
        /// </summary>
        public ItemRef()
        {
        }

        internal ItemRef(XElement itemrefXElement)
        {
            Idref = itemrefXElement.Attribute("idref").Value;
        }
    }
}
