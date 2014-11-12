// --------------------------------
// <copyright file="AssemblyInfo.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace EPubReader.Contents
{
    /// <summary>
    /// Book manifest.
    /// </summary>
    public class Manifest
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public Collection<Item> Items
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of Manifest class.
        /// </summary>
        public Manifest()
        {
        }

        internal Manifest(IEnumerable<XElement> manifestXElements)
        {
            // Items
            Items = new Collection<Item>();
            foreach (XElement itemXElement in manifestXElements.Elements(Loader.IdpfOrg2007OpfNameSpace + "item"))
            {
                Items.Add(new Item(itemXElement));
            }
        }
    }
}
