// --------------------------------
// <copyright file="NavPoint.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace EPubReader.Tocs
{
    /// <summary>
    /// Nav point.
    /// </summary>
    public class NavPoint
    {
        /// <summary>
        /// Gets or sets the play order.
        /// </summary>
        public int PlayOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the nav label.
        /// </summary>
        public NavLabel NavLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public Content Content
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the nav points.
        /// </summary>
        public Collection<NavPoint> NavPoints
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of NavPoint class.
        /// </summary>
        public NavPoint()
        {
        }

        internal NavPoint(XElement navPointXElement)
        {
            // PlayOrder
            if (navPointXElement.Attribute("playOrder") != null)
                PlayOrder = Convert.ToInt32(navPointXElement.Attribute("playOrder").Value, CultureInfo.InvariantCulture);

            // NavLabel
            NavLabel = new NavLabel(navPointXElement.Element(Toc.DaisyOrgNcxNameSpace + "navLabel"));

            // Content
            Content = new Content(navPointXElement.Element(Toc.DaisyOrgNcxNameSpace + "content"));

            // NavPoints
            IEnumerable<XElement> navPointChildsXElement = navPointXElement.Elements(Toc.DaisyOrgNcxNameSpace + "navPoint");
            if (navPointChildsXElement.Count() > 0)
            {
                NavPoints = new Collection<NavPoint>();
                foreach (XElement navPointChildXElement in navPointChildsXElement)
                {
                    NavPoints.Add(new NavPoint(navPointChildXElement));
                }
            }
        }        
    }
}
