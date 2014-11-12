// --------------------------------
// <copyright file="Subject.cs" company="Cubicsoft (www.cubicsoft.pl)">
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
    /// The subject of the publication.
    /// </summary>
    public class Subject : BasePurl
    {
        /// <summary>
        /// Initializes new instance of Subject class.
        /// </summary>
        public Subject()
        {
        }

        internal Subject(XElement metaXElement)            
        {
            Value = metaXElement.Value;
        }
    }
}
