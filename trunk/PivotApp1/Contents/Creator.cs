// --------------------------------
// <copyright file="Creator.cs" company="Cubicsoft (www.cubicsoft.pl)">
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
    /// A primary creator or author of the publication.
    /// </summary>
    public class Creator : BasePurl
    {
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public string Role
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of Creator class.
        /// </summary>
        public Creator()
        {
        }

        internal Creator(XElement metaXElement)            
        {
            Value = metaXElement.Value;
            if (metaXElement.Attribute(Loader.IdpfOrg2007OpfNameSpace + "role") != null)
            {
                Role = metaXElement.Attribute(Loader.IdpfOrg2007OpfNameSpace + "role").Value;
            }
        }
    }
}
