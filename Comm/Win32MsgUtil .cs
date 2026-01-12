using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static BaseUtil.GcjWinApi;
using Color = System.Drawing.Color;

namespace BaseUtil
{
    public partial class WinMsgUtil
    {
        public static string GetHResultMsg(long hResult)
        {
            return GetHResultMsg((int)hResult);
        }

        public static string GetHResultMsg(uint hResult)
        {
            return GetHResultMsg((int)hResult);
        }

        public static string GetHResultMsg(int hResult)
        {
            if (hResult != 0) // 非0表示有错误
            {
                StringBuilder message = new StringBuilder(512);
                GcjWinApi.FormatMessage(
                    GcjWinApi.FORMAT_MESSAGE_FROM_SYSTEM,
                    IntPtr.Zero,
                    (uint)hResult,
                    0,
                    message,
                    (uint)message.Capacity,
                    IntPtr.Zero);

                return message.ToString();
            }
            else
            {
                return "Success";
            }
        }

        public static string GetWindowsInfo(IntPtr hWnd, string strCategory, string sWndMsg, string sAppendMsg = "")
        {
            StringBuilder sb = new StringBuilder($"0x{hWnd.ToInt32():X8} {sWndMsg}, {strCategory} : ");
            StringBuilder s = new StringBuilder(256);
            GcjWinApi.GetClassName(hWnd, s, 255);
            sb.Append(s.ToString()).Append(" - '");
            GcjWinApi.GetWindowText(hWnd, s, 255);
            sb.Append(s.ToString()).Append("'");
            if (!string.IsNullOrEmpty(sAppendMsg)) sb.Append(sAppendMsg);
            return sb.ToString();
        }

        public static string GetWindowsInfo(IntPtr hWnd, string strCategory, uint nMsg, string sAppendMsg = "")
        {
            return GetWindowsInfo(hWnd, strCategory, GetMsgAsStr(nMsg), sAppendMsg);
        }

        public static string GetWindowsInfo(IntPtr hWnd, string strCategory, int nMsg, string sAppendMsg = "")
        {
            return GetWindowsInfo(hWnd, strCategory, GetMsgAsStr((uint)nMsg), sAppendMsg);
        }

        public static string GetKeyStateStr(int wParam)
        {
            StringBuilder sb = new StringBuilder("(");
            if ((GcjWinApi.MK_CONTROL & wParam) == GcjWinApi.MK_CONTROL) sb.Append("Ctrl ");
            if ((GcjWinApi.MK_LBUTTON & wParam) == GcjWinApi.MK_LBUTTON) sb.Append("LBtn ");
            if ((GcjWinApi.MK_MBUTTON & wParam) == GcjWinApi.MK_MBUTTON) sb.Append("MBtn ");
            if ((GcjWinApi.MK_RBUTTON & wParam) == GcjWinApi.MK_RBUTTON) sb.Append("RBtn ");
            if ((GcjWinApi.MK_SHIFT & wParam) == GcjWinApi.MK_SHIFT) sb.Append("Shift ");
            if ((GcjWinApi.MK_XBUTTON1 & wParam) == GcjWinApi.MK_XBUTTON1) sb.Append("XBtn1 ");
            if ((GcjWinApi.MK_XBUTTON2 & wParam) == GcjWinApi.MK_XBUTTON2) sb.Append("XBtn2 ");
            if (sb.Length > 1)
                return sb.Append(")").ToString();
            else
                return "";
        }

        class MsgInfoForDebug
        {
            public uint nMsg;
            public string sMsg;
        }

        static Dictionary<uint, string> dictMsgInfoForDebug = new MsgInfoForDebug[] {
            new MsgInfoForDebug(){ nMsg =WM_ACTIVATE, sMsg="WM_ACTIVATE"},
            new MsgInfoForDebug(){ nMsg =WM_ACTIVATEAPP, sMsg="WM_ACTIVATEAPP"},
            new MsgInfoForDebug(){ nMsg =WM_AFXFIRST, sMsg="WM_AFXFIRST"},
            new MsgInfoForDebug(){ nMsg =WM_AFXLAST, sMsg="WM_AFXLAST"},
            new MsgInfoForDebug(){ nMsg =WM_APP, sMsg="WM_APP"},
            new MsgInfoForDebug(){ nMsg =WM_APPCOMMAND, sMsg="WM_APPCOMMAND"},
            new MsgInfoForDebug(){ nMsg =WM_ASKCBFORMATNAME, sMsg="WM_ASKCBFORMATNAME"},
            new MsgInfoForDebug(){ nMsg =WM_CANCELJOURNAL, sMsg="WM_CANCELJOURNAL"},
            new MsgInfoForDebug(){ nMsg =WM_CANCELMODE, sMsg="WM_CANCELMODE"},
            new MsgInfoForDebug(){ nMsg =WM_CAPTURECHANGED, sMsg="WM_CAPTURECHANGED"},
            new MsgInfoForDebug(){ nMsg =WM_CHANGECBCHAIN, sMsg="WM_CHANGECBCHAIN"},
            new MsgInfoForDebug(){ nMsg =WM_CHANGEUISTATE, sMsg="WM_CHANGEUISTATE"},
            new MsgInfoForDebug(){ nMsg =WM_CHAR, sMsg="WM_CHAR"},
            new MsgInfoForDebug(){ nMsg =WM_CHARTOITEM, sMsg="WM_CHARTOITEM"},
            new MsgInfoForDebug(){ nMsg =WM_CHILDACTIVATE, sMsg="WM_CHILDACTIVATE"},
            new MsgInfoForDebug(){ nMsg =WM_CLEAR, sMsg="WM_CLEAR"},
            new MsgInfoForDebug(){ nMsg =WM_CLIPBOARDUPDATE, sMsg="WM_CLIPBOARDUPDATE"},
            new MsgInfoForDebug(){ nMsg =WM_CLOSE, sMsg="WM_CLOSE"},
            new MsgInfoForDebug(){ nMsg =WM_COMMAND, sMsg="WM_COMMAND"},
            new MsgInfoForDebug(){ nMsg =WM_COMMNOTIFY, sMsg="WM_COMMNOTIFY"},
            new MsgInfoForDebug(){ nMsg =WM_COMPACTING, sMsg="WM_COMPACTING"},
            new MsgInfoForDebug(){ nMsg =WM_COMPAREITEM, sMsg="WM_COMPAREITEM"},
            new MsgInfoForDebug(){ nMsg =WM_CONTEXTMENU, sMsg="WM_CONTEXTMENU"},
            new MsgInfoForDebug(){ nMsg =WM_COPY, sMsg="WM_COPY"},
            new MsgInfoForDebug(){ nMsg =WM_COPYDATA, sMsg="WM_COPYDATA"},
            new MsgInfoForDebug(){ nMsg =WM_CREATE, sMsg="WM_CREATE"},
            new MsgInfoForDebug(){ nMsg =WM_CTLCOLORBTN, sMsg="WM_CTLCOLORBTN"},
            new MsgInfoForDebug(){ nMsg =WM_CTLCOLORDLG, sMsg="WM_CTLCOLORDLG"},
            new MsgInfoForDebug(){ nMsg =WM_CTLCOLOREDIT, sMsg="WM_CTLCOLOREDIT"},
            new MsgInfoForDebug(){ nMsg =WM_CTLCOLORLISTBOX, sMsg="WM_CTLCOLORLISTBOX"},
            new MsgInfoForDebug(){ nMsg =WM_CTLCOLORMSGBOX, sMsg="WM_CTLCOLORMSGBOX"},
            new MsgInfoForDebug(){ nMsg =WM_CTLCOLORSCROLLBAR, sMsg="WM_CTLCOLORSCROLLBAR"},
            new MsgInfoForDebug(){ nMsg =WM_CTLCOLORSTATIC, sMsg="WM_CTLCOLORSTATIC"},
            new MsgInfoForDebug(){ nMsg =WM_CUT, sMsg="WM_CUT"},
            new MsgInfoForDebug(){ nMsg =WM_DEADCHAR, sMsg="WM_DEADCHAR"},
            new MsgInfoForDebug(){ nMsg =WM_DELETEITEM, sMsg="WM_DELETEITEM"},
            new MsgInfoForDebug(){ nMsg =WM_DESTROY, sMsg="WM_DESTROY"},
            new MsgInfoForDebug(){ nMsg =WM_DESTROYCLIPBOARD, sMsg="WM_DESTROYCLIPBOARD"},
            new MsgInfoForDebug(){ nMsg =WM_DEVICECHANGE, sMsg="WM_DEVICECHANGE"},
            new MsgInfoForDebug(){ nMsg =WM_DEVMODECHANGE, sMsg="WM_DEVMODECHANGE"},
            new MsgInfoForDebug(){ nMsg =WM_DISPLAYCHANGE, sMsg="WM_DISPLAYCHANGE"},
            new MsgInfoForDebug(){ nMsg =WM_DPICHANGED, sMsg="WM_DPICHANGED"},
            new MsgInfoForDebug(){ nMsg =WM_DPICHANGED_AFTERPARENT, sMsg="WM_DPICHANGED_AFTERPARENT"},
            new MsgInfoForDebug(){ nMsg =WM_DPICHANGED_BEFOREPARENT, sMsg="WM_DPICHANGED_BEFOREPARENT"},
            new MsgInfoForDebug(){ nMsg =WM_DRAWCLIPBOARD, sMsg="WM_DRAWCLIPBOARD"},
            new MsgInfoForDebug(){ nMsg =WM_DRAWITEM, sMsg="WM_DRAWITEM"},
            new MsgInfoForDebug(){ nMsg =WM_DROPFILES, sMsg="WM_DROPFILES"},
            new MsgInfoForDebug(){ nMsg =WM_DWMCOLORIZATIONCOLORCHANGED, sMsg="WM_DWMCOLORIZATIONCOLORCHANGED"},
            new MsgInfoForDebug(){ nMsg =WM_DWMCOMPOSITIONCHANGED, sMsg="WM_DWMCOMPOSITIONCHANGED"},
            new MsgInfoForDebug(){ nMsg =WM_DWMNCRENDERINGCHANGED, sMsg="WM_DWMNCRENDERINGCHANGED"},
            new MsgInfoForDebug(){ nMsg =WM_DWMSENDICONICLIVEPREVIEWBITMAP, sMsg="WM_DWMSENDICONICLIVEPREVIEWBITMAP"},
            new MsgInfoForDebug(){ nMsg =WM_DWMSENDICONICTHUMBNAIL, sMsg="WM_DWMSENDICONICTHUMBNAIL"},
            new MsgInfoForDebug(){ nMsg =WM_DWMWINDOWMAXIMIZEDCHANGE, sMsg="WM_DWMWINDOWMAXIMIZEDCHANGE"},
            new MsgInfoForDebug(){ nMsg =WM_ENABLE, sMsg="WM_ENABLE"},
            new MsgInfoForDebug(){ nMsg =WM_ENDSESSION, sMsg="WM_ENDSESSION"},
            new MsgInfoForDebug(){ nMsg =WM_ENTERIDLE, sMsg="WM_ENTERIDLE"},
            new MsgInfoForDebug(){ nMsg =WM_ENTERMENULOOP, sMsg="WM_ENTERMENULOOP"},
            new MsgInfoForDebug(){ nMsg =WM_ENTERSIZEMOVE, sMsg="WM_ENTERSIZEMOVE"},
            new MsgInfoForDebug(){ nMsg =WM_ERASEBKGND, sMsg="WM_ERASEBKGND"},
            new MsgInfoForDebug(){ nMsg =WM_EXITMENULOOP, sMsg="WM_EXITMENULOOP"},
            new MsgInfoForDebug(){ nMsg =WM_EXITSIZEMOVE, sMsg="WM_EXITSIZEMOVE"},
            new MsgInfoForDebug(){ nMsg =WM_FONTCHANGE, sMsg="WM_FONTCHANGE"},
            new MsgInfoForDebug(){ nMsg =WM_GESTURE, sMsg="WM_GESTURE"},
            new MsgInfoForDebug(){ nMsg =WM_GESTURENOTIFY, sMsg="WM_GESTURENOTIFY"},
            new MsgInfoForDebug(){ nMsg =WM_GETDLGCODE, sMsg="WM_GETDLGCODE"},
            new MsgInfoForDebug(){ nMsg =WM_GETDPISCALEDSIZE, sMsg="WM_GETDPISCALEDSIZE"},
            new MsgInfoForDebug(){ nMsg =WM_GETFONT, sMsg="WM_GETFONT"},
            new MsgInfoForDebug(){ nMsg =WM_GETHOTKEY, sMsg="WM_GETHOTKEY"},
            new MsgInfoForDebug(){ nMsg =WM_GETICON, sMsg="WM_GETICON"},
            new MsgInfoForDebug(){ nMsg =WM_GETMINMAXINFO, sMsg="WM_GETMINMAXINFO"},
            new MsgInfoForDebug(){ nMsg =WM_GETOBJECT, sMsg="WM_GETOBJECT"},
            new MsgInfoForDebug(){ nMsg =WM_GETTEXT, sMsg="WM_GETTEXT"},
            new MsgInfoForDebug(){ nMsg =WM_GETTEXTLENGTH, sMsg="WM_GETTEXTLENGTH"},
            new MsgInfoForDebug(){ nMsg =WM_GETTITLEBARINFOEX, sMsg="WM_GETTITLEBARINFOEX"},
            new MsgInfoForDebug(){ nMsg =WM_HANDHELDFIRST, sMsg="WM_HANDHELDFIRST"},
            new MsgInfoForDebug(){ nMsg =WM_HANDHELDLAST, sMsg="WM_HANDHELDLAST"},
            new MsgInfoForDebug(){ nMsg =WM_HELP, sMsg="WM_HELP"},
            new MsgInfoForDebug(){ nMsg =WM_HOTKEY, sMsg="WM_HOTKEY"},
            new MsgInfoForDebug(){ nMsg =WM_HSCROLL, sMsg="WM_HSCROLL"},
            new MsgInfoForDebug(){ nMsg =WM_HSCROLLCLIPBOARD, sMsg="WM_HSCROLLCLIPBOARD"},
            new MsgInfoForDebug(){ nMsg =WM_ICONERASEBKGND, sMsg="WM_ICONERASEBKGND"},
            new MsgInfoForDebug(){ nMsg =WM_IME_CHAR, sMsg="WM_IME_CHAR"},
            new MsgInfoForDebug(){ nMsg =WM_IME_COMPOSITION, sMsg="WM_IME_COMPOSITION"},
            new MsgInfoForDebug(){ nMsg =WM_IME_COMPOSITIONFULL, sMsg="WM_IME_COMPOSITIONFULL"},
            new MsgInfoForDebug(){ nMsg =WM_IME_CONTROL, sMsg="WM_IME_CONTROL"},
            new MsgInfoForDebug(){ nMsg =WM_IME_ENDCOMPOSITION, sMsg="WM_IME_ENDCOMPOSITION"},
            new MsgInfoForDebug(){ nMsg =WM_IME_KEYDOWN, sMsg="WM_IME_KEYDOWN"},
            new MsgInfoForDebug(){ nMsg =WM_IME_KEYLAST, sMsg="WM_IME_KEYLAST"},
            new MsgInfoForDebug(){ nMsg =WM_IME_KEYUP, sMsg="WM_IME_KEYUP"},
            new MsgInfoForDebug(){ nMsg =WM_IME_NOTIFY, sMsg="WM_IME_NOTIFY"},
            new MsgInfoForDebug(){ nMsg =WM_IME_REQUEST, sMsg="WM_IME_REQUEST"},
            new MsgInfoForDebug(){ nMsg =WM_IME_SELECT, sMsg="WM_IME_SELECT"},
            new MsgInfoForDebug(){ nMsg =WM_IME_SETCONTEXT, sMsg="WM_IME_SETCONTEXT"},
            new MsgInfoForDebug(){ nMsg =WM_IME_STARTCOMPOSITION, sMsg="WM_IME_STARTCOMPOSITION"},
            new MsgInfoForDebug(){ nMsg =WM_INITDIALOG, sMsg="WM_INITDIALOG"},
            new MsgInfoForDebug(){ nMsg =WM_INITMENU, sMsg="WM_INITMENU"},
            new MsgInfoForDebug(){ nMsg =WM_INITMENUPOPUP, sMsg="WM_INITMENUPOPUP"},
            new MsgInfoForDebug(){ nMsg =WM_INPUT, sMsg="WM_INPUT"},
            new MsgInfoForDebug(){ nMsg =WM_INPUT_DEVICE_CHANGE, sMsg="WM_INPUT_DEVICE_CHANGE"},
            new MsgInfoForDebug(){ nMsg =WM_INPUTLANGCHANGE, sMsg="WM_INPUTLANGCHANGE"},
            new MsgInfoForDebug(){ nMsg =WM_INPUTLANGCHANGEREQUEST, sMsg="WM_INPUTLANGCHANGEREQUEST"},
            new MsgInfoForDebug(){ nMsg =WM_KEYDOWN, sMsg="WM_KEYDOWN"},
            new MsgInfoForDebug(){ nMsg =WM_KEYFIRST, sMsg="WM_KEYFIRST"},
            new MsgInfoForDebug(){ nMsg =WM_KEYLAST, sMsg="WM_KEYLAST"},
            new MsgInfoForDebug(){ nMsg =WM_KEYUP, sMsg="WM_KEYUP"},
            new MsgInfoForDebug(){ nMsg =WM_KILLFOCUS, sMsg="WM_KILLFOCUS"},
            new MsgInfoForDebug(){ nMsg =WM_LBUTTONDBLCLK, sMsg="WM_LBUTTONDBLCLK"},
            new MsgInfoForDebug(){ nMsg =WM_LBUTTONDOWN, sMsg="WM_LBUTTONDOWN"},
            new MsgInfoForDebug(){ nMsg =WM_LBUTTONUP, sMsg="WM_LBUTTONUP"},
            new MsgInfoForDebug(){ nMsg =WM_MBUTTONDBLCLK, sMsg="WM_MBUTTONDBLCLK"},
            new MsgInfoForDebug(){ nMsg =WM_MBUTTONDOWN, sMsg="WM_MBUTTONDOWN"},
            new MsgInfoForDebug(){ nMsg =WM_MBUTTONUP, sMsg="WM_MBUTTONUP"},
            new MsgInfoForDebug(){ nMsg =WM_MDIACTIVATE, sMsg="WM_MDIACTIVATE"},
            new MsgInfoForDebug(){ nMsg =WM_MDICASCADE, sMsg="WM_MDICASCADE"},
            new MsgInfoForDebug(){ nMsg =WM_MDICREATE, sMsg="WM_MDICREATE"},
            new MsgInfoForDebug(){ nMsg =WM_MDIDESTROY, sMsg="WM_MDIDESTROY"},
            new MsgInfoForDebug(){ nMsg =WM_MDIGETACTIVE, sMsg="WM_MDIGETACTIVE"},
            new MsgInfoForDebug(){ nMsg =WM_MDIICONARRANGE, sMsg="WM_MDIICONARRANGE"},
            new MsgInfoForDebug(){ nMsg =WM_MDIMAXIMIZE, sMsg="WM_MDIMAXIMIZE"},
            new MsgInfoForDebug(){ nMsg =WM_MDINEXT, sMsg="WM_MDINEXT"},
            new MsgInfoForDebug(){ nMsg =WM_MDIREFRESHMENU, sMsg="WM_MDIREFRESHMENU"},
            new MsgInfoForDebug(){ nMsg =WM_MDIRESTORE, sMsg="WM_MDIRESTORE"},
            new MsgInfoForDebug(){ nMsg =WM_MDISETMENU, sMsg="WM_MDISETMENU"},
            new MsgInfoForDebug(){ nMsg =WM_MDITILE, sMsg="WM_MDITILE"},
            new MsgInfoForDebug(){ nMsg =WM_MEASUREITEM, sMsg="WM_MEASUREITEM"},
            new MsgInfoForDebug(){ nMsg =WM_MENUCHAR, sMsg="WM_MENUCHAR"},
            new MsgInfoForDebug(){ nMsg =WM_MENUCOMMAND, sMsg="WM_MENUCOMMAND"},
            new MsgInfoForDebug(){ nMsg =WM_MENUDRAG, sMsg="WM_MENUDRAG"},
            new MsgInfoForDebug(){ nMsg =WM_MENUGETOBJECT, sMsg="WM_MENUGETOBJECT"},
            new MsgInfoForDebug(){ nMsg =WM_MENURBUTTONUP, sMsg="WM_MENURBUTTONUP"},
            new MsgInfoForDebug(){ nMsg =WM_MENUSELECT, sMsg="WM_MENUSELECT"},
            new MsgInfoForDebug(){ nMsg =WM_MOUSEACTIVATE, sMsg="WM_MOUSEACTIVATE"},
            new MsgInfoForDebug(){ nMsg =WM_MOUSEFIRST, sMsg="WM_MOUSEFIRST"},
            new MsgInfoForDebug(){ nMsg =WM_MOUSEHOVER, sMsg="WM_MOUSEHOVER"},
            new MsgInfoForDebug(){ nMsg =WM_MOUSEHWHEEL, sMsg="WM_MOUSEHWHEEL"},
            new MsgInfoForDebug(){ nMsg =WM_MOUSELAST, sMsg="WM_MOUSELAST"},
            new MsgInfoForDebug(){ nMsg =WM_MOUSELEAVE, sMsg="WM_MOUSELEAVE"},
            new MsgInfoForDebug(){ nMsg =WM_MOUSEMOVE, sMsg="WM_MOUSEMOVE"},
            new MsgInfoForDebug(){ nMsg =WM_MOUSEWHEEL, sMsg="WM_MOUSEWHEEL"},
            new MsgInfoForDebug(){ nMsg =WM_MOVE, sMsg="WM_MOVE"},
            new MsgInfoForDebug(){ nMsg =WM_MOVING, sMsg="WM_MOVING"},
            new MsgInfoForDebug(){ nMsg =WM_NCACTIVATE, sMsg="WM_NCACTIVATE"},
            new MsgInfoForDebug(){ nMsg =WM_NCCALCSIZE, sMsg="WM_NCCALCSIZE"},
            new MsgInfoForDebug(){ nMsg =WM_NCCREATE, sMsg="WM_NCCREATE"},
            new MsgInfoForDebug(){ nMsg =WM_NCDESTROY, sMsg="WM_NCDESTROY"},
            new MsgInfoForDebug(){ nMsg =WM_NCHITTEST, sMsg="WM_NCHITTEST"},
            new MsgInfoForDebug(){ nMsg =WM_NCLBUTTONDBLCLK, sMsg="WM_NCLBUTTONDBLCLK"},
            new MsgInfoForDebug(){ nMsg =WM_NCLBUTTONDOWN, sMsg="WM_NCLBUTTONDOWN"},
            new MsgInfoForDebug(){ nMsg =WM_NCLBUTTONUP, sMsg="WM_NCLBUTTONUP"},
            new MsgInfoForDebug(){ nMsg =WM_NCMBUTTONDBLCLK, sMsg="WM_NCMBUTTONDBLCLK"},
            new MsgInfoForDebug(){ nMsg =WM_NCMBUTTONDOWN, sMsg="WM_NCMBUTTONDOWN"},
            new MsgInfoForDebug(){ nMsg =WM_NCMBUTTONUP, sMsg="WM_NCMBUTTONUP"},
            new MsgInfoForDebug(){ nMsg =WM_NCMOUSEHOVER, sMsg="WM_NCMOUSEHOVER"},
            new MsgInfoForDebug(){ nMsg =WM_NCMOUSELEAVE, sMsg="WM_NCMOUSELEAVE"},
            new MsgInfoForDebug(){ nMsg =WM_NCMOUSEMOVE, sMsg="WM_NCMOUSEMOVE"},
            new MsgInfoForDebug(){ nMsg =WM_NCPAINT, sMsg="WM_NCPAINT"},
            new MsgInfoForDebug(){ nMsg =WM_NCPOINTERDOWN, sMsg="WM_NCPOINTERDOWN"},
            new MsgInfoForDebug(){ nMsg =WM_NCPOINTERUP, sMsg="WM_NCPOINTERUP"},
            new MsgInfoForDebug(){ nMsg =WM_NCPOINTERUPDATE, sMsg="WM_NCPOINTERUPDATE"},
            new MsgInfoForDebug(){ nMsg =WM_NCRBUTTONDBLCLK, sMsg="WM_NCRBUTTONDBLCLK"},
            new MsgInfoForDebug(){ nMsg =WM_NCRBUTTONDOWN, sMsg="WM_NCRBUTTONDOWN"},
            new MsgInfoForDebug(){ nMsg =WM_NCRBUTTONUP, sMsg="WM_NCRBUTTONUP"},
            new MsgInfoForDebug(){ nMsg =WM_NCXBUTTONDBLCLK, sMsg="WM_NCXBUTTONDBLCLK"},
            new MsgInfoForDebug(){ nMsg =WM_NCXBUTTONDOWN, sMsg="WM_NCXBUTTONDOWN"},
            new MsgInfoForDebug(){ nMsg =WM_NCXBUTTONUP, sMsg="WM_NCXBUTTONUP"},
            new MsgInfoForDebug(){ nMsg =WM_NEXTDLGCTL, sMsg="WM_NEXTDLGCTL"},
            new MsgInfoForDebug(){ nMsg =WM_NEXTMENU, sMsg="WM_NEXTMENU"},
            new MsgInfoForDebug(){ nMsg =WM_NOTIFY, sMsg="WM_NOTIFY"},
            new MsgInfoForDebug(){ nMsg =WM_NOTIFYFORMAT, sMsg="WM_NOTIFYFORMAT"},
            new MsgInfoForDebug(){ nMsg =WM_PAINT, sMsg="WM_PAINT"},
            new MsgInfoForDebug(){ nMsg =WM_PAINTCLIPBOARD, sMsg="WM_PAINTCLIPBOARD"},
            new MsgInfoForDebug(){ nMsg =WM_PAINTICON, sMsg="WM_PAINTICON"},
            new MsgInfoForDebug(){ nMsg =WM_PALETTECHANGED, sMsg="WM_PALETTECHANGED"},
            new MsgInfoForDebug(){ nMsg =WM_PALETTEISCHANGING, sMsg="WM_PALETTEISCHANGING"},
            new MsgInfoForDebug(){ nMsg =WM_PARENTNOTIFY, sMsg="WM_PARENTNOTIFY"},
            new MsgInfoForDebug(){ nMsg =WM_PASTE, sMsg="WM_PASTE"},
            new MsgInfoForDebug(){ nMsg =WM_PENWINFIRST, sMsg="WM_PENWINFIRST"},
            new MsgInfoForDebug(){ nMsg =WM_PENWINLAST, sMsg="WM_PENWINLAST"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERACTIVATE, sMsg="WM_POINTERACTIVATE"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERCAPTURECHANGED, sMsg="WM_POINTERCAPTURECHANGED"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERDEVICECHANGE, sMsg="WM_POINTERDEVICECHANGE"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERDEVICEINRANGE, sMsg="WM_POINTERDEVICEINRANGE"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERDEVICEOUTOFRANGE, sMsg="WM_POINTERDEVICEOUTOFRANGE"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERDOWN, sMsg="WM_POINTERDOWN"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERENTER, sMsg="WM_POINTERENTER"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERHWHEEL, sMsg="WM_POINTERHWHEEL"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERLEAVE, sMsg="WM_POINTERLEAVE"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERROUTEDAWAY, sMsg="WM_POINTERROUTEDAWAY"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERROUTEDRELEASED, sMsg="WM_POINTERROUTEDRELEASED"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERROUTEDTO, sMsg="WM_POINTERROUTEDTO"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERUP, sMsg="WM_POINTERUP"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERUPDATE, sMsg="WM_POINTERUPDATE"},
            new MsgInfoForDebug(){ nMsg =WM_POINTERWHEEL, sMsg="WM_POINTERWHEEL"},
            new MsgInfoForDebug(){ nMsg =WM_POWER, sMsg="WM_POWER"},
            new MsgInfoForDebug(){ nMsg =WM_POWERBROADCAST, sMsg="WM_POWERBROADCAST"},
            new MsgInfoForDebug(){ nMsg =WM_PRINT, sMsg="WM_PRINT"},
            new MsgInfoForDebug(){ nMsg =WM_PRINTCLIENT, sMsg="WM_PRINTCLIENT"},
            new MsgInfoForDebug(){ nMsg =WM_QUERYDRAGICON, sMsg="WM_QUERYDRAGICON"},
            new MsgInfoForDebug(){ nMsg =WM_QUERYENDSESSION, sMsg="WM_QUERYENDSESSION"},
            new MsgInfoForDebug(){ nMsg =WM_QUERYNEWPALETTE, sMsg="WM_QUERYNEWPALETTE"},
            new MsgInfoForDebug(){ nMsg =WM_QUERYOPEN, sMsg="WM_QUERYOPEN"},
            new MsgInfoForDebug(){ nMsg =WM_QUERYUISTATE, sMsg="WM_QUERYUISTATE"},
            new MsgInfoForDebug(){ nMsg =WM_QUEUESYNC, sMsg="WM_QUEUESYNC"},
            new MsgInfoForDebug(){ nMsg =WM_QUIT, sMsg="WM_QUIT"},
            new MsgInfoForDebug(){ nMsg =WM_RBUTTONDBLCLK, sMsg="WM_RBUTTONDBLCLK"},
            new MsgInfoForDebug(){ nMsg =WM_RBUTTONDOWN, sMsg="WM_RBUTTONDOWN"},
            new MsgInfoForDebug(){ nMsg =WM_RBUTTONUP, sMsg="WM_RBUTTONUP"},
            new MsgInfoForDebug(){ nMsg =WM_RENDERALLFORMATS, sMsg="WM_RENDERALLFORMATS"},
            new MsgInfoForDebug(){ nMsg =WM_RENDERFORMAT, sMsg="WM_RENDERFORMAT"},
            new MsgInfoForDebug(){ nMsg =WM_SETCURSOR, sMsg="WM_SETCURSOR"},
            new MsgInfoForDebug(){ nMsg =WM_SETFOCUS, sMsg="WM_SETFOCUS"},
            new MsgInfoForDebug(){ nMsg =WM_SETFONT, sMsg="WM_SETFONT"},
            new MsgInfoForDebug(){ nMsg =WM_SETHOTKEY, sMsg="WM_SETHOTKEY"},
            new MsgInfoForDebug(){ nMsg =WM_SETICON, sMsg="WM_SETICON"},
            new MsgInfoForDebug(){ nMsg =WM_SETREDRAW, sMsg="WM_SETREDRAW"},
            new MsgInfoForDebug(){ nMsg =WM_SETTEXT, sMsg="WM_SETTEXT"},
            new MsgInfoForDebug(){ nMsg =WM_SHOWWINDOW, sMsg="WM_SHOWWINDOW"},
            new MsgInfoForDebug(){ nMsg =WM_SIZE, sMsg="WM_SIZE"},
            new MsgInfoForDebug(){ nMsg =WM_SIZECLIPBOARD, sMsg="WM_SIZECLIPBOARD"},
            new MsgInfoForDebug(){ nMsg =WM_SIZING, sMsg="WM_SIZING"},
            new MsgInfoForDebug(){ nMsg =WM_SPOOLERSTATUS, sMsg="WM_SPOOLERSTATUS"},
            new MsgInfoForDebug(){ nMsg =WM_STYLECHANGED, sMsg="WM_STYLECHANGED"},
            new MsgInfoForDebug(){ nMsg =WM_STYLECHANGING, sMsg="WM_STYLECHANGING"},
            new MsgInfoForDebug(){ nMsg =WM_SYNCPAINT, sMsg="WM_SYNCPAINT"},
            new MsgInfoForDebug(){ nMsg =WM_SYSCHAR, sMsg="WM_SYSCHAR"},
            new MsgInfoForDebug(){ nMsg =WM_SYSCOLORCHANGE, sMsg="WM_SYSCOLORCHANGE"},
            new MsgInfoForDebug(){ nMsg =WM_SYSCOMMAND, sMsg="WM_SYSCOMMAND"},
            new MsgInfoForDebug(){ nMsg =WM_SYSDEADCHAR, sMsg="WM_SYSDEADCHAR"},
            new MsgInfoForDebug(){ nMsg =WM_SYSKEYDOWN, sMsg="WM_SYSKEYDOWN"},
            new MsgInfoForDebug(){ nMsg =WM_SYSKEYUP, sMsg="WM_SYSKEYUP"},
            new MsgInfoForDebug(){ nMsg =WM_TABLET_FIRST, sMsg="WM_TABLET_FIRST"},
            new MsgInfoForDebug(){ nMsg =WM_TABLET_LAST, sMsg="WM_TABLET_LAST"},
            new MsgInfoForDebug(){ nMsg =WM_TCARD, sMsg="WM_TCARD"},
            new MsgInfoForDebug(){ nMsg =WM_THEMECHANGED, sMsg="WM_THEMECHANGED"},
            new MsgInfoForDebug(){ nMsg =WM_TIMECHANGE, sMsg="WM_TIMECHANGE"},
            new MsgInfoForDebug(){ nMsg =WM_TIMER, sMsg="WM_TIMER"},
            new MsgInfoForDebug(){ nMsg =WM_TOUCH, sMsg="WM_TOUCH"},
            new MsgInfoForDebug(){ nMsg =WM_TOUCHHITTESTING, sMsg="WM_TOUCHHITTESTING"},
            new MsgInfoForDebug(){ nMsg =WM_UNDO, sMsg="WM_UNDO"},
            new MsgInfoForDebug(){ nMsg =WM_UNICHAR, sMsg="WM_UNICHAR"},
            new MsgInfoForDebug(){ nMsg =WM_UNINITMENUPOPUP, sMsg="WM_UNINITMENUPOPUP"},
            new MsgInfoForDebug(){ nMsg =WM_UPDATEUISTATE, sMsg="WM_UPDATEUISTATE"},
            new MsgInfoForDebug(){ nMsg =WM_USER, sMsg="WM_USER"},
            new MsgInfoForDebug(){ nMsg =WM_USERCHANGED, sMsg="WM_USERCHANGED"},
            new MsgInfoForDebug(){ nMsg =WM_VKEYTOITEM, sMsg="WM_VKEYTOITEM"},
            new MsgInfoForDebug(){ nMsg =WM_VSCROLL, sMsg="WM_VSCROLL"},
            new MsgInfoForDebug(){ nMsg =WM_VSCROLLCLIPBOARD, sMsg="WM_VSCROLLCLIPBOARD"},
            new MsgInfoForDebug(){ nMsg =WM_WINDOWPOSCHANGED, sMsg="WM_WINDOWPOSCHANGED"},
            new MsgInfoForDebug(){ nMsg =WM_WINDOWPOSCHANGING, sMsg="WM_WINDOWPOSCHANGING"},
            new MsgInfoForDebug(){ nMsg =WM_WININICHANGE, sMsg="WM_WININICHANGE"},
            new MsgInfoForDebug(){ nMsg =WM_WTSSESSION_CHANGE, sMsg="WM_WTSSESSION_CHANGE"},
            new MsgInfoForDebug(){ nMsg =WM_XBUTTONDBLCLK, sMsg="WM_XBUTTONDBLCLK"},
            new MsgInfoForDebug(){ nMsg =WM_XBUTTONDOWN, sMsg="WM_XBUTTONDOWN"},
            new MsgInfoForDebug(){ nMsg =WM_XBUTTONUP, sMsg="WM_XBUTTONUP"},
        }.GroupBy(item => item.nMsg).ToDictionary(item => item.Key, item => item.First().sMsg);

        public static string GetMsgAsStr(uint msg)
        {
            if (dictMsgInfoForDebug.ContainsKey(msg))
                return dictMsgInfoForDebug[msg];
            else
                return $"UnknownMsg 0x{msg:X8}";
        }

        public static void TraceMessageDetail(string sCategory, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            return;
            switch (msg)
            {
                case WM_CREATE:
                case WM_ACTIVATE:
                case WM_ACTIVATEAPP:
                case WM_CLOSE:
                case WM_DESTROY:
                case WM_PAINT:
                case WM_ERASEBKGND:
                case WM_IME_CHAR:
                case WM_IME_COMPOSITIONFULL:
                case WM_IME_CONTROL:
                case WM_IME_REQUEST:
                case WM_IME_NOTIFY:
                case WM_IME_KEYDOWN:
                case WM_IME_SETCONTEXT:
                case WM_IME_STARTCOMPOSITION:
                case WM_IME_COMPOSITION:
                case WM_IME_ENDCOMPOSITION:
                case WM_IME_SELECT:
                case WM_IME_KEYUP:
                    Debug.WriteLine($"{GetWindowsInfo(hWnd, sCategory, GetMsgAsStr(msg))}.");
                    break;
                case WM_CHAR:
                case WM_KEYDOWN:
                case WM_SYSKEYDOWN:
                    Debug.WriteLine($"{sCategory}::{GetWindowsInfo(hWnd, sCategory, GetMsgAsStr(msg))}: value:{wParam}, VKey:{(System.Windows.Input.Key)wParam}, CHAR:{(char)wParam}");
                    break;
                case WM_KEYUP:
                    break;
                case WM_LBUTTONDOWN:
                case WM_RBUTTONDOWN:
                case WM_MBUTTONDOWN:
                    Debug.WriteLine($"{sCategory}::{GetWindowsInfo(hWnd, sCategory, GetMsgAsStr(msg))}: {GetKeyStateStr(wParam.ToInt32())} ,X:{(lParam.ToInt32() & 0xFFFF)}, Y:{(lParam.ToInt32() >> 16)}");
                    break;
                case WM_MOUSEMOVE:
                case WM_LBUTTONUP:
                case WM_RBUTTONUP:
                case WM_MBUTTONUP:
                    break;
                case WM_SIZE:
                    Debug.WriteLine($"{sCategory}::{GetWindowsInfo(hWnd, sCategory, GetMsgAsStr(msg))}: {GetKeyStateStr(wParam.ToInt32())} ,cx:{lParam.ToInt32() & 0xFFFF}, cy:{(lParam.ToInt32() >> 16) & 0xFFFF}");
                    break;
            }
        }
    }
}
