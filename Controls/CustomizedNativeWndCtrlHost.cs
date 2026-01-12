using BaseUtil;
using GcjUtil;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using static BaseUtil.GcjWinApi;

namespace GcjUiCtrl.Control
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)] public delegate ushort DelegateRegisterContainerCls(IntPtr hInstannce, string sWndClsName, IntPtr lpfnWndProc);

    public partial class CustomizedNativeWndCtrlHost : HwndHost
    {
        private IntPtr _hwndNativeCtrl = IntPtr.Zero;
        private IntPtr _hwndNativeCtrlInContainer = IntPtr.Zero;
        private HwndHost? _realHwndHost = null;
        public IntPtr? HWnd { get { return _hwndNativeCtrl; } }
        public IntPtr HWndReal { get { return _hwndNativeCtrlInContainer; } }
        private IntPtr oldWndProc = IntPtr.Zero;
        private WndProcDelegate? newWndProcDelegate = null;

        private const string RICHEDIT_CLASS = "RICHEDIT50W";// "RichEdit20W";
        private const string _sEmptyContainer_CLASS = "EmptyContainer";
        public const string _sCustomizedNative_CLASS = "Microsoft.Web.WebView2.Wpf.WebView2";
        private IntPtr hInstanceRichEdit = 0;
        private WndProcDelegate _wndEmptyContainerProc;

        public WebView2? WebView2
        {
            get { return _realHwndHost as WebView2; }
        }

        #region CreateControl Methods
        public CustomizedNativeWndCtrlHost(CustomizedNativeContainer xCustomizedNativeContainer, HwndHost? realHwndHost=null)
        {
            _nativeContainer = xCustomizedNativeContainer;
            _realHwndHost = realHwndHost;
            _wndEmptyContainerProc = new WndProcDelegate(WndEmptyContainerProc);
            xCustomizedNativeContainer.Children.Add(this);
            xCustomizedNativeContainer.WndCtrlHost = this;
            hInstanceRichEdit = GcjWinApi.LoadLibrary("msftedit.dll");
            if (hInstanceRichEdit == IntPtr.Zero)
                throw new InvalidOperationException("can not load msftedit.dll.");

        }

        private IntPtr CreateNativeEditCtrl(IntPtr hwndParent, int dwStyle)
        {
            // 创建嵌入 Edit的原生窗口
            dwStyle |= WS_CHILD | WS_VISIBLE | ES_CENTER | ES_MULTILINE | ES_AUTOVSCROLL | ES_AUTOHSCROLL | WS_VSCROLL;
            IntPtr hwnd = CreateWindowEx(
                0, "EDIT", "test text",
                dwStyle,
                 0, 0, (int)Width, (int)Height,
                hwndParent,
                 IntPtr.Zero,
                Marshal.GetHINSTANCE(GetType().Module),
                 IntPtr.Zero
            );
            return hwnd;
        }

        private IntPtr CreateNativeStaticCtrl(IntPtr hwndParent, int dwStyle)
        {
            // 创建嵌入 Edit的原生窗口
            dwStyle |= WS_CHILD | WS_VISIBLE | ES_CENTER;
            IntPtr hwnd = CreateWindowEx(
                0, "STATIC", "test static text",
                dwStyle,
                 300, 300, (int)Width, (int)Height,
                hwndParent, IntPtr.Zero, Marshal.GetHINSTANCE(GetType().Module), IntPtr.Zero
            );
            return hwnd;
        }

        private IntPtr CreateNativeRichEditCtrl(IntPtr hwndParent, int dwStyle)
        {
            // 创建嵌入 Edit的原生窗口
            dwStyle |= WS_CHILD | WS_VISIBLE | ES_MULTILINE | WS_VSCROLL | WS_HSCROLL;
            IntPtr hwnd = CreateWindowEx(
                    0, RICHEDIT_CLASS, "",
                    dwStyle,//WS_BORDER | 
                    0, 0, (int)Width, (int)Height,
                    hwndParent, IntPtr.Zero, hInstanceRichEdit, IntPtr.Zero);
            return hwnd;
        }

        private IntPtr CreateNativeContainerCtrl(IntPtr hwndParent, int dwStyle)
        {
            // 创建嵌入 Edit的原生窗口
            var wndClass = new WNDCLASSEX
            {
                cbSize = (uint)Marshal.SizeOf(typeof(WNDCLASSEX)),
                style = 0,
                lpfnWndProc = Marshal.GetFunctionPointerForDelegate(_wndEmptyContainerProc),
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = Marshal.GetHINSTANCE(typeof(CustomizedNativeWndCtrlHost).Module),
                hIcon = IntPtr.Zero,
                hCursor = IntPtr.Zero,
                hbrBackground = IntPtr.Zero,
                lpszClassName = _sEmptyContainer_CLASS
            };

            ushort atom = GcjWinApi.RegisterClassEx(ref wndClass);

            int dwContainerStyle = WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN;
            IntPtr hwnd = CreateWindowEx(
                0, _sEmptyContainer_CLASS, "Native Window",
                dwContainerStyle,//WS_BORDER | 
                0, 0, (int)Width, (int)Height,
                hwndParent, IntPtr.Zero, wndClass.hInstance, IntPtr.Zero);
            return hwnd;
        }
        #endregion CreateControl Methods

        #region Visual Function
        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            int dwStyle = 0;
#if  false
            if (string.Compare(NativeContainerClsName, _sEmptyContainer_CLASS, true) != 0)
            {
                dwStyle = WS_BORDER;
            }
#endif
            if (string.Compare(NativeContainerClsName, "EDIT", true) == 0)
            {
                _hwndNativeCtrl = CreateNativeEditCtrl(hwndParent.Handle, dwStyle);
            } else if (string.Compare(NativeContainerClsName, "STATIC", true) == 0)
            {
                _hwndNativeCtrl = CreateNativeStaticCtrl(hwndParent.Handle, dwStyle);
            }
            else if (string.Compare(NativeContainerClsName, RICHEDIT_CLASS, true) == 0)
            {
                _hwndNativeCtrl = CreateNativeRichEditCtrl(hwndParent.Handle, dwStyle);
            }
            else if (string.Compare(NativeContainerClsName, _sEmptyContainer_CLASS, true) == 0)
            {
                _hwndNativeCtrl = CreateNativeContainerCtrl(hwndParent.Handle, dwStyle);
                //if(string.Compare(NativeCtrlClsName, _sCustomizedNative_CLASS) == 0)
                //{
                //    _webView2 = new WebView2();
                //    //_webView2.Dock = DockStyle.Fill;
                //}
            }
            else
            {
                _hwndNativeCtrl = CreateNativeStaticCtrl(hwndParent.Handle, dwStyle);
            }

            if (_hwndNativeCtrl == IntPtr.Zero)
            {
                int nError = Marshal.GetLastWin32Error();
                uint nError2 = GcjWinApi.GetLastError();
                string sErrMsg = CtrlUtil.GetErrorMessage((uint)nError);

                throw new InvalidOperationException("can not create native child window" + sErrMsg);
            }

            this.Loaded += NativeWndCtrlHost_Loaded;
            if (string.Compare(NativeContainerClsName, _sEmptyContainer_CLASS, true) != 0)
            {
                // 创建自定义的 WndProc 委托
                newWndProcDelegate = new WndProcDelegate(CustomWndProc);
                IntPtr newWndProcPtr = Marshal.GetFunctionPointerForDelegate(newWndProcDelegate);
                // 替换 WndProc 并保存原始的 WndProc
                oldWndProc = GcjWinApi.SetWindowLongPtr((IntPtr)HWnd, GcjWinApi.GWL_WNDPROC, newWndProcPtr);
            }

            Trace.WriteLine($"CustomizedNativeWndCtrlHost({NativeContainerClsName})::BuildWindowCore Update Width:{(int)Width}, Height:{(int)Height} with CornerRadius({CornerRadius?.TopLeft}, {CornerRadius?.BottomRight})");
            ShowWindow(_hwndNativeCtrl, SW_SHOW);
            UpdateWindow(_hwndNativeCtrl);

            return new HandleRef(this, _hwndNativeCtrl);
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            GcjWinApi.DestroyWindow(hwnd.Handle);
            //if(_webView2!=null ) _webView2?.Dispose();
        }
        #endregion Visual Function

        #region EventHander
        private void NativeWndCtrlHost_Loaded(object sender, RoutedEventArgs e)
        {
            // 获取窗口句柄
            IntPtr hwnd = _hwndNativeCtrl;// new WindowInteropHelper(this).Handle;

            double nActiveWidth = WpfPanel.ActualWidth;
            double nActiveHeight = WpfPanel.ActualHeight;
            Trace.WriteLine($"CustomizedNativeWndCtrlHost({NativeContainerClsName})::NativeWndCtrlHost_Loaded: Recreate Rgn and Update Width:{(int)nActiveWidth}, Height:{(int)nActiveHeight} with CornerRadius({CornerRadius?.TopLeft}, {CornerRadius?.BottomRight})");

            var source = PresentationSource.FromVisual(this);
            if (source != null)
            {
                var dpiScale = source.CompositionTarget.TransformToDevice;
                nActiveWidth = (int)(nActiveWidth * dpiScale.M11);
                nActiveHeight = (int)(ActualHeight * dpiScale.M22);
            }

            IntPtr hRgn = CreateRoundRectRgn(0, 0, (int)nActiveWidth, (int)nActiveHeight, (int)CornerRadius?.TopLeft, (int)CornerRadius?.BottomRight);
            SetWindowRgn(hwnd, hRgn, true);
            GcjWinApi.RedrawWindow(HWndReal, IntPtr.Zero, IntPtr.Zero, GcjWinApi.RDW_INVALIDATE | GcjWinApi.RDW_ERASE);

            UpdateWindow(_hwndNativeCtrl);
            ShowWindow(_hwndNativeCtrl, SW_SHOW);

            if (NativeContainerClsName == _sEmptyContainer_CLASS)
            {
                int dwStyle = 0;
                if (string.Compare(this.CustomizedNativeCtrlClsNameInEmptyContainer, "EDIT", true) == 0)
                {
                    _hwndNativeCtrlInContainer = CreateNativeEditCtrl(_hwndNativeCtrl, dwStyle);
                }
                else if (string.Compare(CustomizedNativeCtrlClsNameInEmptyContainer, "STATIC", true) == 0)
                {
                    _hwndNativeCtrlInContainer = CreateNativeStaticCtrl(_hwndNativeCtrl, dwStyle);
                }
                else if (string.Compare(CustomizedNativeCtrlClsNameInEmptyContainer, RICHEDIT_CLASS, true) == 0)
                {
                    _hwndNativeCtrlInContainer = CreateNativeRichEditCtrl(_hwndNativeCtrl, dwStyle);
                }
                else if (string.Compare(CustomizedNativeCtrlClsNameInEmptyContainer, _sCustomizedNative_CLASS, true) == 0)
                {
                    _hwndNativeCtrlInContainer = WebView2?.Handle ?? IntPtr.Zero;
                    //_webView2?.CreateControl();
                    SetParent(_hwndNativeCtrlInContainer, _hwndNativeCtrl);
                    //InitializeWebViewAsync();
                }
                else
                {
                    _hwndNativeCtrlInContainer = CreateNativeEditCtrl(_hwndNativeCtrl, dwStyle);
                }

                if (_hwndNativeCtrlInContainer != IntPtr.Zero)
                {
                    ShowWindow(_hwndNativeCtrlInContainer, GcjWinApi.SW_SHOW);
                    UpdateWindow(_hwndNativeCtrlInContainer);
                }
            }
        }
        #endregion EventHander

        #region Methods
        private async void InitializeWebViewAsync()
        {
        }

        private int DbgWndEmptyContainer(IntPtr hWnd)
        {
            PAINTSTRUCT ps = new PAINTSTRUCT();
            IntPtr hdc = BeginPaint(hWnd, out ps);

            Rectangle rectClient;
            GetClientRect(hWnd, out rectClient);
            IntPtr hBrush = CreateSolidBrush(0xADD8E6); // 浅蓝色
            FillRect(hdc, ref rectClient, hBrush);
            DeleteObject(hBrush);

            // 画黑边
            IntPtr hBlackBrush = GetStockObject(GcjWinApi.NULL_BRUSH); // 不填充
            IntPtr hOldBrush = SelectObject(hdc, hBlackBrush);

            // 计算圆心和半径
            int centerX = (rectClient.Width) / 2;
            int centerY = (rectClient.Height) / 2;
            int radius = Math.Min(centerX, centerY) * 2 / 3; // 半径

            // 画圆
            Ellipse(hdc, centerX - radius, centerY - radius, centerX + radius, centerY + radius);

            // 恢复旧的画刷
            SelectObject(hdc, hOldBrush);

            EndPaint(hWnd, ref ps);
            return 0;
            //return DefWindowProc(hWnd, msg, wParam, lParam);
        }

        private int AdjustNativePosByContainer(IntPtr hWnd)
        {
            Rectangle rect;
            GetWindowRect(hWnd, out rect);

            int nActiveWidth = rect.Width;
            int nActiveHeight = rect.Height;

            var source = PresentationSource.FromVisual(WpfPanel);
            if (source != null)
            {
                var dpiScale = source.CompositionTarget.TransformToDevice;
                nActiveWidth = (int)(nActiveWidth * dpiScale.M11);
                nActiveHeight = (int)(ActualHeight * dpiScale.M22);
            }

            IntPtr hRgn = GcjWinApi.CreateRoundRectRgn(0, 0, (int)nActiveWidth, (int)nActiveHeight, (int)CornerRadius?.TopLeft, (int)CornerRadius?.BottomRight);
            GcjWinApi.SetWindowRgn(hWnd, hRgn, true);
            GcjWinApi.SetWindowPos(hWnd, IntPtr.Zero, 0, 0, (int)nActiveWidth, (int)nActiveHeight, GcjWinApi.SWP_NOMOVE | GcjWinApi.SWP_NOZORDER);

            GcjWinApi.RedrawWindow(hWnd, IntPtr.Zero, IntPtr.Zero, GcjWinApi.RDW_INVALIDATE | GcjWinApi.RDW_ERASE);
            UpdateWindow(hWnd);
            Trace.WriteLine($"CustomizedNativeWndCtrlHost({NativeContainerClsName})::WndEmptyContainer::WM_SIZE: Recreate Rgn and Update Width:{(int)nActiveWidth}, Height:{(int)nActiveHeight} with CornerRadius({CornerRadius?.TopLeft}, {CornerRadius?.BottomRight})");
            return 0;
        }
        #endregion Methods

        #region WndProc
        private IntPtr WndEmptyContainerProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            // 处理消息
            WinMsgUtil.TraceMessageDetail("Wpf.WndHost.Container.Ctrl", hWnd, (uint)msg, wParam, lParam);
            if (msg >= WM_KEYFIRST || msg <= WM_KEYLAST)
            {
                if ((char)wParam == (char)Key.Enter)
                    Debug.Assert(true);
            }
            if (msg == WM_CREATE)
            {
                return GcjWinApi.DefWindowProc(hWnd, msg, wParam, lParam);
            }
            else if (msg == WM_PAINT)
            {
                return GcjWinApi.DefWindowProc(hWnd, msg, wParam, lParam);
            }
            else if (msg == WM_ERASEBKGND)
            {
                IntPtr hdc = (IntPtr)wParam;
                Rectangle rc;
                GetClientRect(hWnd, out rc);

                // 创建一个背景色画刷
                IntPtr hBrush = CreateSolidBrush(CtrlUtil.RGB(0xF0, 0xF0, 0xF0)); // 浅灰色背景
                FillRect(hdc, ref rc, hBrush);
                DeleteObject(hBrush);
                // 返回 1 表示我们已处理消息，Windows 不需要再处理
                return 1;
            }
            else if (msg == WM_SHOWWINDOW)
            {
                //return 0;
                return GcjWinApi.DefWindowProc(hWnd, msg, wParam, lParam);
            }
            else if (msg == WM_SIZE)
            {
                //return AdjustNativePosByContainer(hWnd);
                return GcjWinApi.DefWindowProc(hWnd, msg, wParam, lParam);
            }
            else if (msg == WM_LBUTTONDOWN)
            {
            }
            else if (msg >= WM_MOUSEFIRST && msg <= WM_MOUSELAST)
            {
            }
            else if (msg >= WM_KEYFIRST && msg <= WM_KEYLAST)
            {
            }
            else
            {
                switch (msg) { 
                    case WM_IME_KEYDOWN:
                        break;
                    default:
                        break;
                }
            }
            return GcjWinApi.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        private IntPtr CustomWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WM_CLOSE:
                    {
                        WinMsgUtil.TraceMessageDetail("Wpf.WndHost.RealCtrl", hWnd, (uint)msg, wParam, lParam);
                        //if (HWnd == hWnd && oldWndProc != IntPtr.Zero)
                        //{
                        //    GcjWinApi.SetWindowLongPtr(hWnd, GcjWinApi.GWL_WNDPROC, oldWndProc);
                        //    oldWndProc = IntPtr.Zero;
                        //}
                    }
                    break;
                case WM_CREATE: 
                    {
                        CREATESTRUCT createStruct = Marshal.PtrToStructure<CREATESTRUCT>(lParam);
                        WinMsgUtil.TraceMessageDetail("Wpf.WndHost.RealCtrl", hWnd, (uint)msg, wParam, lParam);

                        if (HWnd == hWnd)
                        {
                            // 获取窗口的宽度和高度
                            int width = createStruct.cx;
                            int height = createStruct.cy;

                            var source = PresentationSource.FromVisual(this);
                            if (source != null)
                            {
                                var dpiScale = source.CompositionTarget.TransformToDevice;
                                width = (int)(width * dpiScale.M11);
                                height = (int)(height * dpiScale.M22);
                            }

                            IntPtr hRgn = GcjWinApi.CreateRoundRectRgn(0, 0, (int)width, (int)height, (int)CornerRadius?.TopLeft, (int)CornerRadius?.BottomRight);
                            GcjWinApi.SetWindowRgn((IntPtr)HWnd, hRgn, true);
                            GcjWinApi.UpdateWindow((IntPtr)HWnd);

                            Trace.WriteLine($"CustomizedNativeWndCtrlHost({NativeContainerClsName})::WM_CREATE: Recreate Rgn and Update Width:{(int)width}, Height:{(int)height} with CornerRadius({CornerRadius?.TopLeft}, {CornerRadius?.BottomRight})");
                        }
                    }
                    break;
                case WM_CTLCOLORSTATIC:
                //if (false)
                //{
                //    IntPtr hdc = wParam;
                //    SetBkMode(hdc, GcjWinApi.TRANSPARENT); // 设置背景为透明模式
                //    return 0;
                //}
                //break;
                case WM_ERASEBKGND:
                case WM_PAINT:
                case WM_DESTROY:
                case WM_SIZE:
                    WinMsgUtil.TraceMessageDetail("Wpf.WndHost.RealCtrl", hWnd, (uint)msg, wParam, lParam);
                    break;
                case WM_LBUTTONDOWN:
                case WM_RBUTTONDOWN:
                case WM_MBUTTONDOWN:
                    WinMsgUtil.TraceMessageDetail("Wpf.WndHost.RealCtrl", hWnd, (uint)msg, wParam, lParam);
                    break;
                case WM_MOUSEMOVE:
                case WM_LBUTTONUP:
                case WM_MBUTTONUP:
                case WM_RBUTTONUP:
                    break;
                case WM_IME_KEYDOWN:
                case WM_KEYDOWN:
                    WinMsgUtil.TraceMessageDetail("Wpf.WndHost.RealCtrl", hWnd, (uint)msg, wParam, lParam);
                    break;
                default:
                    break;
            }

            // 调用原始的 WndProc
            return GcjWinApi.CallWindowProc(oldWndProc, hWnd, msg, wParam, lParam);
        }
        #endregion WndProc

        #region properties
        #region NativeContainerClsName // 定义 NativeContainerClsName 依赖属性
        public string? NativeContainerClsName
        {
            get { return WpfPanel?.NativeCtrlClsName; }
            //set { if (WpfPanel != null) WpfPanel.NativeCtrlClsName = (string)value; }
        }
        #endregion NativeContainerClsName

        #region CustomizedNativeCtrlClsNameInEmptyContainer
        public string CustomizedNativeCtrlClsNameInEmptyContainer
        {
            get { return WpfPanel?.CustomizedNativeCtrlClsNameInEmptyContainer; }
        }
        #endregion CustomizedNativeCtrlClsNameInEmptyContainer


        #region DefaultNativeContainerClsName // 定义 DefaultNativeContainerClsName 依赖属性
        [Description("Gets or sets the Native Control Window Class Name")]
        [Category("GcjControl")]
        public string? DefaultNativeContainerClsName
        {
            get { return _sEmptyContainer_CLASS; }
        }
        #endregion DefaultNativeContainerClsName // 定义 DefaultNativeContainerClsName 依赖属性

        #region NativeCtrlClsName // 定义 NativeCtrlClsName 依赖属性
        public string NativeCtrlClsName
        {
            get { return (string)(WpfPanel?.NativeCtrlClsName); }
        }
        #endregion NativeCtrlClsName // 定义 NativeCtrlClsName 依赖属性

        #region GcjVer // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedNativeWndCtrlHost),
            new PropertyMetadata("Gcj CustomizedNativeWndCtrlHost v1.0"));

        [Description("Gets or sets the CustomizedNativeWndCtrlHost Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer // 定义 GcjVer 依赖属性
        
        #region CornerRadius // 定义 CornerRadius 依赖属性
        public CornerRadius? CornerRadius
        {
            get { return WpfPanel?.CornerRadius; }
            //set { if(WpfPanel!=null) WpfPanel.CornerRadius = (CornerRadius)value; }
        }

        #endregion CornerRadius

        #region WpfPanel // 定义 WpfPanel 依赖属性
        CustomizedNativeContainer? _nativeContainer;
        [Description("Gets or sets the Native Control Panel")]
        [Category("GcjControl")]
        public CustomizedNativeContainer? WpfPanel
        {
            get { return _nativeContainer; }
        }
        #endregion WpfPanel // 定义 WpfPanel 依赖属性
        #endregion properties
    }
}
