// --------------------------------
// <copyright file="BasePurl.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace EPubReader.Contents
{
    /// <summary>
    /// Base Purl element.
    /// </summary>
    public abstract class BasePurl
    {
        internal const string PurlOrgDcElementsNameSpace = "{http://purl.org/dc/elements/1.1/}";

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of BasePurl class.
        /// </summary>
        protected BasePurl()
        {
        }

        internal BasePurl(string elementName, IEnumerable<XElement> metadataXElements)
        {
            XElement xe = metadataXElements.Elements(PurlOrgDcElementsNameSpace + elementName).SingleOrDefault();
            if (xe != null)
            {
                Value = xe.Value;
            }
        }
    }
}
