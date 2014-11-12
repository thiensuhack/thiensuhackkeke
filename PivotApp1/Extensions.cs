// --------------------------------
// <copyright file="Extensions.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;

namespace EPubReader
{
    internal static class Extensions
    {
        internal static void PurgeDirectory(this IsolatedStorageFile isf, string directory)
        {
#if DEBUG
            PurgeDirectoryNotSafe(isf, directory);
#else
            try
            {
                PurgeDirectory(isf, directory);
            }
            catch (Exception ex)
            {
                throw new EPubReaderException("Exception occured while directory purge" + directory, ex);
            }
#endif
        }

        private static void PurgeDirectoryNotSafe(IsolatedStorageFile isf, string directory)
        {
            if (!isf.DirectoryExists(directory))
            {
                return;
            }

            // delete files
            foreach (string file in isf.GetFileNames(directory + "\\*"))
            {
                isf.DeleteFile(Path.Combine(directory, file));
            }

            // delete sub directories
            foreach (string subDirectory in isf.GetDirectoryNames(directory + "\\*"))
            {
                isf.PurgeDirectory(Path.Combine(directory, subDirectory));
            }

            // delete directory
            isf.DeleteDirectory(directory);
        }

        internal static string ReadString(this IsolatedStorageFile isf, string path)
        {
            string s;
#if DEBUG
            s = ReadStringNotSafe(isf, path);
#else
            try
            {
                s = ReadStringNotSafe(isf, path);
            }
            catch (Exception ex)
            {
                throw new EPubReaderException("Exception occured while reading string from file " + path, ex);
            }
#endif
            return s;
        }

        private static string ReadStringNotSafe(IsolatedStorageFile isf, string path)
        {
            string s;
            using (IsolatedStorageFileStream isfs = isf.OpenFile(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(isfs))
                {
                    s = sr.ReadToEnd();
                    sr.Close();
                }

                isfs.Close();
            }
            return s;
        }

        internal static Stream ReadStream(this IsolatedStorageFile isf, string path)
        {
            MemoryStream ms;
#if DEBUG
            ms = ReadStreamNotSafe(isf, path);
#else
            try
            {
                ms = ReadStreamNotSafe(isf, path);
            }
            catch (Exception ex)
            {
                throw new EPubReaderException("Exception occured while reading string from file " + path, ex);
            }
#endif

            return ms;
        }

        private static MemoryStream ReadStreamNotSafe(IsolatedStorageFile isf, string path)
        {
            MemoryStream ms;
            using (IsolatedStorageFileStream isfs = isf.OpenFile(path, FileMode.Open, FileAccess.Read))
            {
                byte[] data = new byte[isfs.Length];
                isfs.Read(data, 0, data.Length);

                ms = new MemoryStream(data);

                isfs.Close();
            }
            return ms;
        }

        internal static void ForEach<T>(this ICollection<T> list, Action<T> action)
        {
            foreach (T item in list)
            {
                action.Invoke(item);
            }
        }

        internal static Elements.TextFontStyle ToTextFontStyle(this FontStyle fs)
        {
            if (fs == FontStyles.Italic)
            {
                return Elements.TextFontStyle.Italic;
            }
            else
            {
                return Elements.TextFontStyle.Normal;
            }
        }

        internal static FontStyle ToFontStyle(this Elements.TextFontStyle tfs)
        {
            if (tfs == Elements.TextFontStyle.Italic)
            {
                return FontStyles.Italic;
            }
            else
            {
                return FontStyles.Normal;
            }
        }

        internal static Elements.TextFontWeight ToTextFontWeight(this FontWeight fw)
        {
            if (fw == FontWeights.Black)
            {
                return Elements.TextFontWeight.Black;
            }
            else if (fw == FontWeights.Bold)
            {
                return Elements.TextFontWeight.Bold;
            }
            else if (fw == FontWeights.ExtraBlack)
            {
                return Elements.TextFontWeight.ExtraBlack;
            }
            else if (fw == FontWeights.ExtraBold)
            {
                return Elements.TextFontWeight.ExtraBold;
            }
            else if (fw == FontWeights.ExtraLight)
            {
                return Elements.TextFontWeight.ExtraLight;
            }
            else if (fw == FontWeights.Light)
            {
                return Elements.TextFontWeight.Light;
            }
            else if (fw == FontWeights.Medium)
            {
                return Elements.TextFontWeight.Medium;
            }
            else if (fw == FontWeights.SemiBold)
            {
                return Elements.TextFontWeight.SemiBold;
            }
            else if (fw == FontWeights.Thin)
            {
                return Elements.TextFontWeight.Thin;
            }
            else
            {
                return Elements.TextFontWeight.Normal;
            }
        }

        internal static FontWeight ToFontWeight(this Elements.TextFontWeight tfw)
        {
            if (tfw == Elements.TextFontWeight.Black)
            {
                return FontWeights.Black;
            }
            else if (tfw == Elements.TextFontWeight.Bold)
            {
                return FontWeights.Bold;
            }
            else if (tfw == Elements.TextFontWeight.ExtraBlack)
            {
                return FontWeights.ExtraBlack;
            }
            else if (tfw == Elements.TextFontWeight.ExtraBold)
            {
                return FontWeights.ExtraBold;
            }
            else if (tfw == Elements.TextFontWeight.ExtraLight)
            {
                return FontWeights.ExtraLight;
            }
            else if (tfw == Elements.TextFontWeight.Light)
            {
                return FontWeights.Light;
            }
            else if (tfw == Elements.TextFontWeight.Medium)
            {
                return FontWeights.Medium;
            }
            else if (tfw == Elements.TextFontWeight.SemiBold)
            {
                return FontWeights.SemiBold;
            }
            else if (tfw == Elements.TextFontWeight.Thin)
            {
                return FontWeights.Thin;
            }
            else
            {
                return FontWeights.Normal;
            }
        }

        internal static Elements.TextDecoration ToTextDecoration(this TextDecorationCollection tdc)
        {
            if (tdc != null && tdc == TextDecorations.Underline)
            {
                return Elements.TextDecoration.Underline;
            }
            else
            {
                return Elements.TextDecoration.Normal;
            }
        }

        internal static TextDecorationCollection ToTextDecorationCollection(this Elements.TextDecoration td)
        {
            if (td == Elements.TextDecoration.Underline)
            {
                return TextDecorations.Underline;
            }
            else
            {
                return null;
            }
        }
    }
}
