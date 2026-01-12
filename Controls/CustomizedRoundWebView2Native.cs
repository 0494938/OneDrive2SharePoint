using BaseUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;

namespace GcjUiCtrl.Control
{
    public partial class CustomizedRoundWebView2Native : Microsoft.Web.WebView2.Wpf.WebView2
    {
        // 获取 WebView2 的窗口句柄
        private IntPtr GetWindowHandle(Microsoft.Web.WebView2.Wpf.WebView2 webView2)
        {
            var hwndSource = (HwndSource)PresentationSource.FromVisual(webView2);
            return hwndSource?.Handle ?? IntPtr.Zero;
        }

      
        // 枚举所有子窗口
        private void EnumerateChildWindows(IntPtr hwnd)
        {
            // 获取当前 WPF 窗口的句柄
            //IntPtr hwnd = new WindowInteropHelper(this).Handle;
            //IntPtr hwnd = GetWindowHandle(this);

            // 列表来保存子窗口句柄
            List<IntPtr> childWindows = new List<IntPtr>();

            // 使用回调函数枚举子窗口
            GcjWinApi.EnumChildWindows(hwnd, (childHwnd, lParam) =>
            {
                // 这里可以根据需求处理每个子窗口的句柄
                childWindows.Add(childHwnd);
                Debug.WriteLine("Found child window: " + childHwnd);
                return true; // 返回 true 继续枚举
            }, IntPtr.Zero);

            // 输出所有子窗口
            foreach (var child in childWindows)
            {
                Debug.WriteLine($"Child window handle: {child}");
            }
        }

        private void EnumerateAllChildWindows(IntPtr hwnd, int nWidth, int nHeight)
        {
            // 列表来保存子窗口句柄
            List<IntPtr> childWindows = new List<IntPtr>();

            // 使用回调函数枚举子窗口
            GcjWinApi.EnumChildWindows(hwnd, (childHwnd, lParam) =>
            {
                // 这里可以根据需求处理每个子窗口的句柄
                childWindows.Add(childHwnd);
                Debug.WriteLine("Found child window: " + childHwnd);
                ApplyRoundCornerRegn(childHwnd, nWidth, nHeight);
                EnumerateAllChildWindows(childHwnd, nWidth, nHeight);
                return true; // 返回 true 继续枚举
            }, IntPtr.Zero);

            // 输出所有子窗口
            foreach (var child in childWindows)
            {
                Debug.WriteLine($"Child window handle: {child}");
            }
        }

        private void ForceInvalidateAndUpdateWindow()
        {
            //IntPtr hwnd = new WindowInteropHelper(this).Handle;
            IntPtr hwnd = GetWindowHandle(this);

            // 标记整个窗口的区域为无效
            GcjWinApi.InvalidateRect(hwnd, IntPtr.Zero, true);

            // 强制更新窗口
            GcjWinApi.UpdateWindow(hwnd);
        }
        public CustomizedRoundWebView2Native() : base()
        {
            this.Loaded += CustomizedRoundWebView2_Loaded;
            this.SizeChanged += CustomizedRoundWebView2Native_SizeChanged;
            LayoutUpdated += CustomizedRoundWebView2Native_LayoutUpdated;

        }

        private void CustomizedRoundWebView2Native_LayoutUpdated(object? sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void CustomizedRoundWebView2Native_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            IntPtr hwnd = GetWindowHandle(this);
            if (hwnd != IntPtr.Zero)
                ApplyRoundCornerRegn(hwnd, (int)e.NewSize.Width, (int)e.NewSize.Height);
        }

        private void CustomizedRoundWebView2_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hwnd = GetWindowHandle(this);
            if (hwnd != IntPtr.Zero)
                ApplyRoundCornerRegn(hwnd, (int)this.Width, (int)this.Height);
        }

        private void ApplyRoundCornerRegn(IntPtr hwnd, int nWidth, int nHeight) {
#if false
            // 创建圆角区域并应用
            //IntPtr hRgn = CreateRoundRectRgn(0, 0, (int)this.ActualWidth, (int)this.ActualHeight, (int)CornerRadius.TopLeft, (int)CornerRadius.BottomRight);
            IntPtr hRgn = CreateRoundRectRgn(0, 0, nWidth, nHeight, (int)CornerRadius.TopLeft, (int)CornerRadius.BottomRight);

            SetWindowRgn(hwnd, hRgn, true);
            DeleteObject(hRgn);
//#else
            int cornerPreference = GcjWinApi.DWMWCP_ROUND; // 设置圆角样式
            int nRet = GcjWinApi.DwmSetWindowAttribute(hwnd, GcjWinApi.DWMWA_WINDOW_CORNER_PREFERENCE, ref cornerPreference, sizeof(int));
#endif
            EnumerateAllChildWindows(hwnd, nWidth, nHeight);
            ForceInvalidateAndUpdateWindow();
        }
        // 强制更新窗口
        private void ForceUpdateWindow()
        {
            //IntPtr hwnd = new WindowInteropHelper(this).Handle;
            IntPtr hwnd = GetWindowHandle(this);
            GcjWinApi.UpdateWindow(hwnd);
        }

        #region GcjVer // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedRoundWebView2Native),
            new PropertyMetadata("Gcj Grid based Round WebView v1.0"));

        [Description("Gets or sets the CustomizedRoundWebView2Native Version")]
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
            typeof(CustomizedRoundWebView2Native),
            new PropertyMetadata(new CornerRadius(5.0)));

        [Description("Gets or sets the CustomizedRoundWebView2Native CornerRadius")]
        [Category("GcjControl")]
        public System.Windows.CornerRadius CornerRadius
        {
            get { return (System.Windows.CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion CornerRadius

    }
}
