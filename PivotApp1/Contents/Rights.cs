// --------------------------------
// <copyright file="Rights.cs" company="Cubicsoft (www.cubicsoft.pl)">
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
    /// A statement about rights, or a reference to one.
    /// </summary>
    public class Rights : BasePurl
    {
        /// <summary>
        /// Initializes new instance of Rights class.
        /// </summary>
        public Rights()
        {
        }

        internal Rights(XElement metaXElement)
        {
            Value = metaXElement.Value;
        }
    }
}
