// --------------------------------
// <copyright file="ItemElementsContainer.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EPubReader
{
    /// <summary>
    /// Item element container.
    /// </summary>
    public class ItemElementsContainer
    {
        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        public Contents.Item Item
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the elements.
        /// </summary>
        public Collection<Elements.BaseElement> Elements
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the start location.
        /// </summary>
        public int StartLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the end location.
        /// </summary>
        public int EndLocation
        {
            get 
            {
                if (Elements.Count > 0)
                {
                    return StartLocation + Elements.Count - 1;
                }
                else
                {
                    return StartLocation;
                }
            }
        }

        /// <summary>
        /// Gets the pages.
        /// </summary>
        internal List<Page> Pages
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes new instance of ItemElementsContainer class.
        /// </summary>
        public ItemElementsContainer()
        {
            Pages = new List<Page>();
        }
    }
}