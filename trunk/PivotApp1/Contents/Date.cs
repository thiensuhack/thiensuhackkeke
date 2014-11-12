// --------------------------------
// <copyright file="Date.cs" company="Cubicsoft (www.cubicsoft.pl)">
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
    /// Date of publication.
    /// </summary>
    public class Date : BasePurl
    {        
        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        public string Event
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of date class.
        /// </summary>
        public Date()
        {
        }

        internal Date(XElement metaXElement)
        {
            Value = metaXElement.Value;
            if (metaXElement.Attribute(Loader.IdpfOrg2007OpfNameSpace + "event") != null)
            {
                Event = metaXElement.Attribute(Loader.IdpfOrg2007OpfNameSpace + "event").Value;
            }
        }
    }
}
