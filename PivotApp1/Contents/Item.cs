// --------------------------------
// <copyright file="Item.cs" company="Cubicsoft (www.cubicsoft.pl)">
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
    /// Manifest item.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        public string Href
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the media type.
        /// </summary>
        public string MediaType
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of Item class.
        /// </summary>
        public Item()
        {
        }

        internal Item(XElement itemXElement)
        {
            Id = itemXElement.Attribute("id").Value;
            Href = itemXElement.Attribute("href").Value;
            MediaType = itemXElement.Attribute("media-type").Value;
        }
    }
}
