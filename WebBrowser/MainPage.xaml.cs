using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WebBrowser
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            DataAccess dataAccess = new DataAccess();
            dataAccess.CreateSettingsFile();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            if (webBrowser.CanGoBack)
            {
                webBrowser.GoBack();
            }
        }

        private void frdBtn_Click(object sender, RoutedEventArgs e)
        {
            if (webBrowser.CanGoForward)
            {
                webBrowser.GoForward();
            }
        }

        private void SearchBar_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Search();
            }
        }

        private void Search()
        {
            webBrowser.Source = new Uri("https://www.google.com/search?q=" + SearchBar.Text);
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.Refresh();
        }

        private void homeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void settingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }

        private void MainBrowserWindow_Loading(FrameworkElement sender, object args)
        {

        }

        private void webBrowser_Loading(FrameworkElement sender, object args)
        {
            statusText.Text = webBrowser.Source.AbsoluteUri;
        }

        private void webBrowser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            browserProgress.IsEnabled = false;
            browserProgress.IsIndeterminate = false;
            try
            {
                statusText.Text = webBrowser.Source.AbsoluteUri;
                AppTitle.Text = "OneWebBrowser" + "  " + webBrowser.DocumentTitle;

                DataTransfer dataTransfer = new DataTransfer();
                if (!string.IsNullOrEmpty(SearchBar.Text))
                {
                    dataTransfer.SaveSearchTerm(SearchBar.Text, webBrowser.DocumentTitle, webBrowser.Source.AbsoluteUri);
                }
            }
            catch
            {

            }
        }

        private void webBrowser_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            browserProgress.IsEnabled = true;
            browserProgress.IsIndeterminate = true;
        }
    }
}