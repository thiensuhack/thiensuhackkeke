// --------------------------------
// <copyright file="NavLabel.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Xml.Linq;

namespace EPubReader.Tocs
{
    /// <summary>
    /// Nav label.
    /// </summary>
    public class NavLabel
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public Text Text
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of NaLabel class.
        /// </summary>
        public NavLabel()
        {
        }

        internal NavLabel(XElement navLabelXElement)
        {
            // Text
            Text = new Text(navLabelXElement.Element(Toc.DaisyOrgNcxNameSpace + "text"));
        }
    }
}
