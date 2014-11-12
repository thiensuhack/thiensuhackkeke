// --------------------------------
// <copyright file="Language.cs" company="Cubicsoft (www.cubicsoft.pl)">
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
    /// Identifies a language of the intellectual content of the Publication.
    /// </summary>
    public class Language : BasePurl
    {
        /// <summary>
        /// Initializes new instance of Language class.
        /// </summary>
        public Language()
        {
        }

        internal Language(XElement metaXElement)            
        {
            Value = metaXElement.Value;
        }
    }
}
