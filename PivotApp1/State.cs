// --------------------------------
// <copyright file="State.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

namespace EPubReader
{
    /// <summary>
    /// State of the EPubViewer.
    /// </summary>
    public enum State
    {
        /// <summary>
        /// Normal state.
        /// </summary>
        Normal,

        /// <summary>
        /// Cover state.
        /// </summary>
        Cover,

        /// <summary>
        /// Table of contents.
        /// </summary>
        Toc
    }
}
