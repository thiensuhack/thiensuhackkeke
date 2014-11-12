// --------------------------------
// <copyright file="Book.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.ObjectModel;
using System.Linq;
using EPubReader.Utilities;

namespace EPubReader
{
    /// <summary>
    /// EPub book.
    /// </summary>
    public class Book
    {
        private const string OpsDirectory = "OPS";
        private const string OebpsDirectory = "OEBPS";
        private const string EpubDirectory = "EPUB";

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        public Contents.Metadata Metadata
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the manifest.
        /// </summary>
        public Contents.Manifest Manifest
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the spine.
        /// </summary>
        public Contents.Spine Spine
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the toc.
        /// </summary>
        public Tocs.Toc Toc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the files.
        /// </summary>
        public Collection<string> Files
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the item elements containers.
        /// </summary>
        public Collection<ItemElementsContainer> ItemElementsContainers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the assembly version on which book was readed (for cache).
        /// </summary>
        public string ReaderVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the book is parsed.
        /// </summary>
        public bool IsParsed
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes new instance of Book class.
        /// </summary>
        public Book()
        {
        }

        internal string GetFilePath(string file)
        {
            return Files.Where(f => 
                f.Equals(file, StringComparison.OrdinalIgnoreCase)
                ||
                f.Equals(OpsDirectory + "/" + file, StringComparison.OrdinalIgnoreCase)
                || 
                f.Equals(OebpsDirectory + "/" + file, StringComparison.OrdinalIgnoreCase) 
                || 
                f.Equals(EpubDirectory + "/" + file, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
        }

        internal string GetImagesDirectory()
        {
            return Globals.StorageImagesDirectory + "\\" + PathHelper.Decode(Metadata.Identifier.Value);
        }
    }
}
