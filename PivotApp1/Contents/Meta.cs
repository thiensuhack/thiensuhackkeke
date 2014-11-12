// --------------------------------
// <copyright file="Meta.cs" company="Cubicsoft (www.cubicsoft.pl)">
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
    /// Meta of the publication's content.
    /// </summary>
    public class Meta : BasePurl
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        public string Property
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of Meta class.
        /// </summary>
        public Meta()
        {
        }

        internal Meta(XElement metaXElement)
        {
            if (metaXElement.Attribute("name") != null)
            {
                Name = metaXElement.Attribute("name").Value;
            }

            if (metaXElement.Attribute("content") != null)
            {
                Content = metaXElement.Attribute("content").Value;
            }

            if (metaXElement.Attribute("property") != null)
            {
                Property = metaXElement.Attribute("property").Value;
            }

            Value = metaXElement.Value;
        }
    }
}
