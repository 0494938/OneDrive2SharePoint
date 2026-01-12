using BaseUtil;
using GcjUiCtrl.Control;
using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace IsidTestApp
{

    /// <summary>
    /// TestControlWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class IsidTestControlWindowBak1 : GcjBaseWindow
    {
        private void OnBackGroundColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            TestContext? datacontext = this.DataContext as TestContext;
            if (datacontext != null && e.NewValue != null)
            {
                datacontext.CustomizedBgColor = e.NewValue ?? (Colors.BlueViolet);
                RgnBorder.GetBindingExpression(Border.BackgroundProperty)?.UpdateTarget();
            }
        }


        private void StartStopAnimation(object sender, RoutedEventArgs e)
        {
            btnAnimationButton.StartAnimation = !btnAnimationButton.StartAnimation;
        }

        public IsidTestControlWindowBak1()
        {
            InitializeComponent();
            InitBrowser();
            Loaded += IsidTestControlWindowBak1_Loaded;
            Unloaded += IsidTestControlWindowBak1_Unloaded;
            // 初始化TextBox内容
            for (int j = 0; j < 100; j++)
            {
                AutoScrollTextBox.AppendText($"This is line {j + 1} with a long content to test horizontal scrolling. \n");
            }
        }

        CustomizedNativeWndCtrlHost? nativeCtrl = null;
        private void IsidTestControlWindowBak1_Unloaded(object sender, RoutedEventArgs e)
        {
            nativeCtrl?.Dispose();
        }

        private void IsidTestControlWindowBak1_Loaded(object sender, RoutedEventArgs e)
        {
            nativeCtrl = new CustomizedNativeWndCtrlHost(xNativeWebViewContainer, (HwndHost)webBrowser);
        }

        public async void InitBrowser()
        {
            TestContext? datacontext = this.DataContext as TestContext;
            string sFileSavePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string sFileTempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "temp\\";
            Microsoft.Win32.RegistryKey? registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(CtrlUtil.REGISTRY_PATH, false);
            if (registryKey != null)
            {
                sFileSavePath = (registryKey?.GetValue("FileSavePath") as string) ?? sFileSavePath;
                sFileTempPath = (registryKey?.GetValue("FileTempPath") as string) ?? sFileTempPath;
                registryKey.Close();
            }

            var env = await CoreWebView2Environment.CreateAsync(null, string.IsNullOrEmpty(sFileSavePath) ? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) : sFileSavePath);
            await webBrowser.EnsureCoreWebView2Async(env);

            webBrowser.NavigationCompleted += WebBrowser_NavigationCompleted;
            webBrowser.Loaded += WebBrowser_Loaded;
            webBrowser.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            webBrowser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;

            await webBrowser.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
            await webBrowser.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");

            webBrowser.CoreWebView2.Navigate("https://www.google.com/");
        }

        private void CoreWebView2_NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}:IsidTestControlWindowBak1::CoreWebView2_NewWindowRequested Triggered");

            e.NewWindow = (sender as CoreWebView2);
        }

        private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
        }

        private void WebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void WebBrowser_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
        }

        private void TextScrollView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}:TextScrollView_SizeChanged Triggered");
        }

        
        private void TextPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}:TextPanel_SizeChanged Triggered");
        }

        private void CustomizedImageButton_Click(object sender, RoutedEventArgs e)
        {
            TestContext? datacontext = this.DataContext as TestContext;
            if ((datacontext != null))
            {
                LoadUiUrl(datacontext, txtUrl.Text);
            }
        }

        public void LoadUiUrl(TestContext? datacontext, string strURL)
        {
            if (datacontext != null)
            {
                if (!strURL.StartsWith("http://") && !strURL.StartsWith("https://"))
                {
                    strURL = "https://" + strURL;
                }
                webBrowser.CoreWebView2.Navigate(strURL);
            }
        }

        private void grdLeft_LayoutUpdated(object sender, EventArgs e)
        {
            Debug.WriteLine ($"grdLeft_LayoutUpdated: (ActualWidth:{grdLeft.ActualWidth}, ActualHeight:{grdLeft.ActualHeight})");
            //webBrowser.GetBindingExpression(HwndHost.MarginProperty)?.UpdateTarget();
        }

        private void grdWebBrowser_LayoutUpdated(object sender, EventArgs e)
        {
            Debug.WriteLine($"grdWebBrowser_LayoutUpdated: (ActualWidth:{grdWebBrowser.ActualWidth}, ActualHeight:{grdWebBrowser.ActualHeight})");
            //webBrowser.GetBindingExpression(HwndHost.MarginProperty)?.UpdateTarget();
        }

        private void gridTitle_LayoutUpdated(object sender, EventArgs e)
        {
            Debug.WriteLine($"gridTitle_LayoutUpdated: (ActualWidth:{gridTitle.ActualWidth}, ActualHeight:{gridTitle.ActualHeight})");
            //webBrowser.GetBindingExpression(HwndHost.MarginProperty)?.UpdateTarget();
        }

        private void GcjBaseWindow_LayoutUpdated(object sender, EventArgs e)
        {
            Debug.WriteLine($"GcjBaseWindow_LayoutUpdated: (ActualWidth:{grdLeft.ActualWidth}, ActualHeight:{grdLeft.ActualHeight})");
        }

        private void grdWebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"grdWebBrowser_Loaded: (ActualWidth:{grdWebBrowser.ActualWidth}, ActualHeight:{grdWebBrowser.ActualHeight})");
        }

        private void grdLeft_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"grdLeft_Loaded: (ActualWidth:{grdLeft.ActualWidth}, ActualHeight:{grdLeft.ActualHeight})");
        }

        private void gridTitle_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"gridTitle_Loaded: (ActualWidth:{gridTitle.ActualWidth}, ActualHeight:{gridTitle.ActualHeight})");
        }

        private void GcjBaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"GcjBaseWindow_Loaded: (ActualWidth:{this.ActualWidth}, ActualHeight:{this.ActualHeight})");
        }

        private void grdLeft_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine($"grdLeft_SizeChanged: (ActualWidth:{grdLeft.ActualWidth}, ActualHeight:{grdLeft.ActualHeight})");
            webBrowser.GetBindingExpression(HwndHost.MarginProperty)?.UpdateTarget();
        }

        private void gridTitle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine($"gridTitle_SizeChanged: (ActualWidth:{gridTitle.ActualWidth}, ActualHeight:{gridTitle.ActualHeight})");
            webBrowser.GetBindingExpression(HwndHost.MarginProperty)?.UpdateTarget();
        }
        
        private void grdWebBrowser_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine($"grdWebBrowser_SizeChanged: (ActualWidth:{grdWebBrowser.ActualWidth}, ActualHeight:{grdWebBrowser.ActualHeight})");
            webBrowser.GetBindingExpression(HwndHost.MarginProperty)?.UpdateTarget();
        }
    }
}
