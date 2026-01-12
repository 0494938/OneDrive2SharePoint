using System.ComponentModel;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedAddressedWebView2 : System.Windows.Controls.ContentControl
    {
        #region Properties

        #region WebViewCtrl         // 定义 WebView2 依赖属性
        public static readonly DependencyProperty _WebView2 = DependencyProperty.Register(
            "WebViewCtrl",
            typeof(CustomizedWebView2),
            typeof(CustomizedAddressedWebView2),
            new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedAddressedWebView2 WebView2 Control")]
        [Category("GcjControl")]
        public CustomizedWebView2 WebViewCtrl
        {
            //get { return (CustomizedWebView2)GetValue(_WebView2); }
            get { return webVW2; }
        }
        #endregion WebViewCtrl

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedAddressedWebView2),
            new PropertyMetadata("Gcj AddressedWebView2 v1.0"));

        [Description("Gets or sets the CustomizedAddressedWebView2 Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region IsMuted         // 定义 IsMuted 依赖属性
        //Mapping to webBrowser.CoreWebView2.IsMuted
        public static readonly DependencyProperty _IsMuted = DependencyProperty.Register(
            "IsMuted",
            typeof(bool),
            typeof(CustomizedAddressedWebView2),
            new PropertyMetadata(true));

        [Description("Gets or sets the CustomizedAddressedWebView2 Version")]
        [Category("GcjControl")]
        public bool IsMuted
        {
            get
            {
                if (webVW2?.CoreWebView2 != null)
                    return webVW2?.CoreWebView2.IsMuted ?? true;
                return (bool)GetValue(_IsMuted);
            }
            set
            {
                SetValue(_IsMuted, value);
                if (webVW2 != null && webVW2?.CoreWebView2 != null)
                    webVW2.CoreWebView2.IsMuted = value;
            }
        }
        #endregion IsMuted

        #region NeedInitiaze         // 定义 NeedInitiaze 依赖属性
        public static readonly DependencyProperty _NeedInitiazeProperty = DependencyProperty.Register(
            "NeedInitiaze",
            typeof(bool),
            typeof(CustomizedAddressedWebView2),
            new PropertyMetadata(true));

        [Description("Gets or sets the CustomizedAddressedWebView2 Version")]
        [Category("GcjControl")]
        public bool NeedInitiaze
        {
            get
            {
                return (bool)GetValue(_NeedInitiazeProperty);
            }
            set
            {
                SetValue(_NeedInitiazeProperty, value);
            }
        }
        #endregion NeedInitiaze

        #endregion Properties
    }
}
