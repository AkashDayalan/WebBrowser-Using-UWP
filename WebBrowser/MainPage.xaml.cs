using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;
using static Windows.Web.Http.HttpClient;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WebBrowser
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int settingPageCount = 0;
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
            //this.Frame.Navigate(typeof(SettingsPage));
            if (settingPageCount == 0)
            {
                AddSettingsTab();
                settingPageCount++;
            }
        }

        private void AddSettingsTab()
        {
            var settingsTab = new muxc.TabViewItem();

            settingsTab.Header = "Settings";
            settingsTab.Name = "Settings";
            settingsTab.IconSource = new muxc.SymbolIconSource() { Symbol = Symbol.Setting};

            Frame frame = new Frame();
            settingsTab.Content = frame;
            frame.Navigate(typeof(SettingsPage));

            TabControl.TabItems.Add(settingsTab);
            TabControl.SelectedItem = settingsTab;

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
            browserProgress.IsActive = false;
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
            bool isSSL;
            if(webBrowser.Source.AbsoluteUri.Contains("https"))
            {
                isSSL = true;
                //Change icon image
                sslIcon.FontFamily = new FontFamily("Segoe MDL2 Assets");
                sslIcon.Glyph = "\xE72E";
                ToolTip toolTip = new ToolTip();
                toolTip.Content = "This website has a SSL certificate";
                ToolTipService.SetToolTip(sslButton, toolTip);
            }
            else
            {
                isSSL = false;
                //Change icon image
                sslIcon.FontFamily = new FontFamily("Segoe MDL2 Assets");
                sslIcon.Glyph = "\xE785";
                ToolTip toolTip = new ToolTip();
                toolTip.Content = "This website is unsafe and doesn't have a SSL certificate";
                ToolTipService.SetToolTip(sslButton, toolTip);
            }
        }

        private void webBrowser_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            browserProgress.IsActive = true;
            
        }

        private void TabControl_AddTabButtonClick(muxc.TabView sender, Object args)
        {
            var newTab = new muxc.TabViewItem();

            newTab.IconSource = new muxc.SymbolIconSource() { Symbol = Symbol.Document };
            newTab.Header = "Blank Page";

            WebView webView = new WebView();

            newTab.Content = webView;

            webView.Navigate(new Uri("https://www.google.co.in"));
            sender.TabItems.Add(newTab);
            sender.SelectedItem = newTab;

        }

        private void TabControl_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
            if(args.Tab.Name == "Settings")
            {
                settingPageCount = 0;
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            //Search
            Search();
        }
    }
}