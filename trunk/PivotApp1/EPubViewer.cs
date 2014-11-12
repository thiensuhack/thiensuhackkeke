// --------------------------------
// <copyright file="EPubViewer.cs" company="Cubicsoft (www.cubicsoft.pl)">
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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using EPubReader.Utilities;
using Microsoft.Phone.Tasks;
using System.Threading;
using System.ComponentModel;

namespace EPubReader
{
    /// <summary>
    /// Represents a EPubViewer control.
    /// </summary>
    [TemplateVisualState(Name = NormalState, GroupName = CommonStateGroup)]
    [TemplateVisualState(Name = CoverState, GroupName = CommonStateGroup)]
    [TemplateVisualState(Name = TocState, GroupName = CommonStateGroup)]
    [TemplatePart(Name = ContentCanvasName, Type = typeof(Canvas))]
    [TemplatePart(Name = CoverImageName, Type = typeof(Image))]
    [TemplatePart(Name = TocListBoxName, Type = typeof(ListBox))]
    [TemplatePart(Name = ProgressGridName, Type = typeof(Grid))]
    public class EPubViewer : Control, IDisposable
    {
        #region Consts 

        private const string CommonStateGroup = "CommonStates";

        private const string NormalState = "Normal";
        private const string CoverState = "Cover";
        private const string TocState = "Toc";

        private const string ModeSettingsKey = "EPubViewer_Mode";
        private const string FontSizeSettingsKey = "EPubViewer_FontSize";
        private const string FontFamilySettingsKey = "EPubViewer_FontFamily";
        private const string LineHeightSettingsKey = "EPubViewer_LineHeight";
        private const string BrightnessSettingsKey = "EPubViewer_Brightness";
        private const string ContentMarginSettingsKey = "EPubViewer_ContentMargin";

        private const string ContentCanvasName = "ContentCanvas";
        private const string CoverImageName = "CoverImage";
        internal const string TocListBoxName = "TocListBox";
        internal const string ProgressGridName = "ProgressGrid";

        #endregion

        #region Fields

        // xaml elements
        private Canvas _contentCanvas;
        private Image _coverImage;
        private BitmapImage _coverBitmapImage;
        private ListBox _tocListBox;
        private Grid _progressGrid;

        // internal process classes
        private Loader _loader;
        private Parser _parser;
        private Renderer _renderer;

        // current element helpers
        private int _currentIecIndex;
        private int _currentPageIndex;

        // disposing
        private bool _disposed;

        // content size
        private Size _contentSize;

        #endregion

        #region Internal properties

        internal Size ContentSize
        {
            get { return _contentSize; }
        }

        #endregion

        #region Source DependencyProperty

        private const Stream DefaultSource = null;

        /// <summary>
        /// Identifies the Source dependency property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Stream), typeof(EPubViewer), new PropertyMetadata(DefaultSource, OnSourcePropertyChanged));

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        public Stream Source
        {
            get { return (Stream)this.GetValue(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Source changed handler.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnSourcePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((EPubViewer)obj).OnSourcePropertyChanged();
        }

        #endregion

        #region Mode DependencyProperty

        private const Mode DefaultMode = Mode.Day;

        /// <summary>
        /// Identifies the Mode dependency property.
        /// </summary>
        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(Enum), typeof(EPubViewer), new PropertyMetadata(DefaultMode, OnModePropertyChanged));        

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        public Mode Mode
        {
            get { return (Mode)this.GetValue(ModeProperty); }
            set { this.SetValue(ModeProperty, value); }
        }

        /// <summary>
        /// Mode changed handler.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnModePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((EPubViewer)obj).OnModePropertyChanged();
        }

        #endregion

        #region State DependencyProperty

        private const State DefaultState = State.Normal;

        /// <summary>
        /// Identifies the State dependency property.
        /// </summary>
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(Enum), typeof(EPubViewer), new PropertyMetadata(DefaultState, OnStatePropertyChanged));

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public State State
        {
            get { return (State)this.GetValue(StateProperty); }
            set { this.SetValue(StateProperty, value); }
        }

        /// <summary>
        /// State changed handler.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnStatePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((EPubViewer)obj).OnStatePropertyChanged();
        }

        #endregion

        #region Book DependencyProperty

        private const Book DefaultBook = null;

        /// <summary>
        /// Identifies the Book dependency property.
        /// </summary>
        public static readonly DependencyProperty BookProperty = DependencyProperty.Register("Book", typeof(Book), typeof(EPubViewer), new PropertyMetadata(DefaultBook));

        /// <summary>
        /// Gets the book.
        /// </summary>
        public Book Book
        {
            get { return (Book)this.GetValue(BookProperty); }
            private set { this.SetValue(BookProperty, value); }
        }

        #endregion

        #region FontSize DependencyProperty

        private const double DefaultFontSize = 26;

        /// <summary>
        /// Identifies the FontSize dependency property.
        /// </summary>
        public new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register("FontSize", typeof(double), typeof(EPubViewer), new PropertyMetadata(DefaultFontSize, OnFontSizePropertyChanged));

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        public new double FontSize
        {
            get { return (double)this.GetValue(FontSizeProperty); }
            set { this.SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// FontSize changed handler.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnFontSizePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((EPubViewer)obj).OnFontSizePropertyChanged();
        }

        #endregion

        #region FontFamily DependencyProperty

        private static readonly FontFamily DefaultFontFamily = new FontFamily("Arial");

        /// <summary>
        /// Identifies the FontFamily dependency property.
        /// </summary>
        public new static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(EPubViewer), new PropertyMetadata(DefaultFontFamily, OnFontFamilyPropertyChanged));

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        public new FontFamily FontFamily
        {
            get { return (FontFamily)this.GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// FontFamily changed handler.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnFontFamilyPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((EPubViewer)obj).OnFontFamilyPropertyChanged();
        }

        #endregion

        #region LineHeight DependencyProperty

        private const double DefaultLineHeight = 39;

        /// <summary>
        /// Identifies the LineHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty LineHeightProperty = DependencyProperty.Register("LineHeight", typeof(double), typeof(EPubViewer), new PropertyMetadata(DefaultLineHeight, OnLineHeightPropertyChanged));

        /// <summary>
        /// Gets or sets the line height.
        /// </summary>
        public double LineHeight
        {
            get { return (double)this.GetValue(LineHeightProperty); }
            set { this.SetValue(LineHeightProperty, value); }
        }

        /// <summary>
        /// LineHeight changed handler.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnLineHeightPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((EPubViewer)obj).OnLineHeightPropertyChanged();
        }

        #endregion

        #region Brightness DependencyProperty

        private const double DefaultBrightness = 1d;

        /// <summary>
        /// Identifies the Brightness dependency property.
        /// </summary>
        public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register("Brightness", typeof(double), typeof(EPubViewer), new PropertyMetadata(DefaultBrightness, OnBrightnessPropertyChanged));

        /// <summary>
        /// Gets or sets the brightness.
        /// </summary>
        public double Brightness
        {
            get { return (double)this.GetValue(BrightnessProperty); }
            set { this.SetValue(BrightnessProperty, value); }
        }

        /// <summary>
        /// Brightness changed handler.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnBrightnessPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((EPubViewer)obj).OnBrightnessPropertyChanged();
        }

        #endregion

        #region FurthestLocation DependencyProperty

        private const int DefaultFurthestLocation = 0;
        
        /// <summary>
        /// Identifies the FurthestLocation dependency property.
        /// </summary>
        public static readonly DependencyProperty FurthestLocationProperty = DependencyProperty.Register("FurthestLocation", typeof(int), typeof(EPubViewer), new PropertyMetadata(DefaultFurthestLocation));        

        /// <summary>
        /// Gets the furthest location.
        /// </summary>
        public int FurthestLocation
        {
            get { return (int)this.GetValue(FurthestLocationProperty); }
            private set { this.SetValue(FurthestLocationProperty, value); }
        }        

        #endregion

        #region CurrentLocation DependencyProperty

        private const int DefaultCurrentLocation = -1;

        /// <summary>
        /// Identifies the CurrentLocation dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentLocationProperty = DependencyProperty.Register("CurrentLocation", typeof(int), typeof(EPubViewer), new PropertyMetadata(DefaultCurrentLocation, OnCurrentLocationPropertyChanged));        

        /// <summary>
        /// Gets the current location.
        /// </summary>
        public int CurrentLocation
        {
            get { return (int)this.GetValue(CurrentLocationProperty); }
            set { this.SetValue(CurrentLocationProperty, value); }
        }

        /// <summary>
        /// CurrentLocation changed handler.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnCurrentLocationPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((EPubViewer)obj).OnCurrentLocationPropertyChanged((int)e.OldValue, (int)e.NewValue);
        }
        
        #endregion

        #region HasCover DependencyProperty

        private const bool DefaultHasCover = false;

        /// <summary>
        /// Identifies the HasCover dependency property.
        /// </summary>
        public static readonly DependencyProperty HasCoverProperty = DependencyProperty.Register("HasCover", typeof(bool), typeof(EPubViewer), new PropertyMetadata(DefaultHasCover));

        /// <summary>
        /// Identifies the book has cover.
        /// </summary>
        public bool HasCover
        {
            get { return (bool)this.GetValue(HasCoverProperty); }
            private set { this.SetValue(HasCoverProperty, value); }
        }

        #endregion

        #region HasToc DependencyProperty

        private const bool DefaultHasToc = false;

        /// <summary>
        /// Identifies the HasToc dependency property.
        /// </summary>
        public static readonly DependencyProperty HasTocProperty = DependencyProperty.Register("HasToc", typeof(bool), typeof(EPubViewer), new PropertyMetadata(DefaultHasToc));

        /// <summary>
        /// Identifies the book has table of contents.
        /// </summary>
        public bool HasToc
        {
            get { return (bool)this.GetValue(HasTocProperty); }
            private set { this.SetValue(HasTocProperty, value); }
        }

        #endregion

        #region ContentMargin DependencyProperty

        private static readonly Thickness DefaultContentMargin = new Thickness(12);

        /// <summary>
        /// Identifies the ContentMargin dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(EPubViewer), new PropertyMetadata(DefaultContentMargin, OnContentMarginPropertyChanged));

        /// <summary>
        /// Identifies the book content margin.
        /// </summary>
        public Thickness ContentMargin
        {
            get { return (Thickness)this.GetValue(ContentMarginProperty); }
            set { this.SetValue(ContentMarginProperty, value); }
        }

        /// <summary>
        /// ContentMargin changed handler.
        /// </summary>
        /// <param name="obj">The dependency object.</param>
        /// <param name="e">The event information.</param>
        private static void OnContentMarginPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((EPubViewer)obj).OnContentMarginPropertyChanged();
        }

        #endregion

        #region IsCacheEnabled DependencyProperty

        private const bool DefaultIsCacheEnabled = true;

        /// <summary>
        /// Identifies the IsCacheEnabled dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCacheEnabledProperty = DependencyProperty.Register("IsCacheEnabled", typeof(bool), typeof(EPubViewer), new PropertyMetadata(DefaultIsCacheEnabled));

        /// <summary>
        /// Gets or sets the is cache enabled.
        /// </summary>
        public bool IsCacheEnabled
        {
            get { return (bool)this.GetValue(IsCacheEnabledProperty); }
            set { this.SetValue(IsCacheEnabledProperty, value); }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when book is opened.
        /// </summary>
        public event RoutedEventHandler BookOpened;

        /// <summary>
        /// Occurs when there is an error.
        /// </summary>
        public event EventHandler<ExceptionRoutedEventArgs> BookFailed;

        /// <summary>
        /// Occurs when current location changed.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<int> CurrentLocationChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes new instance of EPubViewer class.
        /// </summary>
        public EPubViewer()
        {
            // set template for the control
            DefaultStyleKey = typeof(EPubViewer);

            // activate database
            DatabaseService.Activate();

            SizeChanged += new SizeChangedEventHandler(EPubViewer_SizeChanged);

            ReadSettings();
        }

        private void ReadSettings()
        {
            if (DesignerProperties.IsInDesignTool)
                return;

            if (IsolatedStorageSettings.ApplicationSettings.Contains(ModeSettingsKey))
            {
                try
                {
                    Mode = (Mode)IsolatedStorageSettings.ApplicationSettings[ModeSettingsKey];
                }
                catch { }
            }

            if (IsolatedStorageSettings.ApplicationSettings.Contains(FontSizeSettingsKey))
            {
                try
                {
                    FontSize = (double)IsolatedStorageSettings.ApplicationSettings[FontSizeSettingsKey];
                }
                catch { }
            }

            if (IsolatedStorageSettings.ApplicationSettings.Contains(FontFamilySettingsKey))
            {
                try
                {
                    FontFamily = (FontFamily)IsolatedStorageSettings.ApplicationSettings[FontFamilySettingsKey];
                }
                catch { }
            }

            if (IsolatedStorageSettings.ApplicationSettings.Contains(LineHeightSettingsKey))
            {
                try
                {
                    LineHeight = (double)IsolatedStorageSettings.ApplicationSettings[LineHeightSettingsKey];
                }
                catch { }
            }

            if (IsolatedStorageSettings.ApplicationSettings.Contains(BrightnessSettingsKey))
            {
                try
                {
                    Brightness = (double)IsolatedStorageSettings.ApplicationSettings[BrightnessSettingsKey];
                }
                catch { }
            }

            if (IsolatedStorageSettings.ApplicationSettings.Contains(ContentMarginSettingsKey))
            {
                try
                {
                    ContentMargin = (Thickness)IsolatedStorageSettings.ApplicationSettings[ContentMarginSettingsKey];
                }
                catch { }
            }
        }

        #endregion

        #region Event overrides

        /// <summary>
        /// Applies the template.
        /// </summary>
        public override void OnApplyTemplate()
        {            
            base.OnApplyTemplate();

            _contentCanvas = (Canvas)GetTemplateChild(ContentCanvasName);

            _coverImage = (Image)GetTemplateChild(CoverImageName);
            if (_coverImage != null)
            {
                _coverImage.Source = _coverBitmapImage;
            }

            _tocListBox = (ListBox)GetTemplateChild(TocListBoxName);
            if (_tocListBox != null)
            {
                _tocListBox.SelectionChanged += TocListBox_SelectionChanged;
            }

            _progressGrid = (Grid)GetTemplateChild(ProgressGridName);
            
            UpdateStateVisualStates(true);

            UpdateModeVisualStates();

            LoadBook();
        }

        /// <summary>
        /// Called before the System.Windows.UIElement.Tap event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnTap(GestureEventArgs e)
        {
            if (State != EPubReader.State.Normal)
            {
                return;
            }

            Point p = e.GetPosition(this);

            // check anchor rects
            if (_currentIecIndex > DefaultCurrentLocation && Book.ItemElementsContainers[_currentIecIndex].Pages.Count > 0)
            {                
                AnchorRect anchorRect = Book.ItemElementsContainers[_currentIecIndex].Pages[_currentPageIndex].AnchorRects.Where(ar => ar.Rect.Contains(p)).FirstOrDefault();
                if (anchorRect != null)
                {
                    if (anchorRect.Href.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || anchorRect.Href.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            new WebBrowserTask()
                            {
                                Uri = new Uri(anchorRect.Href)
                            }.Show();
                        }
                        catch { }
                    }
                    else if (anchorRect.Href.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            new EmailComposeTask()
                            {
                                To = anchorRect.Href.Substring(7) // substring "mailto:"
                            }.Show();
                        }
                        catch { }
                    }
                    else
                    {
                        Navigate(anchorRect.Href);
                    }

                    e.Handled = true;
                }
            }

            if (!e.Handled && p.X <= ActualWidth / 3 && CurrentLocation > 0)
            {
                // previous page
                PreviousPage();

                e.Handled = true;
            }
            else if (!e.Handled && p.X >= ActualWidth * 2 / 3 && CurrentLocation < FurthestLocation - 1)
            {
                // next page
                NextPage();

                e.Handled = true;
            }
            else 

            base.OnTap(e);
        }

        private void UpdateStateVisualStates(bool useTransitions)
        {
            switch (State)
            {
                case State.Normal:
                    VisualStateManager.GoToState(this, NormalState, useTransitions);
                    break;
                case State.Cover:
                    VisualStateManager.GoToState(this, CoverState, useTransitions);
                    break;
                case State.Toc:
                    VisualStateManager.GoToState(this, TocState, useTransitions);
                    break;
            }
        }

        private void UpdateModeVisualStates()
        {
            byte c = (byte)(Brightness * 128 + 127);

            switch (Mode)
            {
                case Mode.Day:                    
                    Background = new SolidColorBrush(Color.FromArgb(255, c, c, c));
                    Foreground = new SolidColorBrush(Colors.Black);
                    break;
                case Mode.Night:
                    Background = new SolidColorBrush(Colors.Black);
                    Foreground = new SolidColorBrush(Color.FromArgb(255, c, c, c));
                    break;
            }
        }        

        /// <summary>
        /// Called when the DocumentOpened event occurs.
        /// </summary>
        protected virtual void OnDocumentOpened()
        {
            if (BookOpened != null)
            {
                BookOpened(this, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Called when the DocumentFailed event occurs.
        /// </summary>
        /// <param name="exception">The exception information.</param>
        protected virtual void OnDocumentFailed(Exception exception)
        {
            if (BookFailed != null)
            {
                BookFailed(this, new ExceptionRoutedEventArgs(exception));
            }
        }

        /// <summary>
        /// Called when the value of the CurrentLocation property changes.
        /// </summary>
        private void OnCurrentLocationPropertyChanged(int oldValue, int newValue)
        {
            if (newValue < 0)
            {
                throw new EPubReaderException("Current location number cannot be less then 0");
            }
            else if (newValue > FurthestLocation)
            {
                throw new EPubReaderException("Current location number cannot be bigger then FurthestLocation");
            }

            ShowCurrentLocation();

            if (CurrentLocationChanged != null)
            {
                CurrentLocationChanged(this, new RoutedPropertyChangedEventArgs<int>(oldValue, newValue));
            }
        }       

        /// <summary>
        /// Called when the value of the State property changes.
        /// </summary>
        private void OnStatePropertyChanged()
        {
            UpdateStateVisualStates(true);
        }

        /// <summary>
        /// Called when the value of the Mode property changes.
        /// </summary>
        private void OnModePropertyChanged()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                IsolatedStorageSettings.ApplicationSettings[ModeSettingsKey] = Mode;
            }

            UpdateModeVisualStates();            
        }

        /// <summary>
        /// Called when the value of the Source property changes.
        /// </summary>
        private void OnSourcePropertyChanged()
        {
            LoadBook();
        }

        private void LoadBook()
        {
            if (Source == null)
            {
                return;
            }

            if (_contentCanvas == null)
            {
                return;
            }

            ShowProgress();

            ThreadPool.QueueUserWorkItem((object state) =>
            {                            
                Dispatcher.BeginInvoke(() =>
                {
                    LoadBookAsync();
                });
            });
        }

        private void LoadBookAsync()
        {
            // create new book
            Book = new Book();
            Book.ReaderVersion = Globals.Version;

#if DEBUG
            Book.Files = ZipHelper.Extract(Source);
#else
            try
            {
                Book.Files = ZipHelper.Extract(newValue);
            }
            catch (Exception ex)
            {
                OnDocumentFailed(ex);
                return;
            }
#endif

            // load book
            if (_loader == null)
            {
                _loader = new Loader(this);
            }

#if DEBUG
            _loader.Load();
#else
            try
            {
                _loader.Load();
            }
            catch (Exception ex)
            {
                OnDocumentFailed(ex);
                return;
            }
#endif
            // we have to load book before we gonna try to get it from cache
            // we can need files and we need Identifier
            TryGetBookFromCache();

            // check book has cover
            HasCover = CheckHasCover();
            if (!HasCover)
            {
                _coverBitmapImage = null;
            }

            // check book has ToC
            HasToc = Book.Toc != null;
            if (_tocListBox != null)
            {
                BindToc();
            }

            // parse
            if (_parser == null)
            {
                _parser = new Parser(this);
            }

#if DEBUG
            _parser.Parse();
#else
            try
            {
                _parser.Parse();
            }
            catch (Exception ex)
            {
                OnDocumentFailed(ex);
                return;
            }
#endif
            // update furthest location
            FurthestLocation = Book.ItemElementsContainers.Last().EndLocation;

            // clear storage
            IsolatedStorageHelper.ClearStorageZipDirectory();

            Render();

            OnDocumentOpened();
        }

        /// <summary>
        /// Called when the value of the FontSize property changes.
        /// </summary>
        private void OnFontSizePropertyChanged()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                IsolatedStorageSettings.ApplicationSettings[FontSizeSettingsKey] = FontSize;
            }

            Render();
        }

        /// <summary>
        /// Called when the value of the FontFamily property changes.
        /// </summary>
        private void OnFontFamilyPropertyChanged()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                IsolatedStorageSettings.ApplicationSettings[FontFamilySettingsKey] = FontFamily;
            }

            Render();
        }

        /// <summary>
        /// Called when the value of the LineHeight property changes.
        /// </summary>
        private void OnLineHeightPropertyChanged()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                IsolatedStorageSettings.ApplicationSettings[LineHeightSettingsKey] = LineHeight;
            }

            Render();
        }

        /// <summary>
        /// Called when the value of the Brightness property changes.
        /// </summary>
        private void OnBrightnessPropertyChanged()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                IsolatedStorageSettings.ApplicationSettings[BrightnessSettingsKey] = Brightness;
            }

            UpdateModeVisualStates();
        }

        /// <summary>
        /// Called when the value of the ContentMargin property changes.
        /// </summary>
        private void OnContentMarginPropertyChanged()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                IsolatedStorageSettings.ApplicationSettings[ContentMarginSettingsKey] = ContentMargin;
            }

            Render();
        }

        private void Render()
        {            
            if (Book == null)
            {
                return;
            }

            if (ActualWidth == 0 || ActualHeight == 0)
            {
                return;
            }

            // clear pages if some exists
            Book.ItemElementsContainers.ForEach((itemElementsContainer) => 
            {
                itemElementsContainer.Pages.Clear();
            });

            // update content size
            _contentSize = new Size(ActualWidth - (ContentMargin.Left + ContentMargin.Right), ActualHeight - (ContentMargin.Top + ContentMargin.Bottom));

            // render            
            if (_renderer == null)
            {
                _renderer = new Renderer(this);
            }

            ItemElementsContainer iec;
            if (CurrentLocation == DefaultCurrentLocation)
            {
                // select first
                iec = Book.ItemElementsContainers.First();
            }
            else
            {
                // select current
                iec = Book.ItemElementsContainers.Where(i => i.StartLocation <= CurrentLocation && i.EndLocation >= CurrentLocation).First();
            }

            ShowProgress();
            
#if DEBUG
            _renderer.Render(iec);
#else
            try
            {
                _renderer.Render(iec);
            }
            catch (Exception ex)
            {
                OnDocumentFailed(ex);
                return;
            }
#endif
            HideProgress();
            
            // update current location if we still on DefaultCurrentLocation
            if (CurrentLocation == DefaultCurrentLocation)
            {
                CurrentLocation = iec.StartLocation;
            }
            else
            {
                // force show current location
                ShowCurrentLocation();
            }
        }

        private void ShowCurrentLocation()
        {
            if (_contentCanvas != null && Book != null && CurrentLocation > DefaultCurrentLocation)
            {
                // search for location within ItemElementsContainers
                for (int i = 0; i < Book.ItemElementsContainers.Count; i++)
                {
                    ItemElementsContainer iec = Book.ItemElementsContainers[i];
                    if (iec.StartLocation <= CurrentLocation && iec.EndLocation >= CurrentLocation)
                    {
                        if (iec.Pages.Count == 0)
                        {
                            ShowProgress();

                            ThreadPool.QueueUserWorkItem((object state) =>
                            {                            
                                Dispatcher.BeginInvoke(() =>
                                {
                                    _renderer.Render(iec);
                                    HideProgress();
                                    ShowCurrentPage(iec, i);
                                });
                            });

                            break;
                        }
                        else
                        {
                            ShowCurrentPage(iec, i);
                        }
                    }
                }
            }
        }

        private void ShowCurrentPage(ItemElementsContainer iec, int iecIndex)
        {
            // find page
            for (int i = 0; i < iec.Pages.Count; i++)
            {
                Page p = iec.Pages[i];
                if (p.StartLocation <= CurrentLocation && p.EndLocation >= CurrentLocation)
                {
                    _currentIecIndex = iecIndex;
                    _currentPageIndex = i;

                    // fill content
                    _contentCanvas.Children.Clear();
                    _contentCanvas.Children.Add(p.Content);
                    // exit
                    return;
                }
            }
        }

        private void PreviousPage()
        {
            // it's first page in book
            if (_currentIecIndex == 0 && _currentPageIndex == 0)
            {
                return;
            }
            
            // first page in ItemElementsContainer
            if (_currentPageIndex == 0)
            {
                // get previous ItemElementsContainer
                CurrentLocation = Book.ItemElementsContainers[_currentIecIndex - 1].EndLocation;
            }
            else
            {
                // get previous page
                CurrentLocation = Book.ItemElementsContainers[_currentIecIndex].Pages[_currentPageIndex - 1].EndLocation;
            }
        }

        private void NextPage()
        {
            // it's last page in book
            if (_currentIecIndex == Book.ItemElementsContainers.Count - 1 && _currentPageIndex == Book.ItemElementsContainers[_currentIecIndex].Pages.Count - 1)
            {
                return;
            }

            // last page in ItemElementsContainer
            if (_currentPageIndex == Book.ItemElementsContainers[_currentIecIndex].Pages.Count - 1)
            {
                // get next ItemElementsContainer
                CurrentLocation = Book.ItemElementsContainers[_currentIecIndex + 1].StartLocation;
            }
            else
            {
                // get next page
                CurrentLocation = Book.ItemElementsContainers[_currentIecIndex].Pages[_currentPageIndex + 1].StartLocation;
            }
        }

        private void BindToc()
        {            
            if (Book != null && Book.Toc != null)
            {
                List<NavigationPoint> navigationPonts = new List<NavigationPoint>();

                foreach (Tocs.NavPoint navPoint in Book.Toc.NavMap.NavPoints)
                {
                    if (navPoint.NavLabel.Text.Value.Length > 0) // skip empty Text
                    {
                        navigationPonts.Add(new NavigationPoint() { Text = navPoint.NavLabel.Text.Value, Src = navPoint.Content.Src, Level = 0 });
                    }

                    ExtractDescendantsNavPoints(navigationPonts, navPoint, 1);
                }

                _tocListBox.ItemsSource = navigationPonts;
            }
            else
            {
                _tocListBox.ItemsSource = null;
            }
        }

        private static void ExtractDescendantsNavPoints(List<NavigationPoint> navigationPonts, Tocs.NavPoint navPoint, int level)
        {
            if (navPoint.NavPoints != null)
            {
                foreach (Tocs.NavPoint navPointDescendant in navPoint.NavPoints)
                {
                    if (navPointDescendant.NavLabel.Text.Value.Length > 0) // skip empty Text
                    {
                        navigationPonts.Add(new NavigationPoint() 
                        { 
                            Text = navPointDescendant.NavLabel.Text.Value, 
                            Src = navPointDescendant.Content.Src, Level = level 
                        });
                    }

                    ExtractDescendantsNavPoints(navigationPonts, navPointDescendant, level + 1);
                }
            }
        }

        private bool CheckHasCover()
        {
            // check if book has cover
            Contents.Meta meta = Book.Metadata.Metas.SingleOrDefault(m => m.Name == "cover");
            if (meta != null)
            {
                // get manifest item
                Contents.Item item = Book.Manifest.Items.Where(i => i.Id.Equals(meta.Content)).SingleOrDefault();
                if (item == null)
                {
                    return false;
                }

                string path = Book.GetFilePath(item.Href);
                if (path == null)
                {
                    return false;
                }

                // read stream
                Stream s = IsolatedStorageHelper.ReadZipStream(path);
                if (s != null)
                {
                    // load cover
                    _coverBitmapImage = new BitmapImage();
                    _coverBitmapImage.SetSource(s);

                    if (_coverImage != null) // book can be load before OnApplyTemplate
                    {
                        _coverImage.Source = _coverBitmapImage;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void ShowProgress()
        {
            if (_progressGrid != null)
            {
                _progressGrid.Visibility = Visibility.Visible;
            }
        }

        private void HideProgress()
        {
            if (_progressGrid != null)
            {
                _progressGrid.Visibility = Visibility.Collapsed;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Epub viewer size changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EPubViewer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Render();
        }

        /// <summary>
        /// Selection changed on TOC listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TocListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_tocListBox.SelectedItem == null)
            {
                return;
            }

            Navigate(((NavigationPoint)_tocListBox.SelectedItem).Src);

            // clear selection
            _tocListBox.SelectedItem = null;
        }

        private void Navigate(string src)
        {
            // extract href / id
            string itemHref, id;
            if (src.StartsWith("#", StringComparison.OrdinalIgnoreCase))
            {
                itemHref = String.Empty;
                id = src.Substring(1);
            }
            else if (src.Contains('#'))
            {
                itemHref = src.Substring(0, src.IndexOf('#'));
                id = src.Substring(src.IndexOf('#') + 1);
            }
            else
            {
                itemHref = src;
                id = String.Empty;
            }

            // remove directiories from href
            itemHref = GetFileName(itemHref);
            
            // find location
            int loc = 0;
            for (int i = 0; i < Book.ItemElementsContainers.Count; i++)
            {
                ItemElementsContainer iec = Book.ItemElementsContainers[i];
                for (int j = 0; j < iec.Elements.Count; j++)
                {
                    Elements.BaseElement element = iec.Elements[j];                    
                    if (
                        (String.IsNullOrEmpty(itemHref) || GetFileName(iec.Item.Href).Equals(itemHref, StringComparison.OrdinalIgnoreCase))
                        &&
                        (String.IsNullOrEmpty(id) || (!String.IsNullOrEmpty(id) && element.Identifiers.Contains(id)))
                        )
                    {
                        // change state (from TOC view)
                        State = EPubReader.State.Normal;
                        // set location
                        CurrentLocation = loc + j;
                        // exit
                        return;
                    }
                }

                loc += iec.Elements.Count;
            }
        }

        private string GetFileName(string href)
        {
            // remove directiories from href
            if (href.Contains("/"))
            {
                return href.Substring(href.LastIndexOf("/") + 1);
            }
            else
            {
                return href;
            }
        }

        #endregion

        #region Private methods

        private void TryGetBookFromCache()
        {
            if (String.IsNullOrEmpty(Book.Metadata.Identifier.Value))
            {
                return;
            }

            if (IsCacheEnabled)
            {
                Book b = DatabaseService.Database.Load<Book>(Book.Metadata.Identifier.Value);
                if (b != null && b.ReaderVersion == Globals.Version)
                {
                    Book = b;
                }
            }
        }

        #endregion

        #region Internal methods

        internal void CacheBook()
        {
            if (IsCacheEnabled)
            {
                DatabaseService.Database.Save(Book);
                DatabaseService.Database.Flush();
            }
        }

        #endregion

        #region IDisposed implementation

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    IsolatedStorageHelper.ClearStorageZipDirectory();
                    DatabaseService.Deactivate();
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Releases all resources used by the EPubViewer.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Other methods

        /// <summary>
        /// Deletes book from cache and files from isolated storage.
        /// </summary>
        /// <param name="identifier">Identifier value taken from Book.Metadata.Identifier.Value</param>
        public virtual void DeleteBook(string identifier)
        {
            Book b = DatabaseService.Database.Load<Book>(identifier);
            if (b != null)
            {
                // purge images directory
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    isf.PurgeDirectory(b.GetImagesDirectory());
                }

                // delete from database
                DatabaseService.Database.Delete<Book>(b);
                DatabaseService.Database.Flush();
            }
        }

        #endregion
    }
}
