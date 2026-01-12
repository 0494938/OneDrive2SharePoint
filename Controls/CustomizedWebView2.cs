using Microsoft.Web.WebView2.Core;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedWebView2 : Microsoft.Web.WebView2.Wpf.WebView2
    {
        public CustomizedWebView2() : base()
        {
            //DefaultStyleKey = typeof(CustomizedWebView2);
            Loaded += CustomizedWebView2_Loaded;
            if(!IsInDesignMode())
                InitBrowser();
            else
                this.Source = new Uri("https://www.youtube.com/watch?v=-yG5cYyDAT0");
        }

        private bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(this);
        }

        public async void InitBrowser()
        {
            if (NeedInitiaze)
            {
                string sFileSavePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string sFileTempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "temp\\";
                Microsoft.Win32.RegistryKey? registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\zdhe\\batchdownload\\1.0", false);
                if (registryKey != null)
                {
                    sFileSavePath = (registryKey?.GetValue("FileSavePath") as string) ?? sFileSavePath;
                    sFileTempPath = (registryKey?.GetValue("FileTempPath") as string) ?? sFileTempPath;
                    registryKey?.Close();
                }

                var env = await CoreWebView2Environment.CreateAsync(null, string.IsNullOrEmpty(sFileSavePath) ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) : sFileSavePath);
                await this.EnsureCoreWebView2Async(env);

                this.NavigationCompleted += WebBrowser_NavigationCompleted;
                this.CoreWebView2.NavigationCompleted += WebBrowser_NavigationCompleted;

                this.Loaded += WebBrowser_Loaded;
                this.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;

                this.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
                this.CoreWebView2.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
                this.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
                this.CoreWebView2.FrameNavigationStarting += CoreWebView2_FrameNavigationStarting;
                this.CoreWebView2.FrameNavigationCompleted += CoreWebView2_FrameNavigationCompleted;
                this.CoreWebView2.LaunchingExternalUriScheme += CoreWebView2_LaunchingExternalUriScheme;
                this.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
                this.CoreWebView2InitializationCompleted += CustomizedWebView2_CoreWebView2InitializationCompleted1;
                this.CoreWebView2.NotificationReceived += CoreWebView2_NotificationReceived;
                this.CoreWebView2.SourceChanged += CoreWebView2_SourceChanged;
                this.CoreWebView2.StatusBarTextChanged += CoreWebView2_StatusBarTextChanged;

                await this.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
                await this.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");

                this.CoreWebView2.Navigate("https://www.youtube.com/watch?v=-yG5cYyDAT0");
            }
        }

        private void CoreWebView2_StatusBarTextChanged(object? sender, object e)
        {
        }

        private void CoreWebView2_SourceChanged(object? sender, CoreWebView2SourceChangedEventArgs e)
        {
        }

        private void CoreWebView2_NotificationReceived(object? sender, CoreWebView2NotificationReceivedEventArgs e)
        {
        }

        private void CustomizedWebView2_CoreWebView2InitializationCompleted1(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
        }

        private void CoreWebView2_DOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
        }

        private void CoreWebView2_LaunchingExternalUriScheme(object? sender, CoreWebView2LaunchingExternalUriSchemeEventArgs e)
        {
        }

        private void CustomizedWebView2_CoreWebView2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
        }

        private void CoreWebView2_FrameNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
        }

        private void CoreWebView2_FrameNavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
        }

        private void CoreWebView2_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
        }

        private void WebBrowser_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
        }

        private void CoreWebView2_DocumentTitleChanged(object? sender, object e)
        {
            RaiseOnTitleChanged(sender, e);
        }

        private void CoreWebView2_NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}:IsidTestControlWindow::CoreWebView2_NewWindowRequested Triggered");
            DependencyObject? parent = VisualTreeHelper.GetParent(this) ;
            CustomizedAddressedWebView2? addrVW = null;
            if (parent is CustomizedAddressedWebView2)
                addrVW = (CustomizedAddressedWebView2)parent;
            else {
                parent = VisualTreeHelper.GetParent(parent);
                if (parent is CustomizedAddressedWebView2) {
                    addrVW = (CustomizedAddressedWebView2)parent;

                    //parent = addrVW.Parent;
                    WebVWTabItem? tabItem = addrVW.Parent as WebVWTabItem;
                    if (tabItem != null) {
                        parent = VisualTreeHelper.GetParent(tabItem);
                        if (parent is TabPanel tabPanel)
                        {
                            parent = VisualTreeHelper.GetParent(parent);
                            if (parent is Grid)
                            {
                                parent = VisualTreeHelper.GetParent(parent);
                                System.Windows.Controls.TabControl? tabControl = parent as System.Windows.Controls.TabControl;
                                parent = VisualTreeHelper.GetParent(tabControl);
                                if (parent is Grid grid) {
                                    CustomizedTabWebCtrl? tabWeb = VisualTreeHelper.GetParent(grid) as CustomizedTabWebCtrl;
                                    if(tabWeb!= null)
                                    {
                                        //int nNewIdx = tabControl.SelectedIndex + 1;
                                        //tabWeb.AddTabItem("Loading", e.Uri.ToString(), nNewIdx);
                                        //e.NewWindow = (tabControl.Items[nNewIdx] as CustomizedAddressedWebView2)?.WebViewCtrl?.CoreWebView2;
                                        //tabWeb.UpdateLayout();
                                        //return;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Debug.Assert(false);
                }
            }

            e.NewWindow = this.CoreWebView2;
        }

        private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
        }

        private void WebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
        }


        private void CustomizedWebView2_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}:CustomizedWebView2_Loaded Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");
            //Debug.WriteLine("$$$$ CustomizedWebView2_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            //Debug.WriteLine("  Width=" + this.Width + ", Height=" + this.Height + ", ActualWidth=" + this.ActualWidth + ", ActualHeight=" + this.ActualHeight);
        }
    }
}
