// --------------------------------
// <copyright file="Loader.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using EPubReader.Utilities;

namespace EPubReader
{
    internal sealed class Loader
    {
        private const string MimetypeFile = "mimetype";
        private const string ContainerXmlFile = "container.xml";
        private const string MetaInfDirectory = "meta-inf";

        internal const string IdpfOrg2007OpfNameSpace = "{http://www.idpf.org/2007/opf}";

        private const string ContainerXmlNameSpace = "{urn:oasis:names:tc:opendocument:xmlns:container}";

        // decode        
        private const string DecodeReplace = "";

        private EPubViewer _ePubViewer;

        private Loader()
        {
        }

        internal Loader(EPubViewer ePubViewer)
        {
            _ePubViewer = ePubViewer;
        }

        internal void Load()
        {
#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif

            ReadMimetype();

            // read container xml
            string contentOpfFile = ReadContainerXml();

            // read content opf
            string contentOpfcontent = ReadContentOpf(contentOpfFile);

            // parse metadata
            IEnumerable<XElement> parsedContentOpfcontentPackage = ParseMetadata(contentOpfcontent);
            ParseManifest(parsedContentOpfcontentPackage);
            ParseSpine(parsedContentOpfcontentPackage);
            ParseToc();

#if DEBUG
            sw.Stop();
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "Information: EPubReader.Loader.Load - Executed in {0} ms", sw.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture)));
#endif            
        }

        private void ReadMimetype()
        {
            string path = FileExists(MimetypeFile);
            string content = IsolatedStorageHelper.ReadZipString(path);

            if (String.IsNullOrEmpty(content))
            {
                throw new EPubReaderException(String.Format(CultureInfo.InvariantCulture, "File \"{0}\" does not contain mime type", path));
            }

            // decode
            content = content.Replace(Globals.CarriageReturn, DecodeReplace);
            content = content.Replace(Globals.NewLine, DecodeReplace);
            content = content.Replace(Globals.TabEscape, DecodeReplace);
            content = content.Trim();

            if (content != "application/epub+zip")
            {
                throw new EPubReaderException(String.Format(CultureInfo.InvariantCulture, "Format \"{0}\" is not supported", content));
            }
        }

        private string ReadContainerXml()
        {
            string path = _ePubViewer.Book.GetFilePath(ContainerXmlFile);
            if (path == null)
            {
                path = FileExists(MetaInfDirectory + "/" + ContainerXmlFile);
            }

            string content = IsolatedStorageHelper.ReadZipString(path);

            if (String.IsNullOrEmpty(content))
            {
                throw new EPubReaderException(String.Format(CultureInfo.InvariantCulture, "File \"{0}\" does not contain any content", path));
            }

            XElement xe = XDocument.Parse(content).
                Elements(ContainerXmlNameSpace + "container").
                Elements(ContainerXmlNameSpace + "rootfiles").
                Elements(ContainerXmlNameSpace + "rootfile").
                First();

            // read type
            string mediaType = xe.Attribute("media-type").Value.Trim();

            if (mediaType != "application/oebps-package+xml")
            {
                throw new EPubReaderException(String.Format(CultureInfo.InvariantCulture, "Container media-type \"{0}\" is not supported", mediaType));
            }

            // read content path
            return xe.Attribute("full-path").Value.Trim();
        }

        private string ReadContentOpf(string file)
        {
            string path = FileExists(file);
            string content = IsolatedStorageHelper.ReadZipString(path);

            if (String.IsNullOrEmpty(content))
            {
                throw new EPubReaderException(String.Format(CultureInfo.InvariantCulture, "File \"{0}\" does not contain any content", path));
            }

            return content;
        }

        private IEnumerable<XElement> ParseMetadata(string contentOpfcontent)
        {
            IEnumerable<XElement> parsedContentOpfcontentPackage = XDocument.Parse(contentOpfcontent).Elements(IdpfOrg2007OpfNameSpace + "package");

            IEnumerable<XElement> xElements = parsedContentOpfcontentPackage.
                Elements(IdpfOrg2007OpfNameSpace + "metadata");

            _ePubViewer.Book.Metadata = new Contents.Metadata(xElements);

            return parsedContentOpfcontentPackage;
        }

        private void ParseManifest(IEnumerable<XElement> parsedContentOpfcontentPackage)
        {
            IEnumerable<XElement> xElements = parsedContentOpfcontentPackage.
                Elements(IdpfOrg2007OpfNameSpace + "manifest");

            _ePubViewer.Book.Manifest = new Contents.Manifest(xElements);
        }

        private void ParseSpine(IEnumerable<XElement> parsedContentOpfcontentPackage)
        {
            IEnumerable<XElement> xElements = parsedContentOpfcontentPackage.
                Elements(IdpfOrg2007OpfNameSpace + "spine");

            _ePubViewer.Book.Spine = new Contents.Spine(xElements);
        }

        private void ParseToc()
        {
            Contents.Item item = _ePubViewer.Book.Manifest.Items.Where(i => (i.Id == "ncx" || i.Id == "ncxtoc") && i.MediaType == "application/x-dtbncx+xml").OrderBy(i => i.Id).FirstOrDefault();
            if (item == null)
            {
                return; // book doesn't contains toc manifest item
            }

            string path = _ePubViewer.Book.GetFilePath(item.Href);
            if (path == null)
            {
                return; // book doesn't contains toc file
            }

            string content = IsolatedStorageHelper.ReadZipString(path);

            // WP does not support DOCTYPE
            string contentCleaned = Regex.Replace(content, "<!DOCTYPE[^>]*>", String.Empty);

            _ePubViewer.Book.Toc = new Tocs.Toc(XDocument.Parse(contentCleaned).Elements(Tocs.Toc.DaisyOrgNcxNameSpace + "ncx"));
        }

        internal string FileExists(string file)
        {
            string path = _ePubViewer.Book.GetFilePath(file);
            if (path == null)
            {
                throw new EPubReaderException(String.Format(CultureInfo.InvariantCulture, "EPub does not contain \"{0}\" file", file));
            }
            return path;
        }
    }
}
