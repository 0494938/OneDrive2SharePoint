using GcjUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

namespace BaseUtil
{
    public partial class CtrlUtil
    {
        public static uint RGB(byte r, byte g, byte b)
        {
            return (uint)((r << 16) | (g << 8) | b);
        }

        public static bool IsInDesignMode(DependencyObject obj)
        {
            return DesignerProperties.GetIsInDesignMode(obj);
        }

        public static System.Windows.Point GetPosFromTopWnd(UIElement elem)
        {
            if (elem is Window)
                return new Point(0, 0);
            // 假设 elem 是需要获取位置的控件
            Window? window = DepUtil.FindParent<Window>(elem);
            if (window != null)
            {
                Point position = elem.TranslatePoint(new Point(0, 0), window);
                Debug.WriteLine($"Control Position - X: {position.X}, Y: {position.Y}");
                return position;
            }
            else
                return new System.Windows.Point(0, 0);
        }

        public static int GetTitleBarHeight()
        {
            return GcjWinApi.GetSystemMetrics(GcjWinApi.SM_CYCAPTION);
        }

        public static int GetTitleBarActualHeight(UIElement elem)
        {
            if (HasCaptionStyle(elem))
                return GcjWinApi.GetSystemMetrics(GcjWinApi.SM_CYCAPTION);
            else
                return 0;
        }

        public static bool HasCaptionStyle(UIElement elem)
        {
            IntPtr hwnd = IntPtr.Zero;
            if (elem is Window) { 
                hwnd = new System.Windows.Interop.WindowInteropHelper((Window)elem ).Handle; 
            } else { 
                Window window = DepUtil.FindParent<Window>(elem);
                hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
            }
            
            int style = GcjWinApi.GetWindowLong(hwnd, GcjWinApi.GWL_STYLE);
            return (style & GcjWinApi.WS_CAPTION) == GcjWinApi.WS_CAPTION;
        }

        public static System.Windows.Media.Color GetWhiteBlackForeGroundColor(System.Windows.Media.Color colorBkGround)
        {
            // 根据加权的RGB分量计算感知亮度（YIQ公式）
            double luminance = (0.299 * colorBkGround.R + 0.587 * colorBkGround.G + 0.114 * colorBkGround.B) / 255;

            // 如果亮度小于0.5，选择白色作为前景色；否则选择黑色
            return luminance > 0.5 ? Colors.Black : Colors.White; ;
        }

        public static System.Windows.Media.Brush GetWhiteBlackForeGroundBrush(System.Windows.Media.Color colorBkGround)
        {
            // 根据加权的RGB分量计算感知亮度（YIQ公式）
            double luminance = (0.299 * colorBkGround.R + 0.587 * colorBkGround.G + 0.114 * colorBkGround.B) / 255;

            // 如果亮度小于0.5，选择白色作为前景色；否则选择黑色
            return luminance > 0.5 ? Brushes.Black : Brushes.White; ;
        }

        public static System.Windows.Media.Brush GetWhiteBlackForeGroundBrush(System.Windows.Media.Brush colorBkBrush)
        {
            return GetWhiteBlackForeGroundBrush((colorBkBrush is SolidColorBrush brush) ? brush.Color : Colors.Black);
        }

        public static string GetErrorMessage(uint errorCode)
        {
            StringBuilder messageBuffer = new StringBuilder(256);
            uint result = GcjWinApi.FormatMessage(
                GcjWinApi.FORMAT_MESSAGE_FROM_SYSTEM | GcjWinApi.FORMAT_MESSAGE_IGNORE_INSERTS,
                IntPtr.Zero,
                (uint)errorCode,
                0,
                messageBuffer,
                (uint)messageBuffer.Capacity,
                IntPtr.Zero);

            return result > 0 ? messageBuffer.ToString().Trim() : "Unknown error";
        }

        public static string GetLastErrorMessage()
        {
            StringBuilder messageBuffer = new StringBuilder(256);
            uint result = GcjWinApi.FormatMessage(
                GcjWinApi.FORMAT_MESSAGE_FROM_SYSTEM | GcjWinApi.FORMAT_MESSAGE_IGNORE_INSERTS,
                IntPtr.Zero,
                (uint)Marshal.GetLastWin32Error(),
                0,
                messageBuffer,
                (uint)messageBuffer.Capacity,
                IntPtr.Zero);

            return result > 0 ? messageBuffer.ToString().Trim() : "Unknown error";
        }

        public static bool isWinCtrl(IntPtr hWnd, string sClsName= "STATIC")
        {
            StringBuilder sbClassName = new StringBuilder(100);
            GcjWinApi.GetClassName(hWnd, sbClassName, sbClassName.Capacity);
            string szClsName = sbClassName.ToString();
            if (string.Compare(szClsName, sClsName, true) == 0)
            {
                return true;
            }
            return false;
        }
        public static bool isWinStaticCtrl(IntPtr hWnd)
        {
            return isWinCtrl(hWnd, "STATIC");
        }
        public static bool isWinButtonCtrl(IntPtr hWnd)
        {
            return isWinCtrl(hWnd, "BUTTON");
        }
        public static bool isWinRichEditCtrl(IntPtr hWnd)
        {
            return isWinCtrl(hWnd, "RICHEDIT50W");
        }
        public static bool isWinEditCtrl(IntPtr hWnd)
        {
            return isWinCtrl(hWnd, "EDIT");
        }
        public static bool isWinComboCtrl(IntPtr hWnd)
        {
            return isWinCtrl(hWnd, "COMBOBOX");
        }
        public static bool isWinListBoxCtrl(IntPtr hWnd)
        {
            return isWinCtrl(hWnd, "LISTBOX");
        }

    }
}
