// --------------------------------
// <copyright file="Globals.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

namespace EPubReader
{
    /// <summary>
    /// Global settings.
    /// </summary>
    internal static class Globals
    {
        /// <summary>
        /// Storage directory of extracted files.
        /// </summary>
        internal const string StorageZipDirectory = "EPubReader\\Zip";

        /// <summary>
        /// Storage directory of book images.
        /// </summary>
        internal const string StorageImagesDirectory = "EPubReader\\Images";

        /// <summary>
        /// Reader version.
        /// </summary>
        /// <remarks>
        /// This field should change only if something major was changed in Loader / Parser / Renderer.
        /// </remarks>
        internal const string Version = "0.2.0.0";

        /// <summary>
        /// Consts decoding.
        /// </summary>
        internal const string CarriageReturnNewLine = "\r\n";
        internal const string CarriageReturn = "\r";
        internal const string NewLine = "\n";
        internal const string TabEscape = "\t";
    }
}
