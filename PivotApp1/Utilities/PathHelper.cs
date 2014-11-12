// --------------------------------
// <copyright file="PathHelper.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace EPubReader.Utilities
{
    internal static class PathHelper
    {
        internal static string Decode(string name)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidPathChars()));
            string invalidReStr = String.Format(CultureInfo.InvariantCulture, @"[{0}]+", invalidChars);
            return Regex.Replace(name, invalidReStr, "_");
        }
    }
}
