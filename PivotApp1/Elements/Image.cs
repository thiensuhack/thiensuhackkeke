// --------------------------------
// <copyright file="Image.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

namespace EPubReader.Elements
{
    /// <summary>
    /// Image element.
    /// </summary>
    public class Image : BaseElement
    {
        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int Height
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of Image class.
        /// </summary>
        public Image()
            : base()
        {
        }
    }
}
