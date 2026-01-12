using System.ComponentModel;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedTabWebCtrl : System.Windows.Controls.ContentControl
    {
        #region Properties


        #region WebViewCtrl         // 定义 WebView2 依赖属性
        public static readonly DependencyProperty _WebView2 = DependencyProperty.Register(
            "WebViewCtrl",
            typeof(CustomizedWebView2),
            typeof(CustomizedTabWebCtrl),
            new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedTabWebCtrl WebView2 Control")]
        [Category("GcjControl")]
        public CustomizedWebView2 WebViewCtrl
        {
            //get { return (CustomizedWebView2)GetValue(_WebView2); }
            get {
                if (_tabControl != null && _tabControl.SelectedContent is CustomizedAddressedWebView2)
                    return ((CustomizedAddressedWebView2)_tabControl.SelectedContent).WebViewCtrl;
                else 
                    return (CustomizedWebView2)null; }
        }
        #endregion WebViewCtrl
        
        #region IsMuted         // 定义 IsMuted 依赖属性
        //Mapping to webBrowser.CoreWebView2.IsMuted
        public static readonly DependencyProperty _IsMuted = DependencyProperty.Register(
            "IsMuted",
            typeof(bool),
            typeof(CustomizedTabWebCtrl),
            new PropertyMetadata(true));

        [Description("Gets or sets the CustomizedTabWebCtrl Version")]
        [Category("GcjControl")]
        public bool IsMuted
        {
            get
            {
                //if (webBrowser?.CoreWebView2 != null)
                //    return webBrowser?.CoreWebView2.IsMuted ?? true;
                return (bool)GetValue(_IsMuted);
            }
            set
            {
                SetValue(_IsMuted, value);
                //if (webBrowser != null && webBrowser?.CoreWebView2 != null)
                //    webBrowser.CoreWebView2.IsMuted = value;
            }
        }
        #endregion IsMuted

        #region ProhibitJNewWindow         // 定义 ProhibitJNewWindow 依赖属性
        public static readonly DependencyProperty _ProhibitJNewWindow = DependencyProperty.Register(
            "ProhibitJNewWindow",
            typeof(bool),
            typeof(CustomizedTabWebCtrl),
            new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedTabWebCtrl Version")]
        [Category("GcjControl")]
        public bool ProhibitJNewWindow
        {
            get
            {
                //if (webBrowser?.CoreWebView2 != null)
                //    return webBrowser?.CoreWebView2.ProhibitJNewWindow ?? true;
                return (bool)GetValue(_ProhibitJNewWindow);
            }
            set
            {
                SetValue(_ProhibitJNewWindow, value);
                //if (webBrowser != null && webBrowser?.CoreWebView2 != null)
                //    webBrowser.CoreWebView2.ProhibitJNewWindow = value;
            }
        }
        #endregion ProhibitJNewWindow


        #region IsChromeStyled         // 定义 IsChromeStyled 依赖属性
        public static readonly DependencyProperty _IsChromeStyled = DependencyProperty.Register(
            "IsChromeStyled",
            typeof(bool),
            typeof(CustomizedTabWebCtrl),
            new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedTabWebCtrl IsChromeStyled")]
        [Category("GcjControl")]
        public bool IsChromeStyled
        {
            get
            {
                return (bool)GetValue(_IsChromeStyled);
            }
            set
            {
                SetValue(_IsChromeStyled, value);
            }
        }
        #endregion IsChromeStyled

        #region Items         // 定义 Items 依赖属性
        //Mapping to TabControl.SourceItems
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items",
            typeof(List<WebVWTabItem>),
            typeof(CustomizedTabWebCtrl),
            new PropertyMetadata(new List<WebVWTabItem>()));

        [Description("Gets or sets the CustomizedTabWebCtrl Version")]
        [Category("GcjControl")]
        public List<WebVWTabItem> Items
        {
            get
            {
                //if (webBrowser?.CoreWebView2 != null)
                //    return webBrowser?.CoreWebView2.Items ?? true;
                //StackPanel;
                return (List<WebVWTabItem>)GetValue(ItemsProperty);
            }
            set
            {
                SetValue(ItemsProperty, value);
                //if (webBrowser != null && webBrowser?.CoreWebView2 != null)
                //    webBrowser.CoreWebView2.Items = value;
            }
        }
        #endregion Items

        #region SelectedContent         // 定义 SelectedContent 依赖属性
        //Mapping to TabControl.SelectedContent
        public static readonly DependencyProperty SelectedContentProperty = DependencyProperty.Register(
            "SelectedContent",
            typeof(object),
            typeof(CustomizedTabWebCtrl),
            new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedTabWebCtrl SelectedContent")]
        [Category("GcjControl")]
        public object SelectedContent
        {
            get
            {
                if (_tabControl != null) return _tabControl.SelectedContent;
                else
                    return (object)GetValue(SelectedContentProperty);
            }
        }
        #endregion SelectedContent

        #endregion Properties

    }
}
