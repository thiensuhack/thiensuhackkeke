// --------------------------------
// <copyright file="Page.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Windows.Controls;
using System.Collections.Generic;

namespace EPubReader
{
    /// <summary>
    /// Displayable page.
    /// </summary>
    internal class Page
    {
        /// <summary>
        /// Gets the Canvas containing all controls.
        /// </summary>
        internal Canvas Content
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the start location.
        /// </summary>
        internal int StartLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the end location.
        /// </summary>
        internal int EndLocation
        {
            get;
            set;
        }

        internal List<AnchorRect> AnchorRects
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of Page class.
        /// </summary>
        internal Page()
        {
            Content = new Canvas();
            AnchorRects = new List<AnchorRect>();
        }
    }
}
