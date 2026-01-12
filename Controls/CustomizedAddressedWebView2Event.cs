using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedAddressedWebView2 : System.Windows.Controls.ContentControl
    {
        #region OnTitleChanged Event

        // 注册一个路由事件
        public static readonly RoutedEvent OnTitleChangedEvent = EventManager.RegisterRoutedEvent(
            "OnTitleChanged", // 事件名称
            RoutingStrategy.Bubble, // 路由策略：冒泡、隧道、直接
            typeof(GcjWebView2EventHandler), // 事件处理程序的类型
            typeof(CustomizedAddressedWebView2)); // 注册事件的类

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("AddressedWebView2 Control DocumentTitleChanged Event Handler")]
        [Category("GcjControl")]
        public event GcjWebView2EventHandler OnTitleChanged
        {
            add { AddHandler(OnTitleChangedEvent, value); }
            remove { RemoveHandler(OnTitleChangedEvent, value); }
        }
        protected void RaiseOnTitleChanged(object? sender, object e)
        {
            GcjRoutedEventArgs newArgs = new GcjRoutedEventArgs()
            {
                RoutedEvent = OnTitleChangedEvent,
                Source = sender,
                RntEvtArg = e,
            };
            RaiseEvent(newArgs);
        }

        #endregion OnTitleChanged Event    

    }
}
