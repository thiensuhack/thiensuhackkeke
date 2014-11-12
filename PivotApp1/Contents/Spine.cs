// --------------------------------
// <copyright file="Spine.cs" company="Cubicsoft (www.cubicsoft.pl)">
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
    /// Book spine.
    /// </summary>
    public class Spine
    {
        /// <summary>
        /// Gets or sets the item references.
        /// </summary>
        public Collection<ItemRef> ItemRefs
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of Spine class.
        /// </summary>
        public Spine()
        {
        }

        internal Spine(IEnumerable<XElement> spineXElements)
        {
            // ItemRefs
            ItemRefs = new Collection<ItemRef>();
            foreach (XElement itemrefXElement in spineXElements.Elements(Loader.IdpfOrg2007OpfNameSpace + "itemref"))
            {
                ItemRefs.Add(new ItemRef(itemrefXElement));
            }
        }
    }
}
