// --------------------------------
// <copyright file="IsolatedStorageHelper.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;

namespace EPubReader.Utilities
{
    internal static class IsolatedStorageHelper
    {
        internal static string ReadZipString(string path)
        {
            string content;
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                content = isf.ReadString(System.IO.Path.Combine(Globals.StorageZipDirectory, path));
            }
            return content;
        }

        internal static Stream ReadZipStream(string path)
        {
            Stream stream;
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                stream = isf.ReadStream(System.IO.Path.Combine(Globals.StorageZipDirectory, path));
            }
            return stream;
        }

        internal static void ClearStorageZipDirectory()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                isf.PurgeDirectory(Globals.StorageZipDirectory);
            }
        }
    }
}
