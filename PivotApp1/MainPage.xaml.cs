using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PivotApp1.Resources;
using System.Diagnostics;

namespace PivotApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            LoadEpub();
        }

        private void LoadEpub()
        {
            //EPubViewer.Source = Application.GetResourceStream(new Uri("/[your app name];component/[path to epub file]", UriKind.Relative)).Stream;
            try
            {
                //EPubViewer.Source = Application.GetResourceStream(new Uri("https://epub-samples.googlecode.com/files/mymedia_lite-20130621.epub", UriKind.RelativeOrAbsolute)).Stream;
                
                WebClient client = new WebClient();
                client.OpenReadCompleted += (s, e) =>
                {
                    byte[] imageBytes = new byte[e.Result.Length];
                    e.Result.Read(imageBytes, 0, imageBytes.Length);

                    // Now you can use the returned stream to set the image source too
                    EPubViewer.Source = e.Result;
                };
                client.OpenReadAsync(new Uri("https://epub-samples.googlecode.com/files/moby-dick-20120118.epub", UriKind.RelativeOrAbsolute));
                //Uri temp = new Uri("https://epub-samples.googlecode.com/files/mymedia_lite-20130621.epub", UriKind.RelativeOrAbsolute);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }

        private void EPubViewer_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {           
            isMenuShowing = !isMenuShowing;
            Menu.Visibility = (isMenuShowing ? Visibility.Visible : Visibility.Collapsed);
        }
        bool isMenuShowing = false;
        private void ShowTocBtn_Click(object sender, RoutedEventArgs e)
        {
            EPubViewer.State = EPubReader.State.Toc;
        }

        private void ShowCover_Click(object sender, RoutedEventArgs e)
        {
            EPubViewer.State = EPubReader.State.Cover;
        }

        private void Reader_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}