// --------------------------------
// <copyright file="Renderer.cs" company="Cubicsoft (www.cubicsoft.pl)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Jakub Florczyk (www.jakubflorczyk.pl)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://epubreaderwp.codeplex.com</website>
// ---------------------------------

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Documents;
using System.Text;

namespace EPubReader
{
    internal class Renderer
    {
        private EPubViewer _ePubViewer;

        private Renderer()
        {
        }

        internal Renderer(EPubViewer ePubViewer)
        {
            _ePubViewer = ePubViewer;
        }

        /// <summary>
        /// Render item elements container
        /// </summary>
        /// <param name="iec"></param>
        internal void Render(ItemElementsContainer iec)
        {
#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
            int currentLocation = iec.StartLocation;

            Page page = CreatePage(currentLocation);
            double top = 0;

            for (int i = 0; i < iec.Elements.Count; i++)
            {
                Elements.BaseElement element = iec.Elements[i];
                
                if (element is Elements.Break)
                {
                    RenderBreak(iec, currentLocation, ref page, ref top);
                }
                else if (element is Elements.Image)
                {
                    RenderImage(iec, currentLocation, ref page, ref top, element);
                }
                else if (element is Elements.Line)
                {
                    RenderLine(iec, currentLocation, ref page, ref top);
                }
                else if (element is Elements.Text)
                {
                    RenderText(iec, currentLocation, ref page, ref top, element);
                }

                if (i < iec.Elements.Count - 1)
                {
                    currentLocation++;
                }
            }

            // we push page even doesnt have any content
            page.EndLocation = currentLocation;
            // add page to list
            iec.Pages.Add(page);

#if DEBUG
            sw.Stop();
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "Information: EPubReader.Renderer.Render(\"{1}\") - Executed in {0} ms", sw.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture), iec.Item.Href));
#endif  
        }

        private void RenderBreak(ItemElementsContainer iec, int currentLocation, ref Page page, ref double top)
        {
            // don't add break at begining
            if (page.Content.Children.Count == 0)
            {
                return;
            }

            // measure
            if (top + _ePubViewer.LineHeight > page.Content.Height)
            {
                // push page and create new one
                PushPage(iec, ref page, currentLocation, ref top);
                //we don't render break at begging, so we can return now
            }
            else
            {
                TextBlock tb = ExtractTextBlock(ref page, top);
                tb.Inlines.Add(new LineBreak());

                top += _ePubViewer.LineHeight;
            }
        }

        private void RenderImage(ItemElementsContainer iec, int currentLocation, ref Page page, ref double top, Elements.BaseElement element)
        {
            Elements.Image imageElement = (Elements.Image)element;

            // create UI element
            Image image = new Image();
            image.Tag = imageElement;

            Binding binding = new Binding("Tag");
            binding.RelativeSource = new RelativeSource(RelativeSourceMode.Self);
            binding.Converter = new Converters.IsolatedStorageFileToImageSourceConverter();
            binding.ConverterParameter = _ePubViewer.Book;
            BindingOperations.SetBinding(image, Image.SourceProperty, binding);

            // check size of image (it it's larger then entire space)
            if (imageElement.Width + image.Margin.Left + image.Margin.Right > page.Content.Width
                ||
                imageElement.Height + image.Margin.Top + image.Margin.Bottom > page.Content.Height)
            {
                // image bigger then size of control     
                if (page.Content.Children.Count > 0)
                {
                    PushPage(iec, ref page, currentLocation, ref top);
                }

                // count new width / height
                double widthRatio = (double)imageElement.Width / page.Content.Width;
                double heightRatio = (double)imageElement.Height / page.Content.Height;
                double ratio = Math.Max(widthRatio, heightRatio);
                image.Width = (int)((double)imageElement.Width / ratio);
                image.Height = (int)((double)imageElement.Height / ratio);

                image.Stretch = Stretch.Uniform;

                image.SetValue(Canvas.TopProperty, top);

                page.Content.Children.Add(image);
            }
            else
            {
                // image smaller then size of control

                // try fit current page
                if (top + imageElement.Height + image.Margin.Top + image.Margin.Bottom > page.Content.Height)
                {
                    PushPage(iec, ref page, currentLocation, ref top);
                }

                image.Width = imageElement.Width;
                image.Height = imageElement.Height;

                image.Stretch = Stretch.None;

                image.SetValue(Canvas.TopProperty, top);

                page.Content.Children.Add(image);                
            }

            top += image.Height + image.Margin.Top + image.Margin.Bottom;
        }
        
        private void RenderLine(ItemElementsContainer iec, int currentLocation, ref Page page, ref double top)
        {
            // check if line fits
            if (top + Elements.Line.Height + (2 * Elements.BaseElement.Margin) > page.Content.Height)
            {
                PushPage(iec, ref page, currentLocation, ref top);
            }

            Rectangle rectangle = new Rectangle();
            rectangle.Height = Elements.Line.Height;
            rectangle.Stroke = new SolidColorBrush(Colors.Red);

            Binding binding = new Binding("Foreground");
            binding.Source = _ePubViewer;
            BindingOperations.SetBinding(rectangle, Rectangle.FillProperty, binding);
            BindingOperations.SetBinding(rectangle, Rectangle.StrokeProperty, binding);

            top += Elements.BaseElement.Margin; // top margin

            rectangle.SetValue(Canvas.TopProperty, top);

            page.Content.Children.Add(rectangle);

            top += Elements.Line.Height + Elements.BaseElement.Margin; // height and bottom margin
        }

        private void RenderText(ItemElementsContainer iec, int currentLocation, ref Page page, ref double top, Elements.BaseElement element)
        {
            Elements.Text textElement = (Elements.Text)element;

            string value = "  " + textElement.Value;

            TextBlock tb = ExtractTextBlock(ref page, top);

            double prevTextBlockActualHeight = tb.ActualHeight;
            tb.Inlines.Add(CreateRun(textElement, value));
            double textBlockActualHeight = tb.ActualHeight - prevTextBlockActualHeight;

            // check if fits (in most cases it will pass, because of short paragraphs)
            if (top + textBlockActualHeight > page.Content.Height)            
            {
                // remove inline
                tb.Inlines.RemoveAt(tb.Inlines.Count - 1);

                int idx = 1;
                string[] parts = value.Split(' ');

                while (true)
                {
                    string text = StringJoin(' ', parts, idx);                    
                    prevTextBlockActualHeight = tb.ActualHeight;
                    tb.Inlines.Add(CreateRun(textElement, text));
                    textBlockActualHeight = tb.ActualHeight - prevTextBlockActualHeight;                    
                    if (top + textBlockActualHeight > page.Content.Height)
                    {
                        // remove inline
                        tb.Inlines.RemoveAt(tb.Inlines.Count - 1);

                        // push prev text
                        // it could be empty if nothing fits
                        string prevText = null;
                        if (idx > 1)
                        {
                            prevText = StringJoin(' ', parts, idx - 1);

                            tb.Inlines.Add(CreateRun(textElement, prevText));
                        }

                        PushPage(iec, ref page, currentLocation, ref top);

                        // render rest of text
                        if (idx > 1)
                        {
                            textElement.Value = value.Substring(prevText.Length);
                        }
                        else
                        {
                            textElement.Value = value;
                        }

                        RenderText(iec, currentLocation, ref page, ref top, element);

                        break;
                    }
                    else
                    {
                        // remove inline
                        tb.Inlines.RemoveAt(tb.Inlines.Count - 1);
                    }

                    idx++;
                }
            }
            else
            {
                // create anchor rect if href isn't null
                if (!String.IsNullOrEmpty(textElement.Href))
                {
                    AnchorRect anchorRect = new AnchorRect();
                    anchorRect.Href = textElement.Href;
                    anchorRect.Rect = new Rect(
                        _ePubViewer.ContentMargin.Left,
                        (textBlockActualHeight == 0) ? top - _ePubViewer.LineHeight : top,
                        _ePubViewer.ContentSize.Width,
                        (textBlockActualHeight == 0) ? _ePubViewer.LineHeight : textBlockActualHeight);
                   
                    page.AnchorRects.Add(anchorRect);
                }

                top += textBlockActualHeight;
            }            
        }

        private void PushPage(ItemElementsContainer iec, ref Page page, int location, ref double top)
        {
            // update end location on page
            page.EndLocation = location;
            // add page to list
            iec.Pages.Add(page);
            // create new page
            page = CreatePage(location + 1);
            // update top
            top = 0;
        }

        private Page CreatePage(int startLocation)
        {
            Page page = new Page();

            page.StartLocation = startLocation;
            page.Content.Width = _ePubViewer.ContentSize.Width;
            page.Content.Height = _ePubViewer.ContentSize.Height;
            page.Content.Margin = _ePubViewer.ContentMargin;

            return page;
        }

        private TextBlock ExtractTextBlock(ref Page page, double top)
        {
            if (page.Content.Children.Count > 0 && page.Content.Children[page.Content.Children.Count - 1] is TextBlock)
            {
                return (TextBlock)page.Content.Children[page.Content.Children.Count - 1];
            }

            TextBlock tb = CreateTextBlock(page, top);

            page.Content.Children.Add(tb);

            return (TextBlock)page.Content.Children[page.Content.Children.Count - 1];
        }

        private TextBlock CreateTextBlock(Page page, double top)
        {
            TextBlock tb = new TextBlock();

            tb.Width = page.Content.Width;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.FontFamily = _ePubViewer.FontFamily;
            tb.LineHeight = _ePubViewer.LineHeight;

            Binding binding = new Binding("Foreground");
            binding.Source = _ePubViewer;
            BindingOperations.SetBinding(tb, TextBlock.ForegroundProperty, binding);

            tb.SetValue(Canvas.TopProperty, top);

            return tb;
        }

        private Run CreateRun(Elements.Text textElement, string text)
        {
            Run run = new Run();

            run.FontSize = _ePubViewer.FontSize * textElement.FontSizeMultiplier;
            run.FontStyle = textElement.FontStyle.ToFontStyle();
            run.FontWeight = textElement.FontWeight.ToFontWeight();
            if (textElement.Foreground != null)
            {
                // force Foreground - for links
                run.Foreground = textElement.Foreground;
            }
            run.TextDecorations = textElement.Decoration.ToTextDecorationCollection();
            run.Text = text;

            return run;
        }

        /// <summary>
        /// Alternative version of join.
        /// </summary>
        /// <param name="separator"></param>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private static string StringJoin(char separator, string[] value, int count)
        {
            StringBuilder joined = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                if (value[i].Length == 0)
                {
                    joined.Append(separator);
                }
                else
                {
                    joined.Append(value[i]);
                    if (i < count - 1)
                    {
                        joined.Append(separator);
                    }
                }

            }
            return joined.ToString();
        }  
    }
}
