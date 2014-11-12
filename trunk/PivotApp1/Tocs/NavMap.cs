// --------------------------------
// <copyright file="NavMap.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Linq;

namespace EPubReader.Tocs
{
    /// <summary>
    /// Nav map.
    /// </summary>
    public class NavMap
    {
        /// <summary>
        /// Gets or sets the nav points.
        /// </summary>
        public Collection<NavPoint> NavPoints
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of NavMap class.
        /// </summary>
        public NavMap()
        {
        }

        internal NavMap(XElement navMapXElement)
        {
            // NavPoints
            Collection<NavPoint> navPoints = new Collection<NavPoint>();
            foreach (XElement navPointXElement in navMapXElement.Elements(Toc.DaisyOrgNcxNameSpace + "navPoint"))
            {
                navPoints.Add(new NavPoint(navPointXElement));
            }

            NavPoints = new Collection<NavPoint>(navPoints.OrderBy(np => np.PlayOrder).ToList());
        }
    }
}
