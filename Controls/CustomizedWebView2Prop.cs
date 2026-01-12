using System.ComponentModel;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedWebView2 : Microsoft.Web.WebView2.Wpf.WebView2
    {
        #region Properties

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedWebView2),
            new PropertyMetadata("Gcj WebView2 v1.0"));


        [Description("Gets or sets the CustomizedWebView2 Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region NeedInitiaze         // 定义 NeedInitiaze 依赖属性
        public static readonly DependencyProperty _NeedInitiazeProperty = DependencyProperty.Register(
            "NeedInitiaze",
            typeof(bool),
            typeof(CustomizedWebView2),
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
