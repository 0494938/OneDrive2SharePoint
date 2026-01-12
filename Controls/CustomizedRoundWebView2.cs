using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace GcjUiCtrl.Control
{
    public partial class CustomizedRoundWebView2 : System.Windows.Controls.ContentControl
    {
        private System.Windows.Controls.Border _webView2Border;
        private WebView2 _webView2;
        public WebView2 WebView2 { get {
                //return (WebView2)this.Template.FindName("PART_WebView2", this); 
                return _webView2; 
            }
        }
        public CoreWebView2? CoreWebView2{get{return WebView2?.CoreWebView2;} }
        public CustomizedRoundWebView2() : base()
        {
            _webView2Border = new System.Windows.Controls.Border();
            _webView2Border.Margin = new Thickness(10, 10, 10, 10); // 设置 Margin
            _webView2Border.CornerRadius = new CornerRadius(10,10,10,10);
            _webView2 = new WebView2();
            _webView2.Margin = new Thickness(-10, -10, -10, -10); // 设置 Margin
            _webView2.Name = "abcd";
            this.Loaded += CustomizedRoundWebView2_Loaded;
        }

        private void CustomizedRoundWebView2_Loaded(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            // 将 TextBox 添加到控件的视觉树中
            //AddVisualChild(_webView2);
            AddVisualChild(_webView2Border);
            _webView2Border.Child = _webView2;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _webView2Border; // 返回子控件
        }

        protected override int VisualChildrenCount => 1; // 表示有一个子控件

        #region GcjVer // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedRoundWebView2),
            new PropertyMetadata("Gcj Grid based Round WebView v1.0"));

        [Description("Gets or sets the CustomizedRoundWebView2 Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer // 定义 GcjVer 依赖属性

        #region CornerRadius         // 定义 CornerRadius 依赖属性
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius",
            typeof(System.Windows.CornerRadius),
            typeof(CustomizedRoundWebView2),
            new PropertyMetadata(new CornerRadius(5.0)));

        [Description("Gets or sets the CustomizedRoundWebView2 CornerRadius")]
        [Category("GcjControl")]
        public System.Windows.CornerRadius CornerRadius
        {
            get { return (System.Windows.CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion CornerRadius

    }
}
