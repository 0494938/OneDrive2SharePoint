using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedAddressedWebView2 : System.Windows.Controls.ContentControl
    {
        CustomizedWebView2? webVW2 = null;
        CustomizedTextBox? txtUrl = null;
        CustomizedImageButton? btnNav = null;

        public CustomizedAddressedWebView2() : base()
        {
            //DefaultStyleKey = typeof(CustomizedAddressedWebView2);
            //Initialized += CustomizedAddressedWebView2_Initialized;
            Loaded += CustomizedAddressedWebView2_Loaded;
        }

        private bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(this);
        }

        private void CustomizedAddressedWebView2_Initialized(object? sender, EventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}: CustomizedAddressedWebView2_Initialized Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");
            if (webVW2 == null)
                webVW2 = this.Template.FindName("webBrowser", this) as CustomizedWebView2;
            if (txtUrl == null)
                txtUrl = this.Template.FindName("txtUrl", this) as CustomizedTextBox; 
            if (btnNav == null)
                btnNav = this.Template.FindName("btnNav", this) as CustomizedImageButton;
        }

        private void CustomizedAddressedWebView2_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}: CustomizedAddressedWebView2_Loaded Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");
            if (webVW2 == null)
                webVW2 = this.Template.FindName("webBrowser", this) as CustomizedWebView2;
            if (txtUrl == null)
                txtUrl = this.Template.FindName("txtUrl", this) as CustomizedTextBox;
            if (btnNav == null)
                btnNav = this.Template.FindName("btnNav", this) as CustomizedImageButton;

            if(btnNav != null)
                btnNav.Click += BtnNav_Click;

            if (txtUrl != null)
                txtUrl.PreviewKeyDown += TxtUrl_PreviewKeyDown;
            if (webVW2!=null) {
                webVW2.OnTitleChanged += WebBrowser_OnTitleChanged;
            }
            //Debug.WriteLine("$$$$ CustomizedAddressedWebView2_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            //Debug.WriteLine("  Width=" + this.Width + ", Height=" + this.Height + ", ActualWidth=" + this.ActualWidth + ", ActualHeight=" + this.ActualHeight);
        }

        private void WebBrowser_OnTitleChanged(object sender, GcjRoutedEventArgs e)
        {
            RaiseOnTitleChanged(sender, e);
        }

        private void TxtUrl_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Debug.WriteLine("txtInitURL_PreviewKeyDown, SystemKey: " + e.SystemKey + ", Key:" + e.Key);
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (btnNav.IsEnabled)
                    btnNav.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void BtnNav_Click(object sender, RoutedEventArgs e)
        {
            LoadUiUrl(txtUrl?.Text);
        }

        public void LoadUiUrl(string? strURL)
        {
            if (!string.IsNullOrWhiteSpace(strURL) && strURL.Trim().Length > 5)
            {
                if (!strURL.StartsWith("http://") && !strURL.StartsWith("https://"))
                {
                    strURL = "https://" + strURL;
                }
                webVW2?.CoreWebView2?.Navigate(strURL);
            }
        }
    }
}
