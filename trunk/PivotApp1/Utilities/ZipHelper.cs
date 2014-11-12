// --------------------------------
// <copyright file="ZipHelper.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using ICSharpCode.SharpZipLib.Zip;

namespace EPubReader.Utilities
{
    /// <summary>
    /// Zip library helper.
    /// </summary>
    internal static class ZipHelper
    {
        /// <summary>
        /// Extracts the specified stream.
        /// </summary>
        /// <param name="stream">Stream containings EPub file.</param>
        /// <returns>List of extracted files.</returns>
        internal static Collection<string> Extract(Stream stream)
        {
            Collection<string> files = new Collection<string>();

            // purge storage directory
            IsolatedStorageHelper.ClearStorageZipDirectory();

            // enumarate entries
            using (ZipFile file = new ZipFile(stream))
            {
                foreach (ZipEntry entry in file)
                {
                    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        // create directory
                        isf.CreateDirectory(Path.Combine(Globals.StorageZipDirectory, Path.GetDirectoryName(entry.Name)));

                        // save entry stream
                        if (entry.IsFile)
                        {
                            using (Stream entryStrem = file.GetInputStream(entry))
                            {
                                using (IsolatedStorageFileStream isfs = isf.CreateFile(Path.Combine(Globals.StorageZipDirectory, entry.Name)))
                                {
                                    entryStrem.CopyTo(isfs);

                                    isfs.Close();
                                }

                                entryStrem.Close();
                            }

                            files.Add(entry.Name);
                        }
                    }
                }
                file.Close();
            }

            return files;
        }
    }
}
