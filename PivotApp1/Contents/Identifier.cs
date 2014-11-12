// --------------------------------
// <copyright file="Identifier.cs" company="Cubicsoft (www.cubicsoft.pl)">
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
    /// A identifier of the resource.
    /// </summary>
    public class Identifier : BasePurl
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        public string Scheme
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of Identifier class.
        /// </summary>
        public Identifier()
        {
        }

        internal Identifier(IEnumerable<XElement> metadataXElements)
        {
            IEnumerable<XElement> xElements = metadataXElements.Elements(PurlOrgDcElementsNameSpace + "identifier");
            if (xElements != null && xElements.Count() > 0)
            {
                XElement xe = null;

                if (xElements.Count() > 1)
                {
                    // if we got more than one, we search for element with attribute "id"
                    xe = xElements.Where(x => x.Attribute("id") != null).FirstOrDefault();                    
                }

                // if null - take first
                if (xe == null)
                {
                    xe = xElements.First();
                }

                Value = xe.Value;

                if (xe.Attribute("id") != null)
                {
                    Id = xe.Attribute("id").Value;
                }

                if (xe.Attribute(Loader.IdpfOrg2007OpfNameSpace + "scheme") != null)
                {
                    Scheme = xe.Attribute(Loader.IdpfOrg2007OpfNameSpace + "scheme").Value;
                }
            }
        }
    }
}
