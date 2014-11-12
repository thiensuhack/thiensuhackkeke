// --------------------------------
// <copyright file="Text.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Windows;
using System.Windows.Media;

namespace EPubReader.Elements
{
    /// <summary>
    /// Text element.
    /// </summary>
    public class Text : BaseElement
    {
        /// <summary>
        /// Gets or sets the font size multiplier.
        /// </summary>
        public double FontSizeMultiplier
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        /// <remarks>Sterling is not able to store type FontStyle because of missing default contructor in FontStyle.</remarks>
        public TextFontStyle FontStyle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        /// <remarks>Sterling is not able to store type FontWeight because of missing default contructor in FontWeight.</remarks>
        public TextFontWeight FontWeight
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the foreground
        /// </summary>
        public Brush Foreground
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the decoration.
        /// </summary>
        public TextDecoration Decoration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        public string Href
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of Text class.
        /// </summary>
        public Text()
            : base()
        {          
        }
    }
}
