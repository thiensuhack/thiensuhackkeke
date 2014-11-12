// --------------------------------
// <copyright file="Parser.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EPubReader.Utilities;
using HtmlAgilityPack;

namespace EPubReader
{
    internal class Parser
    {
        private const string XhtmlMediaType = "application/xhtml+xml";
        private const string HtmlBodyPath = "/html/body";
        
        // decode        
        private const string DecodeReplace = " ";

        private static readonly Brush AnchorBrush = new SolidColorBrush(Color.FromArgb(255, 68, 132, 220));

        private EPubViewer _ePubViewer;
        
        private Parser()
        {
        }

        internal Parser(EPubViewer ePubViewer)
        {
            _ePubViewer = ePubViewer;
        }

        internal void Parse()
        {
            if (!_ePubViewer.Book.IsParsed)
            {
#if DEBUG
                Stopwatch sw = new Stopwatch();
                sw.Start();
#endif
                _ePubViewer.Book.ItemElementsContainers = new Collection<ItemElementsContainer>();

                // enumerate itemRef
                int startLocation = 0;
                foreach (Contents.ItemRef itemRef in _ePubViewer.Book.Spine.ItemRefs)
                {
                    startLocation += ParseItemRef(itemRef, ref startLocation);
                }
#if DEBUG
                sw.Stop();
                Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "Information: EPubReader.Parser.Parse - Executed in {0} ms", sw.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture)));
#endif

                _ePubViewer.Book.IsParsed = true;
                _ePubViewer.CacheBook();
            }
        }

        private int ParseItemRef(Contents.ItemRef itemRef, ref int startLocation)
        {
            // find corresponding item
            Contents.Item item = _ePubViewer.Book.Manifest.Items.Where(i => i.Id == itemRef.Idref).SingleOrDefault();
            if (item == null || item.MediaType != XhtmlMediaType) // check mediatype
            {
                Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "Warning: EPubReader.Parseer.ParseItemRef - Manifest does not contains ItemRef with Id \"{0}\"", itemRef.Idref));
                return 0;
            }

            // read file
            string path = _ePubViewer.Book.GetFilePath(HttpUtility.UrlDecode(item.Href));
            if (path == null)
            {
                Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "Warning: EPubReader.Parseer.ParseItemRef - EPub does not contain page file \"{0}\"", item.Href));
                return 0;
            }
            string content = IsolatedStorageHelper.ReadZipString(path);

            // read html
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);

            List<Elements.BaseElement> elements = new List<Elements.BaseElement>();

            ParseNodeChilds(htmlDoc.DocumentNode.SelectSingleNode(HtmlBodyPath), ref elements);

            if (elements.Count > 0)
            {
                _ePubViewer.Book.ItemElementsContainers.Add(
                    new ItemElementsContainer() 
                    { 
                        Item = item, 
                        Elements = new Collection<Elements.BaseElement>(elements), 
                        StartLocation = startLocation 
                    }
                ); // push ItemElementsContainers
            }

            return elements.Count;
        }

        private void ParseNodeChilds(HtmlNode htmlNodeParent, ref List<Elements.BaseElement> elements)
        {
            foreach (HtmlNode htmlNode in htmlNodeParent.ChildNodes)
            {
                if (htmlNode.Name == "#text" && htmlNodeParent.Name != "body") // we don't render text inside body
                {
                    ParseText(htmlNode, ref elements, 1, FontStyles.Normal, FontWeights.Normal, null, null);
                }
                else if (htmlNode.Name == "i" || htmlNode.Name == "em")
                {
                    ParseText(htmlNode, ref elements, 1, FontStyles.Italic, FontWeights.Normal, null, null);
                }
                else if (htmlNode.Name == "b" || htmlNode.Name == "strong")
                {
                    ParseText(htmlNode, ref elements, 1, FontStyles.Normal, FontWeights.Bold, null, null);
                }
                else if (htmlNode.Name == "h1" || htmlNode.Name == "h2" || htmlNode.Name == "h3" || htmlNode.Name == "h4" || htmlNode.Name == "h5" || htmlNode.Name == "h6")
                {
                    ParseHeading(htmlNode, ref elements);
                }
                else if (htmlNode.Name == "br")
                {
                    AddBreak(ref elements);
                }
                else if (htmlNode.Name == "img")
                {
                    ParseImg(htmlNode, ref elements);
                }
                else if (htmlNode.Name == "a")
                {
                    ParseText(htmlNode, ref elements, 1, FontStyles.Normal, FontWeights.Normal, TextDecorations.Underline, AnchorBrush);
                }
                else if (htmlNode.Name == "span" || htmlNode.Name == "center")
                {
                    ParseNodeChilds(htmlNode, ref elements);
                }
                else if (htmlNode.Name == "div" || htmlNode.Name == "p" || htmlNode.Name == "section")
                {
                    ParseContainer(htmlNode, ref elements);
                }
                else if (htmlNode.Name == "ul")
                {
                    ParseOrderedList(htmlNode, ref elements);
                }
                else if (htmlNode.Name == "ol")
                {
                    ParseUnorderedList(htmlNode, ref elements);
                }
                else if (htmlNode.Name == "blockquote" || htmlNode.Name == "q")
                {
                    ParseBlockquote(htmlNode, ref elements);
                }
                else if (htmlNode.Name == "hr")
                {
                    ParseHorizontalLine(htmlNode, ref elements);
                }
                else if (htmlNode.Name == "pre" || htmlNode.Name == "code")
                {
                    ParsePreformattedText(htmlNode, ref elements);
                }
                else if (htmlNode.Name == "dl")
                {
                    ParseDefnitionList(htmlNode, ref elements);
                }
                else if (htmlNode.Name == "table")
                {
                    ParseTable(htmlNode, ref elements);
                }
                else if (htmlNode.Name != "#text")
                {
                    Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "Warning: EPubReader.Parseer.ParseNodeChilds - Not recognizable html node \"{0}\"", htmlNode.Name));
                }
            }
        }

        private void ParseContainer(HtmlNode htmlNode, ref List<Elements.BaseElement> elements)
        {
            AddBreak(ref elements);
            ParseNodeChilds(htmlNode, ref elements);
            AddBreak(ref elements);
        }

        private void ParseOrderedList(HtmlNode htmlNode, ref List<Elements.BaseElement> elements)
        {
            AddBreak(ref elements);

            int idx = 1;
            foreach (HtmlNode hn in htmlNode.ChildNodes.Where(n => n.Name == "li"))
            {
                // number
                HtmlNode hnDot = hn.Clone();
                hnDot.InnerHtml = "    " + (idx++) + " ";
                ParseText(hnDot, ref elements, 1, FontStyles.Normal, FontWeights.Normal, null, null);

                // childs (content)
                ParseNodeChilds(hn, ref elements);

                AddBreak(ref elements);
            }
        }

        private void ParseUnorderedList(HtmlNode htmlNode, ref List<Elements.BaseElement> elements)
        {
            AddBreak(ref elements);

            foreach (HtmlNode hn in htmlNode.ChildNodes.Where(n => n.Name == "li"))
            {
                // dot
                HtmlNode hnDot = hn.Clone();
                hnDot.InnerHtml = "    • ";
                ParseText(hnDot, ref elements, 1, FontStyles.Normal, FontWeights.Normal, null, null);

                // childs (content)
                ParseNodeChilds(hn, ref elements);

                AddBreak(ref elements);
            }
        }

        private static void ParseBlockquote(HtmlNode htmlNode, ref List<Elements.BaseElement> elements)
        {
            AddBreak(ref elements);
            ParseText(htmlNode, ref elements, 1, FontStyles.Italic, FontWeights.Normal, null, null);
            AddBreak(ref elements);
        }

        private static void ParseText(HtmlNode htmlNode, ref List<Elements.BaseElement> elements, double fontSizeMultiplier, FontStyle fontStyle, FontWeight fontWeight, TextDecorationCollection textDecorations, Brush foreground)
        {
            string decodedText = DecodeText(htmlNode.InnerText);

            decodedText = decodedText.Trim();

            if (String.IsNullOrEmpty(decodedText) && elements.Count > 0 && elements[elements.Count - 1] is Elements.Break) // we don't add empty space after break
            {
                return;
            }

            // create text
            Elements.BaseElement element = new Elements.Text() 
            { 
                FontSizeMultiplier = fontSizeMultiplier, 
                FontStyle = fontStyle.ToTextFontStyle(),
                FontWeight = fontWeight.ToTextFontWeight(), 
                Decoration = textDecorations.ToTextDecoration(), 
                Foreground = foreground,
                Value = decodedText
            };
            
            // extract id
            ExtractId(htmlNode, ref element);

            // extract anchor
            if (htmlNode.Name == "a")
            {
                ExtractHref(htmlNode, ref element);
            }

            // add
            elements.Add(element);
        }

        private static string DecodeText(string text)
        {
            string decodedText = HttpUtility.HtmlDecode(text);

            // remove CarriageReturn, NewLine and TabEscape
            decodedText = decodedText.Replace(Globals.CarriageReturnNewLine, DecodeReplace);
            decodedText = decodedText.Replace(Globals.CarriageReturn, DecodeReplace);
            decodedText = decodedText.Replace(Globals.NewLine, DecodeReplace);
            decodedText = decodedText.Replace(Globals.TabEscape, DecodeReplace);

            return decodedText;
        }
        
        private static void ParseHeading(HtmlNode htmlNode, ref List<Elements.BaseElement> elements)
        {
            string decodedText = DecodeText(htmlNode.InnerText);

            if (String.IsNullOrWhiteSpace(decodedText)) // we don't render empty spaces
            {
                return;
            }

            AddBreak(ref elements);
            ParseText(htmlNode, ref elements, 1.5, FontStyles.Normal, FontWeights.Normal, null, null);
            AddBreak(ref elements);
        }

        private static void AddBreak(ref List<Elements.BaseElement> elements)
        {
            if (elements.Count > 0 && elements[elements.Count - 1] is Elements.Break) // we don't render more then one break
            {
                return;
            }

            if (elements.Count > 0 && (elements[elements.Count - 1] is Elements.Image || elements[elements.Count - 1] is Elements.Line)) // we don't add break after image or line
            {
                return;
            }

            // add break
            elements.Add(new Elements.Break());
        }

        private void ParseImg(HtmlNode htmlNode, ref List<Elements.BaseElement> elements)
        {
            string path = _ePubViewer.Book.GetFilePath(HttpUtility.UrlDecode(htmlNode.Attributes["src"].Value.Replace("../", "")));
            if (path == null) // img src file does not exists (don't render)
            {
                Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "Warning: EPubReader.Parseer.ParseImg - Image \"{0}\" does not exists.", HttpUtility.UrlDecode(htmlNode.Attributes["src"].Value.Replace("../", ""))));
                return;
            }

            Stream s = IsolatedStorageHelper.ReadZipStream(path);

            string fileName = Guid.NewGuid().ToString();
            Size size = Size.Empty;

            // detect image type
            if (
                path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                ||
                path.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)
                ||
                path.EndsWith(".jpe", StringComparison.OrdinalIgnoreCase)
                ||
                path.EndsWith(".jif", StringComparison.OrdinalIgnoreCase)
                ||
                path.EndsWith(".jfif", StringComparison.OrdinalIgnoreCase)
                ||
                path.EndsWith(".jfi", StringComparison.OrdinalIgnoreCase)
                )
            {
                BitmapSource bs = new BitmapImage();
                bs.SetSource(s);

                size = new Size(bs.PixelWidth, bs.PixelHeight);

                // save image
                fileName += ".jpg";
                string isfPath = Path.Combine(_ePubViewer.Book.GetImagesDirectory(), fileName);
                try
                {
                    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (!isf.DirectoryExists(Path.GetDirectoryName(isfPath)))
                        {
                            isf.CreateDirectory(Path.GetDirectoryName(isfPath));
                        }

                        using (IsolatedStorageFileStream isfs = isf.CreateFile(isfPath))
                        {
                            WriteableBitmap wb = new WriteableBitmap(bs);
                            wb.SaveJpeg(isfs, bs.PixelWidth, bs.PixelHeight, 0, 100);
                            isfs.Close();
                        }
                    }
                }
                catch
                {
                    Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "Warning: EPubReader.Parseer.ParseImg - Error saving image \"{0}\".", isfPath));
                    return;
                }
            }
            else if (
                path.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase)
                ||
                path.EndsWith(".dib", StringComparison.OrdinalIgnoreCase)
                ||
                path.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
                ||
                path.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                )
            {
                ImageTools.ExtendedImage extendedImage = new ImageTools.ExtendedImage();
                ImageTools.IO.IImageDecoder decoder;

                if (
                    path.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase)
                    ||
                    path.EndsWith(".dib", StringComparison.OrdinalIgnoreCase)
                    )
                {
                    decoder = new ImageTools.IO.Bmp.BmpDecoder();
                }
                else if (path.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    decoder = new ImageTools.IO.Gif.GifDecoder();
                }
                else // if (path.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    decoder = new ImageTools.IO.Png.PngDecoder();
                }

                decoder.Decode(extendedImage, s);

                size = new Size(extendedImage.PixelWidth, extendedImage.PixelHeight);

                ImageTools.IO.Png.PngEncoder pngEncoder = new ImageTools.IO.Png.PngEncoder();

                // save image
                fileName += ".png";
                string isfPath = Path.Combine(_ePubViewer.Book.GetImagesDirectory(), fileName);
                try
                {
                    using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (!isf.DirectoryExists(Path.GetDirectoryName(isfPath)))
                        {
                            isf.CreateDirectory(Path.GetDirectoryName(isfPath));
                        }

                        using (IsolatedStorageFileStream isfs = isf.CreateFile(isfPath))
                        {
                            pngEncoder.Encode(extendedImage, isfs);
                            isfs.Close();
                        }
                    }
                }
                catch
                {
                    Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "Warning: EPubReader.Parseer.ParseImg - Error saving image \"{0}\".", isfPath));
                    return;
                }                
            }
            else
            {
                s.Close();
                s.Dispose();

                Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "Warning: EPubReader.Parseer.ParseImg - Not recognizable image type \"{0}\".", path));
                return;
            }
            
            RemoveLastBreak(ref elements);

            // add image
            Elements.BaseElement element = new Elements.Image()
            { 
                FileName = fileName,
                Width = (int)size.Width,
                Height = (int)size.Height
            };
            
            ExtractId(htmlNode, ref element);

            elements.Add(element);
        }

        private static void ParseHorizontalLine(HtmlNode htmlNode, ref List<Elements.BaseElement> elements)
        {
            // remove previous break if we got any (hr got it's own margin)
            RemoveLastBreak(ref elements);
 
            // add line
            Elements.BaseElement element = new Elements.Line();

            ExtractId(htmlNode, ref element);

            elements.Add(element);
        }

        private void ParsePreformattedText(HtmlNode htmlNode, ref List<Elements.BaseElement> elements)
        {
            string innerText = htmlNode.InnerText;

            innerText = innerText.Replace(Globals.NewLine, "<br/>"); // new line
            innerText = innerText.Replace(Globals.TabEscape, "    "); // tab escape

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(innerText);

            ParseNodeChilds(htmlDoc.DocumentNode, ref elements);
        }

        private static void ParseDefnitionList(HtmlNode htmlNode, ref List<Elements.BaseElement> elements)
        {
            AddBreak(ref elements);

            foreach (HtmlNode hn in htmlNode.ChildNodes)
            {
                if (hn.Name == "dt")
                {
                    ParseText(hn, ref elements, 1, FontStyles.Normal, FontWeights.Normal, null, null);

                    AddBreak(ref elements);
                }
                else if (hn.Name == "dd")
                {
                    // tab
                    HtmlNode hnTab = hn.Clone();
                    hnTab.InnerHtml = "      " + hnTab.InnerHtml;
                    
                    ParseText(hnTab, ref elements, 1, FontStyles.Normal, FontWeights.Normal, null, null);

                    AddBreak(ref elements);
                }
            }
        }

        private void ParseTable(HtmlNode htmlNode, ref List<Elements.BaseElement> elements)
        {
            AddBreak(ref elements);

            foreach (HtmlNode hnTr in htmlNode.ChildNodes.Where(n => n.Name == "tr"))
            {
                foreach (HtmlNode hnTd in hnTr.ChildNodes.Where(n => n.Name == "td"))
                {
                    ParseNodeChilds(hnTd, ref elements);
                    AddBreak(ref elements);
                }
            }
        }

        private static void RemoveLastBreak(ref List<Elements.BaseElement> elements)
        {
            if (elements.Count > 0 && elements[elements.Count - 1] is Elements.Break)
            {
                elements.RemoveAt(elements.Count - 1);
            }
        }

        /// <summary>
        /// Extracts identifier from nodes (for table of contents linkng).
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <param name="element"></param>
        private static void ExtractId(HtmlNode htmlNode, ref Elements.BaseElement element)
        {
            if (htmlNode.Attributes.Contains("id"))
            {
                element.Identifiers.Add(htmlNode.Attributes["id"].Value);
            }

            // we have to also extract id's from parents
            if (htmlNode.ParentNode != null)
            {
                ExtractId(htmlNode.ParentNode, ref element);
            }
        }

        /// <summary>
        /// Extracts source from anchor.
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <param name="element"></param>
        private static void ExtractHref(HtmlNode htmlNode, ref Elements.BaseElement element)
        {
            if (htmlNode.Attributes.Contains("href"))
            {
                ((Elements.Text)element).Href = htmlNode.Attributes["href"].Value;
            }            
        }
    }
}
