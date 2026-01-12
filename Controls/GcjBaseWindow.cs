using BaseUtil;
using GcjUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static BaseUtil.GcjWinApi;
using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;

namespace GcjUiCtrl.Control
{
    public class GcjBaseWindow : System.Windows.Window, INotifyPropertyChanged
    {
        #region DropFileSupport

        #region DropFilePath         // 定义 DropFilePath 依赖属性
        public static readonly DependencyProperty _DropFilePath = DependencyProperty.Register(
            "DropFilePath",
            typeof(string),
            typeof(GcjBaseWindow),
            new PropertyMetadata(null));

        [Description("Gets or sets the GcjBaseWindow Drop File Path")]
        [Category("GcjControl")]
        public string? DropFilePath
        {
            get { return (string?)GetValue(_DropFilePath); }
            set { SetValue(_DropFilePath, value); }
        }
        #endregion DropFilePath

        #region OnFileDropped Event
        private void GcjBaseWindow_DragOver(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void GcjBaseWindow_DragEnter(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                // Allow copy action
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                // No valid data - disable drag effect
                e.Effects = DragDropEffects.None;
            }
        }

        private void GcjBaseWindow_Drop(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                // Get the file paths from the dropped data
                string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                DropFilePath = droppedFiles.FirstOrDefault();

                RaiseOnFileDropped(e);
            }
        }

        // 注册一个路由事件
        public static readonly RoutedEvent OnFileDroppedEvent = EventManager.RegisterRoutedEvent(
            "OnFileDropped", // 事件名称
            RoutingStrategy.Bubble, // 路由策略：冒泡、隧道、直接
            typeof(GcjDragEventHandler), // 事件处理程序的类型
            typeof(GcjBaseWindow)); // 注册事件的类

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("Customized GcjBaseWindow Drop File Event Handler")]
        [Category("GcjControl")]
        public event GcjDragEventHandler OnFileDropped
        {
            add { AddHandler(OnFileDroppedEvent, value); }
            remove { RemoveHandler(OnFileDroppedEvent, value); }
        }
        protected void RaiseOnFileDropped(DragEventArgs e)
        {
            GcjDragEventArgs newArgs = new GcjDragEventArgs()
            {
                RoutedEvent = OnFileDroppedEvent,
                DropFiles = (string[])e.Data.GetData(DataFormats.FileDrop)

            };
            RaiseEvent(newArgs);
        }

        #endregion OnFileDropped Event

        #endregion DropFileSupport

        #region properties
        public static System.Windows.Media.Brush DefaultBgBrush
        {
            get
            {
                //return System.Windows.Media.Brushes.LightGray;
                return new System.Windows.Media.SolidColorBrush(DefaultBgColor);
            }
        }

        public static System.Windows.Media.Brush DefaultRoundEditBgBrush
        {
            get
            {
                //return System.Windows.Media.Brushes.LightGray;
                return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 250, 250, 250));
            }
        }

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(GcjBaseWindow),
            new PropertyMetadata("Gcj Window 1.0"));


        [Description("Gets or sets the GcjBaseWindow Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region CustomizedBgColor
        public static readonly DependencyProperty __customizedBgColor = DependencyProperty.Register(
            "CustomizedBgColor",
            typeof(System.Windows.Media.Color),
            typeof(GcjBaseWindow),
            new PropertyMetadata(GcjBaseWindow.DefaultBgColor));

        public System.Windows.Media.Color CustomizedBgColor
        {
            get { return (System.Windows.Media.Color)GetValue(__customizedBgColor); }
            set
            {
                SetValue(__customizedBgColor, value);
                SetValue(__customizedBgBrush, new SolidColorBrush(value));
                SetValue(__customizedFgColor, CtrlUtil.GetWhiteBlackForeGroundColor(value));
                SetValue(__customizedFgBrush, new SolidColorBrush((System.Windows.Media.Color)GetValue(__customizedFgColor)));
            }
        }

        public static System.Windows.Media.Color DefaultBgColor
        {
            get
            {
                //return System.Windows.Media.Colors.LightGray;
                //return System.Windows.Media.Color.FromRgb(230, 230, 230);
                //return System.Windows.Media.Color.FromArgb(255, 243, 243, 243);
                return System.Windows.Media.Color.FromArgb(255, 240, 240, 240);
            }
        }
        public static System.Windows.Media.Brush DefaultFgBrush
        {
            get
            {
                return System.Windows.Media.Brushes.Black;
            }
        }

        public static System.Windows.Media.Color DefaultFgColor
        {
            get { return System.Windows.Media.Colors.Black; }
        }

        #endregion CustomizedBgColor

        #region CustomizedFgColor
        public static readonly DependencyProperty __customizedFgColor = DependencyProperty.Register(
            "CustomizedFgColor",
            typeof(System.Windows.Media.Color),
            typeof(GcjBaseWindow),
            new PropertyMetadata(Colors.White));
        public System.Windows.Media.Color CustomizedFgColor
        {
            get
            {
                return CtrlUtil.GetWhiteBlackForeGroundColor((System.Windows.Media.Color)GetValue(__customizedFgColor));
            }
            set
            {
                SetValue(__customizedFgColor, value);
                SetValue(__customizedFgBrush, new SolidColorBrush(value));
            }
        }
        #endregion CustomizedBgColor

        #region CustomizedBgBrush
        public static readonly DependencyProperty __customizedBgBrush = DependencyProperty.Register(
            "CustomizedBgBrush",
            typeof(System.Windows.Media.Brush),
            typeof(GcjBaseWindow),
            new PropertyMetadata(System.Windows.Media.Brushes.LightGray));

        public System.Windows.Media.Brush CustomizedBgBrush
        {
            get
            {
                return (System.Windows.Media.Brush)GetValue(__customizedBgBrush);
            }
            set
            {
                SetValue(__customizedBgBrush, value);
            }
        }
        #endregion CustomizedBgBrush

        #region CustomizedFgBrush
        //System.Windows.Media.Brush? __customizedFgBrush = null;// new SolidColorBrush(__customizedBgColor);
        public static readonly DependencyProperty __customizedFgBrush = DependencyProperty.Register(
         "CustomizedFgBrush",
         typeof(System.Windows.Media.Brush),
         typeof(GcjBaseWindow),
         new PropertyMetadata(GcjBaseWindow.DefaultFgBrush));
        public System.Windows.Media.Brush CustomizedFgBrush
        {
            get
            {
                return (System.Windows.Media.Brush)GetValue(__customizedFgBrush);
            }
            set
            {
                SetValue(__customizedFgBrush, value);
            }
        }
        #endregion CustomizedFgBrush

        #region RunTopMostMode // 定义 RunTopMostMode 依赖属性
        public static readonly DependencyProperty RunTopMostModeProperty =
            DependencyProperty.Register("RunTopMostMode", typeof(bool), typeof(GcjBaseWindow), new PropertyMetadata(false));

        [Description("Gets or sets the GcjBaseWindow RunTopMostMode")]
        [Category("GcjControl")]
        public bool RunTopMostMode
        {
            get
            {
                return (bool)GetValue(RunTopMostModeProperty);
            }
            set
            {
                SetValue(RunTopMostModeProperty, value);
            }
        }
        #endregion RunTopMostMode

        #endregion

        private const uint IMAGE_ICON = 1;
        private const uint LR_LOADFROMFILE = 0x00000010;
        public NOTIFYICONDATA _notifyIconData;

        private uint nTrayId = 0;
        HwndSource? source = null;
        private bool bQuitWhenClosing = true;
        Style? trayMenuStyle = null;
        private bool _bAfterLoadEvent = false;

        //private WndProcDelegate _wndProc;
        private WndProcDelegate _newWndProc;
        private IntPtr _oldWndProc = IntPtr.Zero;

        #region constructor
        public GcjBaseWindow() : base()
        {
            //DefaultStyleKey = typeof(GcjBaseWindow);
            Loaded += GcjBaseWindow_Loaded;
            Unloaded += GcjBaseWindow_Unloaded;
            Drop += GcjBaseWindow_Drop;
            DragEnter += GcjBaseWindow_DragEnter;
            DragOver += GcjBaseWindow_DragOver;
            SizeChanged += GcjBaseWindow_SizeChanged;
            ContentRendered += GcjBaseWindow_ContentRendered;
            this.Closed += GcjBaseWindow_Closed;
            _newWndProc = new WndProcDelegate(WindowProc);
        }

        #endregion

        #region methods
        public void UpdateAllControlsVisual(DependencyObject parent, int nMaxLevel = -1)
        {
            if (nMaxLevel > 0 || nMaxLevel == -1)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);

                    // 强制更新子控件的视觉外观
                    if (child is UIElement element)
                    {
                        element.InvalidateVisual();
                        element.UpdateLayout();
                    }

                    // 递归更新子控件
                    UpdateAllControlsVisual(child, nMaxLevel < 0 ? nMaxLevel : nMaxLevel - 1);
                }
            }
            // 遍历所有子控件
        }

        public void CreateNotifyIcon(uint id, string? sIconFile = null, string? sTip = "")
        {
#if false
            if (_bAfterLoadEvent)
                CreateNotifyIconAfterLoadEvent(id, sIconFile , sTip);
            else
                CreateNotifyIconBeforeLoadEvent(id, sIconFile, sTip);
#endif
        }

#if false
        private void CreateNotifyIconAfterLoadEvent(uint id, string? sIconFile = null, string? sTip = "")
        {
            // Load Eventあるいは画面のClickEventなどに初期化する場合
            nTrayId = id;
            IntPtr iconHandle = (sIconFile == null) ? GetCurrWndIconHandle() : LoadIconFromFile(sIconFile);
            _notifyIconData = new NOTIFYICONDATA
            {
                cbSize = (uint)Marshal.SizeOf(typeof(NOTIFYICONDATA)),
                hWnd = new WindowInteropHelper(this).Handle,
                uID = id,
                uFlags = NIF_ICON | NIF_MESSAGE | NIF_TIP,
                uCallbackMessage = WM_TRAYICON,
                hIcon = iconHandle /*SystemIcons.Application.Handle*/,
                szTip = sTip ?? Process.GetCurrentProcess().ProcessName
            };

            if (_notifyIconData.hWnd == 0x00)
                _notifyIconData.hWnd = new WindowInteropHelper(this).Handle;
            if (_notifyIconData.hIcon == 0x00)
                _notifyIconData.hIcon = GetCurrWndIconHandle();
            int nRet = Shell_NotifyIcon(NIM_ADD, ref _notifyIconData);
            source = HwndSource.FromHwnd(_notifyIconData.hWnd);
            source.AddHook(TrayWndProc);
            bQuitWhenClosing = false;
        }

        protected void CreateNotifyIconBeforeLoadEvent(uint id, string? sIconFile = null, string? sTip = "")
        {
            // LoadEventの前にNotifyIconを初期化する場合
            Loaded += (s, e) => CreateNotifyIconAfterLoadEvent(id, sIconFile, sTip);
        }

#endif
        private IntPtr LoadIconFromFile(string filePath)
        {
            return LoadImage(IntPtr.Zero, filePath, IMAGE_ICON, 0, 0, LR_LOADFROMFILE);
        }

        private IntPtr LoadAppIcon() {
            IntPtr iconHandle = LoadIcon(IntPtr.Zero, 0x7F00); // 使用默认的应用程序图标，0x7F00 是 IDI_APPLICATION 的值
            return iconHandle;
        }

#if false
        private IntPtr GetCurrWndIconHandle()
        {
            // 获取应用程序窗口的 Icon
            var iconSource = this.Icon;

            if (iconSource != null)
            {
                // 将 BitmapSource 转换为 System.Drawing.Icon
                BitmapSource? bitmapSource = iconSource as BitmapSource;
                if (bitmapSource != null)
                {
                    return GetHIconFromBitmapSource(bitmapSource);
                }
            }
            return IntPtr.Zero;
        }

        private IntPtr GetHIconFromBitmapSource(BitmapSource bitmapSource)
        {
            // 将 BitmapSource 转换为 Bitmap
            Bitmap bitmap = BitmapFromSource(bitmapSource);
            return bitmap.GetHicon(); // 获取 HICON 句柄
        }

        private Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new System.IO.MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }
#endif

        private void ShowTaskTrayMenu()
        {
            // 创建自定义 ContextMenu
            if (trayMenuStyle == null)
                trayMenuStyle = (Style)FindResource("xCustomizedRoundContextMenuStyle");
            Debug.Assert(trayMenuStyle != null);
            CustomizedRoundContextMenu trayMenu = new CustomizedRoundContextMenu
            {
                Style = trayMenuStyle // 在创建时应用样式
            }; ;

            MenuItem menuItem1 = new MenuItem { Header = "Option 1" };
            MenuItem menuItem2 = new MenuItem { Header = "Option 2" };
            MenuItem exitMenuItem = new MenuItem { Header = "Exit" };

            exitMenuItem.Click += (s, e) => {
                bQuitWhenClosing = true;
                Shell_NotifyIcon(NIM_DELETE, ref _notifyIconData);
                Application.Current.Shutdown();
            };

            trayMenu.Items.Add(menuItem1);
            trayMenu.Items.Add(menuItem2);
            trayMenu.Items.Add(new Separator());
            trayMenu.Items.Add(exitMenuItem);

            // 显示菜单
            trayMenu.IsOpen = true;
        }
#endregion

        #region WndProc
        private IntPtr TrayWndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_TRAYICON && wParam.ToInt32() == nTrayId)
            {
                switch (lParam.ToInt32())
                {
                    case WM_LBUTTONDBLCLK:
                        // 双击图标时显示窗口
                        if (this.IsVisible == false)
                        {
                            Show();
                            WindowState = WindowState.Normal;
                            Activate();
                        }
                        else
                            Hide();
                        break;
                    case WM_MOUSEMOVE: break;
                    case WM_LBUTTONDOWN: break;
                    case WM_LBUTTONUP: break;

                    case WM_RBUTTONDBLCLK: break;
                    case WM_RBUTTONDOWN: break;
                    case WM_RBUTTONUP:
                        //PopUp menu for close etc...
                        ShowTaskTrayMenu();
                        break;

                    case WM_MOUSEWHEEL: break;
                    case WM_MBUTTONDOWN: break;
                    case WM_MBUTTONUP: break;
                    case WM_MBUTTONDBLCLK: break;
                    case WM_MOUSEHOVER: break;
                    case WM_MOUSELEAVE: break;
                    case WM_QUERYENDSESSION:
                        // 系统即将关闭或注销
                        Trace.WriteLine("System is preparing to shut down or log out.");
                        handled = true; // 通知系统可以安全退出
                        bQuitWhenClosing = true;
                        return new IntPtr(1); // 返回 1 表示允许关闭或注销

                    case WM_ENDSESSION:
                        // 系统确认关闭或注销
                        if (wParam.ToInt32() != 0) // 检查是否真的结束了会话
                        {
                            Trace.WriteLine("System is shutting down or logging out.");
                            // 进行任何必要的清理操作
                        }
                        bQuitWhenClosing = true;
                        handled = true;
                        break;
                    default:
                        break;
                }
            }

            return IntPtr.Zero;
        }

        private IntPtr WindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            // 处理消息
            WinMsgUtil.TraceMessageDetail("WPF Window Proc", hWnd, (uint)msg, wParam, lParam);
            if(msg >= WM_KEYFIRST|| msg <= WM_KEYLAST)
            {
                if ((char) wParam == (char)Key.Enter)
                    Debug.Assert(true);
            }
            else { 
                switch (msg) { 
                    case WM_IME_KEYDOWN:
                        break;
                    default:
                        break;
                } 
            }
            return GcjWinApi.CallWindowProc(_oldWndProc, hWnd, msg, wParam, lParam);
        }

        #endregion

        #region event handling
        private void GcjBaseWindow_Closed(object? sender, EventArgs e)
        {
            if (source != null && bQuitWhenClosing == false)
            {
                source.RemoveHook(TrayWndProc);
            }
            bQuitWhenClosing = true;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!bQuitWhenClosing)
            {
                Debug.WriteLine("This window is non-modal (Show).");
                Debug.Assert(_notifyIconData.cbSize > 0);
                //Shell_NotifyIcon(NIM_DELETE, ref _notifyIconData);
                e.Cancel = true; // 隐藏窗口而不是关闭
                Hide();
            }
            else {
                List<HwndHost> lstHwndHost = DepUtil.GetAllChildrenOfType<HwndHost>(this);
                foreach(HwndHost hwnd in lstHwndHost)
                {
                    GcjWinApi.SendMessage(hwnd.Handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    //GcjWinApi.CloseWindow(hwnd.Handle);
                }
            }
        }

        private void GcjBaseWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateAllControlsVisual(this, 3);
        }

        private void GcjBaseWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            if(_oldWndProc!= null && _oldWndProc != IntPtr.Zero)
            {
                GcjWinApi.SetWindowLongPtr(new WindowInteropHelper(this).Handle, GcjWinApi.GWL_WNDPROC, _oldWndProc);
                _oldWndProc = IntPtr.Zero;
            }
            _bAfterLoadEvent = false;
        }

        private void GcjBaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _startLoad = DateTime.Now;
            _bAfterLoadEvent = true;
#if true
            if(_oldWndProc == null || _oldWndProc == IntPtr.Zero)
            {
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                // 替换 WndProc 并保存原始的 WndProc
                _oldWndProc = GcjWinApi.SetWindowLongPtr(hwnd, GcjWinApi.GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(_newWndProc));
            }
#endif
        }

        protected DateTime  _startLoad = DateTime.Now;
        private void GcjBaseWindow_ContentRendered(object sender, EventArgs e)
        {
            TimeSpan span = DateTime.Now - _startLoad;
            Debug.WriteLine($"Render Finished in {span.ToString()} seconds based on _startLoad");
        }

        #endregion
    }
}
