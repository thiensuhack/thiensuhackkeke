// --------------------------------
// <copyright file="AnchorRect.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Windows;

namespace EPubReader
{
    /// <summary>
    /// Anchor rectangle describes clickable element.
    /// </summary>
    internal class AnchorRect
    {
        internal string Href
        {
            get;
            set;
        }

        internal Rect Rect
        {
            get;
            set;
        }
    }
}
