// --------------------------------
// <copyright file="NavigationPoint.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

namespace EPubReader
{
    /// <summary>
    /// Navigation point.
    /// </summary>
    /// <remarks>
    /// Binding object.
    /// </remarks>
    public class NavigationPoint
    {
        /// <summary>
        /// Gets the text of the navigation point.
        /// </summary>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the source of the navigation point.
        /// </summary>
        public string Src
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the level of the navigation point.
        /// </summary>
        public int Level
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of NavigationPoint class.
        /// </summary>
        public NavigationPoint()
        {
        }
    }
}
