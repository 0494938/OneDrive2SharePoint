using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace BaseUtil
{
    #region Delegation
    public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    public delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);
    public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
    #endregion Delegation

    public partial class GcjWinApi
    {
        // 定义 Windows API 函数
        #region Const
        public const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
        public const uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;

        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // Large icon
        public const uint SHGFI_SMALLICON = 0x1; // Small icon
        
        // WinEvent constants for creating and destroying windows
        public const uint EVENT_OBJECT_CREATE = 0x8000;
        public const uint EVENT_OBJECT_DESTROY = 0x8001;

        public const uint WINEVENT_OUTOFCONTEXT = 0x0000;
        public const int OBJID_WINDOW = 0x00000000;

        public const int NULL_BRUSH = 5;
        public const int WH_GETMESSAGE = 3;
        public const int WH_CALLWNDPROC = 4;

        public const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        public const int DWMWCP_ROUND = 0x00000001;
        public const int DWMWCP_ROUNDSMALL = 0x00000002;


        public const int WM_CREATE = 0x0001;
        public const int WM_DESTROY = 0x0002;
        public const int WM_MOVE = 0x0003;
        public const int WM_SIZE = 0x0005;
        public const int WM_ACTIVATE = 0x0006;
        public const int WM_PAINT = 0x000F;
        public const int WM_CLOSE = 0x0010;
        public const int WM_ENABLE = 0x000A;

        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CHAR = 0x0102;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SETFOCUS = 0x0007;
        public const int WM_KILLFOCUS = 0x0008;

        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;

        public const int WM_RBUTTONDBLCLK = 0x0206;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;

        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_MBUTTONDOWN = 0x0207;
        public const int WM_MBUTTONUP = 0x0208;
        public const int WM_MBUTTONDBLCLK = 0x0209;
        public const int WM_MOUSEHOVER = 0x02A1;
        public const int WM_MOUSELEAVE = 0x02A3;

        public const int WM_QUERYENDSESSION = 0x0011; // 系统关机或注销的消息
        public const int WM_ENDSESSION = 0x0016;      // 系统确认关机或注销
        public const int WM_USER = 0x0400;

        public const int WM_SETREDRAW = 0x000B;
        public const int WM_SETTEXT = 0x000C;
        public const int WM_GETTEXT = 0x000D;
        public const int WM_GETTEXTLENGTH = 0x000E;
        public const int WM_QUERYOPEN = 0x0013;
        public const int WM_QUIT = 0x0012;
        public const int WM_ERASEBKGND = 0x0014;
        public const int WM_SYSCOLORCHANGE = 0x0015;
        public const int WM_SHOWWINDOW = 0x0018;
        public const int WM_WININICHANGE = 0x001A;
        public const int WM_DEVMODECHANGE = 0x001B;
        public const int WM_ACTIVATEAPP = 0x001C;
        public const int WM_FONTCHANGE = 0x001D;
        public const int WM_TIMECHANGE = 0x001E;
        public const int WM_CANCELMODE = 0x001F;
        public const int WM_SETCURSOR = 0x0020;
        public const int WM_MOUSEACTIVATE = 0x0021;
        public const int WM_CHILDACTIVATE = 0x0022;
        public const int WM_QUEUESYNC = 0x0023;
        public const int WM_GETMINMAXINFO = 0x0024;
        public const int WM_PAINTICON = 0x0026;
        public const int WM_ICONERASEBKGND = 0x0027;
        public const int WM_NEXTDLGCTL = 0x0028;
        public const int WM_SPOOLERSTATUS = 0x002A;
        public const int WM_DRAWITEM = 0x002B;
        public const int WM_MEASUREITEM = 0x002C;
        public const int WM_DELETEITEM = 0x002D;
        public const int WM_VKEYTOITEM = 0x002E;
        public const int WM_CHARTOITEM = 0x002F;
        public const int WM_SETFONT = 0x0030;
        public const int WM_GETFONT = 0x0031;
        public const int WM_SETHOTKEY = 0x0032;
        public const int WM_GETHOTKEY = 0x0033;
        public const int WM_QUERYDRAGICON = 0x0037;
        public const int WM_COMPAREITEM = 0x0039;
        public const int WM_GETOBJECT = 0x003D;
        public const int WM_COMPACTING = 0x0041;
        public const int WM_COMMNOTIFY = 0x0044;

        public const int WM_WINDOWPOSCHANGING = 0x0046;
        public const int WM_WINDOWPOSCHANGED = 0x0047;
        public const int WM_POWER = 0x0048;

        public const int WM_COPYDATA = 0x004A;
        public const int WM_CANCELJOURNAL = 0x004B;
        public const int WM_NOTIFY = 0x004E;
        public const int WM_INPUTLANGCHANGEREQUEST = 0x0050;
        public const int WM_INPUTLANGCHANGE = 0x0051;
        public const int WM_TCARD = 0x0052;
        public const int WM_HELP = 0x0053;
        public const int WM_USERCHANGED = 0x0054;
        public const int WM_NOTIFYFORMAT = 0x0055;

        public const int WM_CONTEXTMENU = 0x007B;
        public const int WM_STYLECHANGING = 0x007C;
        public const int WM_STYLECHANGED = 0x007D;
        public const int WM_DISPLAYCHANGE = 0x007E;
        public const int WM_GETICON = 0x007F;
        public const int WM_SETICON = 0x0080;
        public const int WM_NCCREATE = 0x0081;
        public const int WM_NCDESTROY = 0x0082;
        public const int WM_NCCALCSIZE = 0x0083;
        public const int WM_NCHITTEST = 0x0084;
        public const int WM_NCPAINT = 0x0085;
        public const int WM_NCACTIVATE = 0x0086;
        public const int WM_GETDLGCODE = 0x0087;
        public const int WM_SYNCPAINT = 0x0088;
        public const int WM_NCMOUSEMOVE = 0x00A0;
        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int WM_NCLBUTTONUP = 0x00A2;
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;
        public const int WM_NCRBUTTONDOWN = 0x00A4;
        public const int WM_NCRBUTTONUP = 0x00A5;
        public const int WM_NCRBUTTONDBLCLK = 0x00A6;
        public const int WM_NCMBUTTONDOWN = 0x00A7;
        public const int WM_NCMBUTTONUP = 0x00A8;
        public const int WM_NCMBUTTONDBLCLK = 0x00A9;
        public const int WM_NCXBUTTONDOWN = 0x00AB;
        public const int WM_NCXBUTTONUP = 0x00AC;
        public const int WM_NCXBUTTONDBLCLK = 0x00AD;
        public const int WM_INPUT_DEVICE_CHANGE = 0x00FE;
        public const int WM_INPUT = 0x00FF;
        public const int WM_KEYFIRST = 0x0100;
        public const int WM_DEADCHAR = 0x0103;
        public const int WM_SYSKEYUP = 0x0105;
        public const int WM_SYSCHAR = 0x0106;
        public const int WM_SYSDEADCHAR = 0x0107;
        public const int WM_UNICHAR = 0x0109;
        public const int WM_KEYLAST = 0x0109;
        public const int WM_IME_STARTCOMPOSITION = 0x010D;
        public const int WM_IME_ENDCOMPOSITION = 0x010E;
        public const int WM_IME_COMPOSITION = 0x010F;
        public const int WM_IME_KEYLAST = 0x010F;
        public const int WM_INITDIALOG = 0x0110;
        public const int WM_COMMAND = 0x0111;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int WM_TIMER = 0x0113;
        public const int WM_HSCROLL = 0x0114;
        public const int WM_VSCROLL = 0x0115;
        public const int WM_INITMENU = 0x0116;
        public const int WM_INITMENUPOPUP = 0x0117;
        public const int WM_GESTURE = 0x0119;
        public const int WM_GESTURENOTIFY = 0x011A;
        public const int WM_MENUSELECT = 0x011F;
        public const int WM_MENUCHAR = 0x0120;
        public const int WM_ENTERIDLE = 0x0121;
        public const int WM_MENURBUTTONUP = 0x0122;
        public const int WM_MENUDRAG = 0x0123;
        public const int WM_MENUGETOBJECT = 0x0124;
        public const int WM_UNINITMENUPOPUP = 0x0125;
        public const int WM_MENUCOMMAND = 0x0126;
        public const int WM_CHANGEUISTATE = 0x0127;
        public const int WM_UPDATEUISTATE = 0x0128;
        public const int WM_QUERYUISTATE = 0x0129;

        public const int WM_CTLCOLORMSGBOX = 0x0132;
        public const int WM_CTLCOLOREDIT = 0x0133;
        public const int WM_CTLCOLORLISTBOX = 0x0134;
        public const int WM_CTLCOLORBTN = 0x0135;
        public const int WM_CTLCOLORDLG = 0x0136;
        public const int WM_CTLCOLORSCROLLBAR = 0x0137;
        public const int WM_CTLCOLORSTATIC = 0x0138;
        public const int WM_MOUSEFIRST = 0x0200;
        public const int WM_XBUTTONDOWN = 0x020B;
        public const int WM_XBUTTONUP = 0x020C;
        public const int WM_XBUTTONDBLCLK = 0x020D;
        public const int WM_MOUSEHWHEEL = 0x020E;
        public const int WM_MOUSELAST = 0x020E;
        public const int WM_PARENTNOTIFY = 0x0210;
        public const int WM_ENTERMENULOOP = 0x0211;
        public const int WM_EXITMENULOOP = 0x0212;
        public const int WM_NEXTMENU = 0x0213;
        public const int WM_SIZING = 0x0214;
        public const int WM_CAPTURECHANGED = 0x0215;
        public const int WM_MOVING = 0x0216;
        public const int WM_POWERBROADCAST = 0x0218;
        public const int WM_DEVICECHANGE = 0x0219;
        public const int WM_MDICREATE = 0x0220;
        public const int WM_MDIDESTROY = 0x0221;
        public const int WM_MDIACTIVATE = 0x0222;
        public const int WM_MDIRESTORE = 0x0223;
        public const int WM_MDINEXT = 0x0224;
        public const int WM_MDIMAXIMIZE = 0x0225;
        public const int WM_MDITILE = 0x0226;
        public const int WM_MDICASCADE = 0x0227;
        public const int WM_MDIICONARRANGE = 0x0228;
        public const int WM_MDIGETACTIVE = 0x0229;
        public const int WM_MDISETMENU = 0x0230;
        public const int WM_ENTERSIZEMOVE = 0x0231;
        public const int WM_EXITSIZEMOVE = 0x0232;
        public const int WM_DROPFILES = 0x0233;
        public const int WM_MDIREFRESHMENU = 0x0234;
        public const int WM_POINTERDEVICECHANGE = 0x238;
        public const int WM_POINTERDEVICEINRANGE = 0x239;
        public const int WM_POINTERDEVICEOUTOFRANGE = 0x23A;
        public const int WM_TOUCH = 0x0240;
        public const int WM_NCPOINTERUPDATE = 0x0241;
        public const int WM_NCPOINTERDOWN = 0x0242;
        public const int WM_NCPOINTERUP = 0x0243;
        public const int WM_POINTERUPDATE = 0x0245;
        public const int WM_POINTERDOWN = 0x0246;
        public const int WM_POINTERUP = 0x0247;
        public const int WM_POINTERENTER = 0x0249;
        public const int WM_POINTERLEAVE = 0x024A;
        public const int WM_POINTERACTIVATE = 0x024B;
        public const int WM_POINTERCAPTURECHANGED = 0x024C;
        public const int WM_TOUCHHITTESTING = 0x024D;
        public const int WM_POINTERWHEEL = 0x024E;
        public const int WM_POINTERHWHEEL = 0x024F;
        public const int WM_POINTERROUTEDTO = 0x0251;
        public const int WM_POINTERROUTEDAWAY = 0x0252;
        public const int WM_POINTERROUTEDRELEASED = 0x0253;
        public const int WM_IME_SETCONTEXT = 0x0281;
        public const int WM_IME_NOTIFY = 0x0282;
        public const int WM_IME_CONTROL = 0x0283;
        public const int WM_IME_COMPOSITIONFULL = 0x0284;
        public const int WM_IME_SELECT = 0x0285;
        public const int WM_IME_CHAR = 0x0286;
        public const int WM_IME_REQUEST = 0x0288;
        public const int WM_IME_KEYDOWN = 0x0290;
        public const int WM_IME_KEYUP = 0x0291;
        public const int WM_NCMOUSEHOVER = 0x02A0;
        public const int WM_NCMOUSELEAVE = 0x02A2;
        public const int WM_WTSSESSION_CHANGE = 0x02B1;
        public const int WM_TABLET_FIRST = 0x02c0;
        public const int WM_TABLET_LAST = 0x02df;
        public const int WM_DPICHANGED = 0x02E0;
        public const int WM_DPICHANGED_BEFOREPARENT = 0x02E2;
        public const int WM_DPICHANGED_AFTERPARENT = 0x02E3;
        public const int WM_GETDPISCALEDSIZE = 0x02E4;
        public const int WM_CUT = 0x0300;
        public const int WM_COPY = 0x0301;
        public const int WM_PASTE = 0x0302;
        public const int WM_CLEAR = 0x0303;
        public const int WM_UNDO = 0x0304;
        public const int WM_RENDERFORMAT = 0x0305;
        public const int WM_RENDERALLFORMATS = 0x0306;
        public const int WM_DESTROYCLIPBOARD = 0x0307;
        public const int WM_DRAWCLIPBOARD = 0x0308;
        public const int WM_PAINTCLIPBOARD = 0x0309;
        public const int WM_VSCROLLCLIPBOARD = 0x030A;
        public const int WM_SIZECLIPBOARD = 0x030B;
        public const int WM_ASKCBFORMATNAME = 0x030C;
        public const int WM_CHANGECBCHAIN = 0x030D;
        public const int WM_HSCROLLCLIPBOARD = 0x030E;
        public const int WM_QUERYNEWPALETTE = 0x030F;
        public const int WM_PALETTEISCHANGING = 0x0310;
        public const int WM_PALETTECHANGED = 0x0311;
        public const int WM_HOTKEY = 0x0312;
        public const int WM_PRINT = 0x0317;
        public const int WM_PRINTCLIENT = 0x0318;
        public const int WM_APPCOMMAND = 0x0319;
        public const int WM_THEMECHANGED = 0x031A;
        public const int WM_CLIPBOARDUPDATE = 0x031D;
        public const int WM_DWMCOMPOSITIONCHANGED = 0x031E;
        public const int WM_DWMNCRENDERINGCHANGED = 0x031F;
        public const int WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320;
        public const int WM_DWMWINDOWMAXIMIZEDCHANGE = 0x0321;
        public const int WM_DWMSENDICONICTHUMBNAIL = 0x0323;
        public const int WM_DWMSENDICONICLIVEPREVIEWBITMAP = 0x0326;
        public const int WM_GETTITLEBARINFOEX = 0x033F;
        public const int WM_HANDHELDFIRST = 0x0358;
        public const int WM_HANDHELDLAST = 0x035F;
        public const int WM_AFXFIRST = 0x0360;
        public const int WM_AFXLAST = 0x037F;
        public const int WM_PENWINFIRST = 0x0380;
        public const int WM_PENWINLAST = 0x038F;
        public const int WM_APP = 0x8000;

        public const int WS_CHILD = 0x40000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WS_BORDER = 0x00800000;
        public const int WS_HSCROLL = 0x00100000;
        public const int WS_VSCROLL = 0x00200000;
        public const int WS_EX_CLIENTEDGE = 0x00000200;
        public const int WS_OVERLAPPED = 0x00000000;
        public const uint WS_POPUP = 0x80000000;
        public const int WS_MINIMIZE = 0x20000000;

        public const int WS_DISABLED = 0x08000000;
        public const int WS_CLIPSIBLINGS = 0x04000000;
        public const int WS_CLIPCHILDREN = 0x02000000;
        public const int WS_MAXIMIZE = 0x01000000;
        public const int WS_CAPTION = 0x00C00000;     /* WS_BORDER | WS_DLGFRAME  */
        public const int WS_DLGFRAME = 0x00400000;
        public const int WS_SYSMENU = 0x00080000;
        public const int WS_THICKFRAME = 0x00040000;
        public const int WS_GROUP = 0x00020000;
        public const int WS_TABSTOP = 0x00010000;

        public const int WS_MINIMIZEBOX = 0x00020000;
        public const int WS_MAXIMIZEBOX = 0x00010000;


        public const int WS_TILED = WS_OVERLAPPED;
        public const int WS_ICONIC = WS_MINIMIZE;
        public const int WS_SIZEBOX = WS_THICKFRAME;
        public const int WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;


        public const int WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);

        public const uint WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU);

        public const int WS_CHILDWINDOW = (WS_CHILD);

        public const long WS_EX_DLGMODALFRAME = 0x00000001L;
        public const long WS_EX_NOPARENTNOTIFY = 0x00000004L;
        public const long WS_EX_TOPMOST = 0x00000008L;
        public const long WS_EX_ACCEPTFILES = 0x00000010L;
        public const long WS_EX_TRANSPARENT = 0x00000020L;

        public const long WS_EX_MDICHILD = 0x00000040L;
        public const long WS_EX_TOOLWINDOW = 0x00000080L;
        public const long WS_EX_WINDOWEDGE = 0x00000100L;
        public const long WS_EX_CONTEXTHELP = 0x00000400L;

        public const long WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
        public const long WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);

        public const int WS_EX_LAYERED = 0x00080000;
        public const long WS_EX_NOINHERITLAYOUT = 0x00100000L; // Disable inheritence of mirroring by children
        public const long WS_EX_NOREDIRECTIONBITMAP = 0x00200000L;
        public const long WS_EX_LAYOUTRTL = 0x00400000L;// Right to left mirroring
        public const long WS_EX_COMPOSITED = 0x02000000L;
        public const long WS_EX_NOACTIVATE = 0x08000000L;

        public const int ES_LEFT = 0x0000;
        public const int ES_CENTER = 0x0001;
        public const int ES_RIGHT = 0x0002;
        public const int ES_MULTILINE = 0x0004;
        public const int ES_UPPERCASE = 0x0008;
        public const int ES_LOWERCASE = 0x0010;
        public const int ES_PASSWORD = 0x0020;
        public const int ES_AUTOVSCROLL = 0x0040;
        public const int ES_AUTOHSCROLL = 0x0080;
        public const int ES_NOHIDESEL = 0x0100;
        public const int ES_OEMCONVERT = 0x0400;
        public const int ES_READONLY = 0x0800;
        public const int ES_WANTRETURN = 0x1000;
        public const int ES_NUMBER = 0x2000;
        public const string IDC_ARROW = "32512";
        public const int GWL_WNDPROC = -4;
        public const int TRANSPARENT = 1;
        public const int OPAQUE = 2;
        public const int BKMODE_LAST = 2;

        public const int UNICODE_NOCHAR = 0xFFFF;
        public const int UISF_HIDEFOCUS = 0x1;
        public const int UISF_HIDEACCEL = 0x2;
        public const int UISF_ACTIVE = 0x4;
        public const int MN_GETHMENU = 0x01E1;
        public const int XBUTTON1 = 0x0001;
        public const int XBUTTON2 = 0x0002;
        public const int PBT_APMQUERYSUSPEND = 0x0000;
        public const int PBT_APMQUERYSTANDBY = 0x0001;
        public const int PBT_APMQUERYSUSPENDFAILED = 0x0002;
        public const int PBT_APMQUERYSTANDBYFAILED = 0x0003;
        public const int PBT_APMSUSPEND = 0x0004;
        public const int PBT_APMSTANDBY = 0x0005;
        public const int PBT_APMRESUMECRITICAL = 0x0006;
        public const int PBT_APMRESUMESUSPEND = 0x0007;
        public const int PBT_APMRESUMESTANDBY = 0x0008;
        public const int PBTF_APMRESUMEFROMFAILURE = 0x00000001;
        public const int PBT_APMBATTERYLOW = 0x0009;
        public const int PBT_APMPOWERSTATUSCHANGE = 0x000A;
        public const int PBT_APMOEMEVENT = 0x000B;
        public const int PBT_APMRESUMEAUTOMATIC = 0x0012;
        public const int PBT_POWERSETTINGCHANGE = 0x8013;
        public const int DM_POINTERHITTEST = 0x0250;

        public const int WVR_ALIGNTOP = 0x0010;
        public const int WVR_ALIGNLEFT = 0x0020;
        public const int WVR_ALIGNBOTTOM = 0x0040;
        public const int WVR_ALIGNRIGHT = 0x0080;
        public const int WVR_HREDRAW = 0x0100;
        public const int WVR_VREDRAW = 0x0200;
        //public const int WVR_REDRAW  (WVR_HREDRAW |;
        public const int WVR_VALIDRECTS = 0x0400;
        public const int MK_LBUTTON = 0x0001;
        public const int MK_RBUTTON = 0x0002;
        public const int MK_SHIFT = 0x0004;
        public const int MK_CONTROL = 0x0008;
        public const int MK_MBUTTON = 0x0010;
        public const int MK_XBUTTON1 = 0x0020;
        public const int MK_XBUTTON2 = 0x0040;
        public const int TME_HOVER = 0x00000001;
        public const int TME_LEAVE = 0x00000002;
        public const int TME_NONCLIENT = 0x00000010;
        public const int TME_QUERY = 0x40000000;
        public const uint TME_CANCEL = 0x80000000;
        public const uint HOVER_DEFAULT = 0xFFFFFFFF;

        public const int CS_VREDRAW = 0x0001;
        public const int CS_HREDRAW = 0x0002;
        public const int CS_DBLCLKS = 0x0008;
        public const int CS_OWNDC = 0x0020;
        public const int CS_CLASSDC = 0x0040;
        public const int CS_PARENTDC = 0x0080;
        public const int CS_NOCLOSE = 0x0200;
        public const int CS_SAVEBITS = 0x0800;
        public const int CS_BYTEALIGNCLIENT = 0x1000;
        public const int CS_BYTEALIGNWINDOW = 0x2000;
        public const int CS_GLOBALCLASS = 0x4000;
        public const int CS_IME = 0x00010000;
        public const int CS_DROPSHADOW = 0x00020000;
        public const long PRF_CHECKVISIBLE = 0x00000001L;
        public const long PRF_NONCLIENT = 0x00000002L;
        public const long PRF_CLIENT = 0x00000004L;
        public const long PRF_ERASEBKGND = 0x00000008L;
        public const long PRF_CHILDREN = 0x00000010L;
        public const long PRF_OWNED = 0x00000020L;
        public const int BDR_RAISEDOUTER = 0x0001;
        public const int BDR_SUNKENOUTER = 0x0002;
        public const int BDR_RAISEDINNER = 0x0004;
        public const int BDR_SUNKENINNER = 0x0008;

        public const int BF_LEFT = 0x0001;
        public const int BF_TOP = 0x0002;
        public const int BF_RIGHT = 0x0004;
        public const int BF_BOTTOM = 0x0008;

        public const int BF_DIAGONAL = 0x0010;
        public const int BF_MIDDLE = 0x0800;
        public const int BF_SOFT = 0x1000;
        public const int BF_ADJUST = 0x2000;
        public const int BF_FLAT = 0x4000;
        public const int BF_MONO = 0x8000;

        public const int DFCS_CAPTIONCLOSE = 0x0000;
        public const int DFCS_CAPTIONMIN = 0x0001;
        public const int DFCS_CAPTIONMAX = 0x0002;
        public const int DFCS_CAPTIONRESTORE = 0x0003;
        public const int DFCS_CAPTIONHELP = 0x0004;
        public const int DFCS_MENUARROW = 0x0000;
        public const int DFCS_MENUCHECK = 0x0001;
        public const int DFCS_MENUBULLET = 0x0002;
        public const int DFCS_MENUARROWRIGHT = 0x0004;
        public const int DFCS_SCROLLUP = 0x0000;
        public const int DFCS_SCROLLDOWN = 0x0001;
        public const int DFCS_SCROLLLEFT = 0x0002;
        public const int DFCS_SCROLLRIGHT = 0x0003;
        public const int DFCS_SCROLLCOMBOBOX = 0x0005;
        public const int DFCS_SCROLLSIZEGRIP = 0x0008;
        public const int DFCS_SCROLLSIZEGRIPRIGHT = 0x0010;
        public const int DFCS_BUTTONCHECK = 0x0000;
        public const int DFCS_BUTTONRADIOIMAGE = 0x0001;
        public const int DFCS_BUTTONRADIOMASK = 0x0002;
        public const int DFCS_BUTTONRADIO = 0x0004;
        public const int DFCS_BUTTON3STATE = 0x0008;
        public const int DFCS_BUTTONPUSH = 0x0010;
        public const int DFCS_INACTIVE = 0x0100;
        public const int DFCS_PUSHED = 0x0200;
        public const int DFCS_CHECKED = 0x0400;
        public const int DFCS_TRANSPARENT = 0x0800;
        public const int DFCS_HOT = 0x1000;
        public const int DFCS_ADJUSTRECT = 0x2000;
        public const int DFCS_FLAT = 0x4000;
        public const int DFCS_MONO = 0x8000;
        public const int DC_ACTIVE = 0x0001;
        public const int DC_SMALLCAP = 0x0002;
        public const int DC_ICON = 0x0004;
        public const int DC_TEXT = 0x0008;
        public const int DC_INBUTTON = 0x0010;
        public const int DC_GRADIENT = 0x0020;
        public const int DC_BUTTONS = 0x1000;
        public const int CF_OWNERDISPLAY = 0x0080;
        public const int CF_DSPTEXT = 0x0081;
        public const int CF_DSPBITMAP = 0x0082;
        public const int CF_DSPMETAFILEPICT = 0x0083;
        public const int CF_DSPENHMETAFILE = 0x008E;
        public const int CF_PRIVATEFIRST = 0x0200;
        public const int CF_PRIVATELAST = 0x02FF;
        public const int CF_GDIOBJFIRST = 0x0300;
        public const int CF_GDIOBJLAST = 0x03FF;

        public const int FNOINVERT = 0x02;
        public const int FSHIFT = 0x04;
        public const int FCONTROL = 0x08;
        public const int FALT = 0x10;
        public const int WPF_SETMINPOSITION = 0x0001;
        public const int WPF_RESTORETOMAXIMIZED = 0x0002;
        public const int WPF_ASYNCWINDOWPLACEMENT = 0x0004;
        public const int ODA_DRAWENTIRE = 0x0001;
        public const int ODA_SELECT = 0x0002;
        public const int ODA_FOCUS = 0x0004;
        public const int ODS_SELECTED = 0x0001;
        public const int ODS_GRAYED = 0x0002;
        public const int ODS_DISABLED = 0x0004;
        public const int ODS_CHECKED = 0x0008;
        public const int ODS_FOCUS = 0x0010;
        public const int ODS_DEFAULT = 0x0020;
        public const int ODS_COMBOBOXEDIT = 0x1000;
        public const int ODS_HOTLIGHT = 0x0040;
        public const int ODS_INACTIVE = 0x0080;
        public const int ODS_NOACCEL = 0x0100;
        public const int ODS_NOFOCUSRECT = 0x0200;
        public const int PM_NOREMOVE = 0x0000;
        public const int PM_REMOVE = 0x0001;
        public const int PM_NOYIELD = 0x0002;

        public const int MOD_ALT = 0x0001;
        public const int MOD_CONTROL = 0x0002;
        public const int MOD_SHIFT = 0x0004;
        public const int MOD_WIN = 0x0008;
        public const int MOD_NOREPEAT = 0x4000;
        public const int ENDSESSION_CLOSEAPP = 0x00000001;
        public const int ENDSESSION_CRITICAL = 0x40000000;
        public const uint ENDSESSION_LOGOFF = 0x80000000;
        public const int EWX_LOGOFF = 0x00000000;
        public const int EWX_SHUTDOWN = 0x00000001;
        public const int EWX_REBOOT = 0x00000002;
        public const int EWX_FORCE = 0x00000004;
        public const int EWX_POWEROFF = 0x00000008;
        public const int EWX_FORCEIFHUNG = 0x00000010;
        public const int EWX_QUICKRESOLVE = 0x00000020;
        public const int EWX_RESTARTAPPS = 0x00000040;
        public const int EWX_HYBRID_SHUTDOWN = 0x00400000;
        public const int EWX_BOOTOPTIONS = 0x01000000;
        public const int EWX_ARSO = 0x04000000;
        public const int EWX_CHECK_SAFE_FOR_SERVER = 0x08000000;
        public const int EWX_SYSTEM_INITIATED = 0x10000000;
        public const int BSM_ALLCOMPONENTS = 0x00000000;
        public const int BSM_VXDS = 0x00000001;
        public const int BSM_NETDRIVER = 0x00000002;
        public const int BSM_INSTALLABLEDRIVERS = 0x00000004;
        public const int BSM_APPLICATIONS = 0x00000008;
        public const int BSM_ALLDESKTOPS = 0x00000010;
        public const int BSF_QUERY = 0x00000001;
        public const int BSF_IGNORECURRENTTASK = 0x00000002;
        public const int BSF_FLUSHDISK = 0x00000004;
        public const int BSF_NOHANG = 0x00000008;
        public const int BSF_POSTMESSAGE = 0x00000010;
        public const int BSF_FORCEIFHUNG = 0x00000020;
        public const int BSF_NOTIMEOUTIFNOTHUNG = 0x00000040;
        public const int BSF_ALLOWSFW = 0x00000080;
        public const int BSF_SENDNOTIFYMESSAGE = 0x00000100;
        public const int BSF_RETURNHDESK = 0x00000200;
        public const int BSF_LUID = 0x00000400;
        public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000;
        public const int DEVICE_NOTIFY_SERVICE_HANDLE = 0x00000001;
        public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 0x00000004;
        public const int ISMEX_NOSEND = 0x00000000;
        public const int ISMEX_SEND = 0x00000001;
        public const int ISMEX_NOTIFY = 0x00000002;
        public const int ISMEX_CALLBACK = 0x00000004;
        public const int ISMEX_REPLIED = 0x00000008;
        public const uint CW_USEDEFAULT = ((uint)0x80000000);
        public const int HWND_DESKTOP = 0;
        public const int PW_CLIENTONLY = 0x00000001;
        public const int PW_RENDERFULLCONTENT = 0x00000002;
        public const int LWA_COLORKEY = 0x00000001;
        public const int LWA_ALPHA = 0x00000002;
        public const int ULW_COLORKEY = 0x00000001;
        public const int ULW_ALPHA = 0x00000002;
        public const int ULW_OPAQUE = 0x00000004;
        public const int ULW_EX_NORESIZE = 0x00000008;
        public const int FLASHW_STOP=	0	;
        public const int FLASHW_CAPTION = 0x00000001;
        public const int FLASHW_TRAY = 0x00000002;
        //public const int FLASHW_ALL  =(FLASHW_CAPTION |;
        public const int FLASHW_TIMER = 0x00000004;
        public const int FLASHW_TIMERNOFG = 0x0000000C;
        public const int WDA_NONE = 0x00000000;
        public const int WDA_MONITOR = 0x00000001;
        public const int WDA_EXCLUDEFROMCAPTURE = 0x00000011;
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_NOREDRAW = 0x0008;
        public const int SWP_NOACTIVATE = 0x0010;
        public const int SWP_FRAMECHANGED = 0x0020;
        public const int SWP_SHOWWINDOW = 0x0040;
        public const int SWP_HIDEWINDOW = 0x0080;
        public const int SWP_NOCOPYBITS = 0x0100;
        public const int SWP_NOOWNERZORDER = 0x0200;
        public const int SWP_NOSENDCHANGING = 0x0400;

        public const int SWP_DEFERERASE = 0x2000;
        public const int SWP_ASYNCWINDOWPOS = 0x4000;
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const int KEYEVENTF_KEYUP = 0x0002;
        public const int KEYEVENTF_UNICODE = 0x0004;
        public const int KEYEVENTF_SCANCODE = 0x0008;
        public const int MOUSEEVENTF_MOVE = 0x0001;
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const int MOUSEEVENTF_LEFTUP = 0x0004;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        public const int MOUSEEVENTF_XDOWN = 0x0080;
        public const int MOUSEEVENTF_XUP = 0x0100;
        public const int MOUSEEVENTF_WHEEL = 0x0800;
        public const int MOUSEEVENTF_HWHEEL = 0x01000;
        public const int MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000;
        public const int MOUSEEVENTF_VIRTUALDESK = 0x4000;
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        public const int TOUCHEVENTF_MOVE = 0x0001;
        public const int TOUCHEVENTF_DOWN = 0x0002;
        public const int TOUCHEVENTF_UP = 0x0004;
        public const int TOUCHEVENTF_INRANGE = 0x0008;
        public const int TOUCHEVENTF_PRIMARY = 0x0010;
        public const int TOUCHEVENTF_NOCOALESCE = 0x0020;
        public const int TOUCHEVENTF_PEN = 0x0040;
        public const int TOUCHEVENTF_PALM = 0x0080;
        public const int TOUCHINPUTMASKF_TIMEFROMSYSTEM = 0x0001;
        public const int TOUCHINPUTMASKF_EXTRAINFO = 0x0002;
        public const int TOUCHINPUTMASKF_CONTACTAREA = 0x0004;

        public const int POINTER_FLAG_NONE = 0x00000000;
        public const int POINTER_FLAG_NEW = 0x00000001;
        public const int POINTER_FLAG_INRANGE = 0x00000002;
        public const int POINTER_FLAG_INCONTACT = 0x00000004;
        public const int POINTER_FLAG_FIRSTBUTTON = 0x00000010;
        public const int POINTER_FLAG_SECONDBUTTON = 0x00000020;
        public const int POINTER_FLAG_THIRDBUTTON = 0x00000040;
        public const int POINTER_FLAG_FOURTHBUTTON = 0x00000080;
        public const int POINTER_FLAG_FIFTHBUTTON = 0x00000100;
        public const int POINTER_FLAG_PRIMARY = 0x00002000;
        public const int POINTER_FLAG_CONFIDENCE = 0x00004000;
        public const int POINTER_FLAG_CANCELED = 0x00008000;
        public const int POINTER_FLAG_DOWN = 0x00010000;
        public const int POINTER_FLAG_UPDATE = 0x00020000;
        public const int POINTER_FLAG_UP = 0x00040000;
        public const int POINTER_FLAG_WHEEL = 0x00080000;
        public const int POINTER_FLAG_HWHEEL = 0x00100000;
        public const int POINTER_FLAG_CAPTURECHANGED = 0x00200000;
        public const int POINTER_FLAG_HASTRANSFORM = 0x00400000;


        public const int TOUCH_FLAG_NONE = 0x00000000;
        public const int TOUCH_MASK_NONE = 0x00000000;
        public const int TOUCH_MASK_CONTACTAREA = 0x00000001;
        public const int TOUCH_MASK_ORIENTATION = 0x00000002;
        public const int TOUCH_MASK_PRESSURE = 0x00000004;
        public const int PEN_FLAG_NONE = 0x00000000;
        public const int PEN_FLAG_BARREL = 0x00000001;
        public const int PEN_FLAG_INVERTED = 0x00000002;
        public const int PEN_FLAG_ERASER = 0x00000004;
        public const int PEN_MASK_NONE = 0x00000000;
        public const int PEN_MASK_PRESSURE = 0x00000001;
        public const int PEN_MASK_ROTATION = 0x00000002;
        public const int PEN_MASK_TILT_X = 0x00000004;
        public const int PEN_MASK_TILT_Y = 0x00000008;
        public const int POINTER_MESSAGE_FLAG_NEW = 0x00000001;
        public const int POINTER_MESSAGE_FLAG_INRANGE = 0x00000002;
        public const int POINTER_MESSAGE_FLAG_INCONTACT = 0x00000004;
        public const int POINTER_MESSAGE_FLAG_FIRSTBUTTON = 0x00000010;
        public const int POINTER_MESSAGE_FLAG_SECONDBUTTON = 0x00000020;
        public const int POINTER_MESSAGE_FLAG_THIRDBUTTON = 0x00000040;
        public const int POINTER_MESSAGE_FLAG_FOURTHBUTTON = 0x00000080;
        public const int POINTER_MESSAGE_FLAG_FIFTHBUTTON = 0x00000100;
        public const int POINTER_MESSAGE_FLAG_PRIMARY = 0x00002000;
        public const int POINTER_MESSAGE_FLAG_CONFIDENCE = 0x00004000;
        public const int POINTER_MESSAGE_FLAG_CANCELED = 0x00008000;
        public const int TOUCH_FEEDBACK_DEFAULT = 0x1;
        public const int TOUCH_FEEDBACK_INDIRECT = 0x2;
        public const int TOUCH_FEEDBACK_NONE = 0x3;
        public const int TOUCH_HIT_TESTING_DEFAULT = 0x0;
        public const int TOUCH_HIT_TESTING_CLIENT = 0x1;
        public const int TOUCH_HIT_TESTING_NONE = 0x2;

        public const int MWMO_WAITALL = 0x0001;
        public const int MWMO_ALERTABLE = 0x0002;
        public const int MWMO_INPUTAVAILABLE = 0x0004;
        public const int QS_KEY = 0x0001;
        public const int QS_MOUSEMOVE = 0x0002;
        public const int QS_MOUSEBUTTON = 0x0004;
        public const int QS_POSTMESSAGE = 0x0008;
        public const int QS_TIMER = 0x0010;
        public const int QS_PAINT = 0x0020;
        public const int QS_SENDMESSAGE = 0x0040;
        public const int QS_HOTKEY = 0x0080;
        public const int QS_ALLPOSTMESSAGE = 0x0100;
        public const int QS_RAWINPUT = 0x0400;
        public const int QS_TOUCH = 0x0800;
        public const int QS_POINTER = 0x1000;

        //public const int QS_MOUSE    (QS_MOUSEMOVE |;
        //public const int QS_INPUT    (QS_MOUSE |;
        //public const int QS_INPUT    (QS_MOUSE |;
        //public const int QS_INPUT    (QS_MOUSE |;
        //public const int QS_ALLEVENTS    (QS_INPUT |;
        //public const int QS_ALLINPUT (QS_INPUT |;
        public const int USER_TIMER_MAXIMUM = 0x7FFFFFFF;
        public const int USER_TIMER_MINIMUM = 0x0000000A;

        public const int SM_REMOTESESSION = 0x1000;
        public const int SM_SHUTTINGDOWN = 0x2000;
        public const int SM_REMOTECONTROL = 0x2001;
        public const int SM_CARETBLINKINGENABLED = 0x2002;
        public const int SM_CONVERTIBLESLATEMODE = 0x2003;
        public const int SM_SYSTEMDOCKED = 0x2004;
        public const int PMB_ACTIVE = 0x00000001;
        public const uint MNS_NOCHECK = 0x80000000;
        public const int MNS_MODELESS = 0x40000000;
        public const int MNS_DRAGDROP = 0x20000000;
        public const int MNS_AUTODISMISS = 0x10000000;
        public const int MNS_NOTIFYBYPOS = 0x08000000;
        public const int MNS_CHECKORBMP = 0x04000000;
        public const int MIM_MAXHEIGHT = 0x00000001;
        public const int MIM_BACKGROUND = 0x00000002;
        public const int MIM_HELPID = 0x00000004;
        public const int MIM_MENUDATA = 0x00000008;
        public const int MIM_STYLE = 0x00000010;
        public const uint MIM_APPLYTOSUBMENUS = 0x80000000;
        public const int MNGOF_TOPGAP = 0x00000001;
        public const int MNGOF_BOTTOMGAP = 0x00000002;
        public const int MNGO_NOINTERFACE = 0x00000000;
        public const int MNGO_NOERROR = 0x00000001;
        public const int MIIM_STATE = 0x00000001;
        public const int MIIM_ID = 0x00000002;
        public const int MIIM_SUBMENU = 0x00000004;
        public const int MIIM_CHECKMARKS = 0x00000008;
        public const int MIIM_TYPE = 0x00000010;
        public const int MIIM_DATA = 0x00000020;
        public const int MIIM_STRING = 0x00000040;
        public const int MIIM_BITMAP = 0x00000080;
        public const int MIIM_FTYPE = 0x00000100;

        public const long GMDI_USEDISABLED = 0x0001L;
        public const long GMDI_GOINTOPOPUPS = 0x0002L;
        public const long TPM_LEFTBUTTON = 0x0000L;
        public const long TPM_RIGHTBUTTON = 0x0002L;
        public const long TPM_LEFTALIGN = 0x0000L;
        public const long TPM_CENTERALIGN = 0x0004L;
        public const long TPM_RIGHTALIGN = 0x0008L;
        public const long TPM_TOPALIGN = 0x0000L;
        public const long TPM_VCENTERALIGN = 0x0010L;
        public const long TPM_BOTTOMALIGN = 0x0020L;
        public const long TPM_HORIZONTAL = 0x0000L;
        public const long TPM_VERTICAL = 0x0040L;
        public const long TPM_NONOTIFY = 0x0080L;
        public const long TPM_RETURNCMD = 0x0100L;
        public const long TPM_RECURSE = 0x0001L;
        public const long TPM_HORPOSANIMATION = 0x0400L;
        public const long TPM_HORNEGANIMATION = 0x0800L;
        public const long TPM_VERPOSANIMATION = 0x1000L;
        public const long TPM_VERNEGANIMATION = 0x2000L;
        public const long TPM_NOANIMATION = 0x4000L;
        public const long TPM_LAYOUTRTL = 0x8000L;
        public const long TPM_WORKAREA = 0x10000L;

        public const int DOF_EXECUTABLE = 0x8001;
        public const int DOF_DOCUMENT = 0x8002;
        public const int DOF_DIRECTORY = 0x8003;
        public const int DOF_MULTIPLE = 0x8004;
        public const int DOF_PROGMAN = 0x0001;
        public const int DOF_SHELLDATA = 0x0002;
        public const long DO_DROPFILE = 0x454C4946L;
        public const long DO_PRINTFILE = 0x544E5250L;
        public const int DT_TOP = 0x00000000;
        public const int DT_LEFT = 0x00000000;
        public const int DT_CENTER = 0x00000001;
        public const int DT_RIGHT = 0x00000002;
        public const int DT_VCENTER = 0x00000004;
        public const int DT_BOTTOM = 0x00000008;
        public const int DT_WORDBREAK = 0x00000010;
        public const int DT_SINGLELINE = 0x00000020;
        public const int DT_EXPANDTABS = 0x00000040;
        public const int DT_TABSTOP = 0x00000080;
        public const int DT_NOCLIP = 0x00000100;
        public const int DT_EXTERNALLEADING = 0x00000200;
        public const int DT_CALCRECT = 0x00000400;
        public const int DT_NOPREFIX = 0x00000800;
        public const int DT_INTERNAL = 0x00001000;
        public const int DT_EDITCONTROL = 0x00002000;
        public const int DT_PATH_ELLIPSIS = 0x00004000;
        public const int DT_END_ELLIPSIS = 0x00008000;
        public const int DT_MODIFYSTRING = 0x00010000;
        public const int DT_RTLREADING = 0x00020000;
        public const int DT_WORD_ELLIPSIS = 0x00040000;
        public const int DT_NOFULLWIDTHCHARBREAK = 0x00080000;
        public const int DT_HIDEPREFIX = 0x00100000;
        public const int DT_PREFIXONLY = 0x00200000;
        public const int DST_COMPLEX = 0x0000;
        public const int DST_TEXT = 0x0001;
        public const int DST_PREFIXTEXT = 0x0002;
        public const int DST_ICON = 0x0003;
        public const int DST_BITMAP = 0x0004;
        public const int DSS_NORMAL = 0x0000;
        public const int DSS_UNION = 0x0010;
        public const int DSS_DISABLED = 0x0020;
        public const int DSS_MONO = 0x0080;
        public const int DSS_HIDEPREFIX = 0x0200;
        public const int DSS_PREFIXONLY = 0x0400;
        public const int DSS_RIGHT = 0x8000;

        public const long DCX_WINDOW = 0x00000001L;
        public const long DCX_CACHE = 0x00000002L;
        public const long DCX_NORESETATTRS = 0x00000004L;
        public const long DCX_CLIPCHILDREN = 0x00000008L;
        public const long DCX_CLIPSIBLINGS = 0x00000010L;
        public const long DCX_PARENTCLIP = 0x00000020L;
        public const long DCX_EXCLUDERGN = 0x00000040L;
        public const long DCX_INTERSECTRGN = 0x00000080L;
        public const long DCX_EXCLUDEUPDATE = 0x00000100L;
        public const long DCX_INTERSECTUPDATE = 0x00000200L;
        public const long DCX_LOCKWINDOWUPDATE = 0x00000400L;
        public const long DCX_VALIDATE = 0x00200000L;
        public const int RDW_INVALIDATE = 0x0001;
        public const int RDW_INTERNALPAINT = 0x0002;
        public const int RDW_ERASE = 0x0004;
        public const int RDW_VALIDATE = 0x0008;
        public const int RDW_NOINTERNALPAINT = 0x0010;
        public const int RDW_NOERASE = 0x0020;
        public const int RDW_NOCHILDREN = 0x0040;
        public const int RDW_ALLCHILDREN = 0x0080;
        public const int RDW_UPDATENOW = 0x0100;
        public const int RDW_ERASENOW = 0x0200;
        public const int RDW_FRAME = 0x0400;
        public const int RDW_NOFRAME = 0x0800;
        public const int SW_SCROLLCHILDREN = 0x0001;
        public const int SW_INVALIDATE = 0x0002;
        public const int SW_ERASE = 0x0004;
        public const int SW_SMOOTHSCROLL = 0x0010;
        public const int ESB_ENABLE_BOTH = 0x0000;
        public const int ESB_DISABLE_BOTH = 0x0003;
        public const int ESB_DISABLE_LEFT = 0x0001;
        public const int ESB_DISABLE_RIGHT = 0x0002;
        public const int ESB_DISABLE_UP = 0x0001;
        public const int ESB_DISABLE_DOWN = 0x0002;

        public const int HELPINFO_WINDOW = 0x0001;
        public const int HELPINFO_MENUITEM = 0x0002;
        public const long MB_OK = 0x00000000L;
        public const long MB_OKCANCEL = 0x00000001L;
        public const long MB_ABORTRETRYIGNORE = 0x00000002L;
        public const long MB_YESNOCANCEL = 0x00000003L;
        public const long MB_YESNO = 0x00000004L;
        public const long MB_RETRYCANCEL = 0x00000005L;
        public const long MB_CANCELTRYCONTINUE = 0x00000006L;
        public const long MB_ICONHAND = 0x00000010L;
        public const long MB_ICONQUESTION = 0x00000020L;
        public const long MB_ICONEXCLAMATION = 0x00000030L;
        public const long MB_ICONASTERISK = 0x00000040L;
        public const long MB_USERICON = 0x00000080L;

        public const long MB_DEFBUTTON1 = 0x00000000L;
        public const long MB_DEFBUTTON2 = 0x00000100L;
        public const long MB_DEFBUTTON3 = 0x00000200L;
        public const long MB_DEFBUTTON4 = 0x00000300L;
        public const long MB_APPLMODAL = 0x00000000L;
        public const long MB_SYSTEMMODAL = 0x00001000L;
        public const long MB_TASKMODAL = 0x00002000L;
        public const long MB_HELP = 0x00004000L;
        public const long MB_NOFOCUS = 0x00008000L;
        public const long MB_SETFOREGROUND = 0x00010000L;
        public const long MB_DEFAULT_DESKTOP_ONLY = 0x00020000L;
        public const long MB_TOPMOST = 0x00040000L;
        public const long MB_RIGHT = 0x00080000L;
        public const long MB_RTLREADING = 0x00100000L;
        public const long MB_SERVICE_NOTIFICATION = 0x00200000L;
        public const long MB_SERVICE_NOTIFICATION_NT3X = 0x00040000L;
        public const long MB_TYPEMASK = 0x0000000FL;
        public const long MB_ICONMASK = 0x000000F0L;
        public const long MB_DEFMASK = 0x00000F00L;
        public const long MB_MODEMASK = 0x00003000L;
        public const long MB_MISCMASK = 0x0000C000L;
        public const int CWP_ALL = 0x0000;
        public const int CWP_SKIPINVISIBLE = 0x0001;
        public const int CWP_SKIPDISABLED = 0x0002;
        public const int CWP_SKIPTRANSPARENT = 0x0004;
        public const int SMTO_NORMAL = 0x0000;
        public const int SMTO_BLOCK = 0x0001;
        public const int SMTO_ABORTIFHUNG = 0x0002;
        public const int SMTO_NOTIMEOUTIFNOTHUNG = 0x0008;
        public const int SMTO_ERRORONEXIT = 0x0020;

        //public const int BDR_OUTER   (BDR_RAISEDOUTER |;
        //public const int BDR_INNER   (BDR_RAISEDINNER |;
        //public const int BDR_RAISED  (BDR_RAISEDOUTER |;
        //public const int BDR_SUNKEN  (BDR_SUNKENOUTER |;
        //public const int EDGE_RAISED (BDR_RAISEDOUTER |;
        //public const int EDGE_SUNKEN (BDR_SUNKENOUTER |;
        //public const int EDGE_ETCHED (BDR_SUNKENOUTER |;
        //public const int EDGE_BUMP   (BDR_RAISEDOUTER |;
        //public const int BF_TOPLEFT  (BF_TOP |;
        //public const int BF_TOPRIGHT (BF_TOP |;
        //public const int BF_BOTTOMLEFT   (BF_BOTTOM |;
        //public const int BF_BOTTOMRIGHT  (BF_BOTTOM |;
        //public const int BF_RECT (BF_LEFT |;
        //public const int BF_DIAGONAL_ENDTOPRIGHT (BF_DIAGONAL |;
        //public const int BF_DIAGONAL_ENDTOPLEFT  (BF_DIAGONAL |;
        //public const int BF_DIAGONAL_ENDBOTTOMLEFT   (BF_DIAGONAL |;
        //public const int BF_DIAGONAL_ENDBOTTOMRIGHT  (BF_DIAGONAL |;

        public const int PWR_OK = 1;
        public const int PWR_SUSPENDREQUEST = 1;
        public const int PWR_SUSPENDRESUME = 2;
        public const int PWR_CRITICALRESUME = 3;
        public const int NFR_ANSI = 1;
        public const int NFR_UNICODE = 2;
        public const int NF_QUERY = 3;
        public const int NF_REQUERY = 4;
        public const int UIS_SET = 1;
        public const int UIS_CLEAR = 2;
        public const int UIS_INITIALIZE = 3;
        public const int WHEEL_DELTA = 120;
        public const int WMSZ_LEFT = 1;
        public const int WMSZ_RIGHT = 2;
        public const int WMSZ_TOP = 3;
        public const int WMSZ_TOPLEFT = 4;
        public const int WMSZ_TOPRIGHT = 5;
        public const int WMSZ_BOTTOM = 6;
        public const int WMSZ_BOTTOMLEFT = 7;
        public const int WMSZ_BOTTOMRIGHT = 8;
        public const int HTNOWHERE = 0;
        public const int HTCLIENT = 1;
        public const int HTCAPTION = 2;
        public const int HTSYSMENU = 3;
        public const int HTGROWBOX = 4;
        public const int HTMENU = 5;
        public const int HTHSCROLL = 6;
        public const int HTVSCROLL = 7;
        public const int HTMINBUTTON = 8;
        public const int HTMAXBUTTON = 9;
        public const int HTLEFT = 10;
        public const int HTRIGHT = 11;
        public const int HTTOP = 12;
        public const int HTTOPLEFT = 13;
        public const int HTTOPRIGHT = 14;
        public const int HTBOTTOM = 15;
        public const int HTBOTTOMLEFT = 16;
        public const int HTBOTTOMRIGHT = 17;
        public const int HTBORDER = 18;
        public const int HTOBJECT = 19;
        public const int HTCLOSE = 20;
        public const int HTHELP = 21;
        public const int MA_ACTIVATE = 1;
        public const int MA_ACTIVATEANDEAT = 2;
        public const int MA_NOACTIVATE = 3;
        public const int MA_NOACTIVATEANDEAT = 4;
        public const int ICON_SMALL = 0;
        public const int ICON_BIG = 1;
        public const int ICON_SMALL2 = 2;
        public const int SIZE_RESTORED = 0;
        public const int SIZE_MINIMIZED = 1;
        public const int SIZE_MAXIMIZED = 2;
        public const int SIZE_MAXSHOW = 3;
        public const int SIZE_MAXHIDE = 4;
        public const int DFC_CAPTION = 1;
        public const int DFC_MENU = 2;
        public const int DFC_SCROLL = 3;
        public const int DFC_BUTTON = 4;
        public const int DFC_POPUPMENU = 5;
        public const int CF_TEXT = 1;
        public const int CF_BITMAP = 2;
        public const int CF_METAFILEPICT = 3;
        public const int CF_SYLK = 4;
        public const int CF_DIF = 5;
        public const int CF_TIFF = 6;
        public const int CF_OEMTEXT = 7;
        public const int CF_DIB = 8;
        public const int CF_PALETTE = 9;
        public const int CF_PENDATA = 10;
        public const int CF_RIFF = 11;
        public const int CF_WAVE = 12;
        public const int CF_UNICODETEXT = 13;
        public const int CF_ENHMETAFILE = 14;
        public const int CF_HDROP = 15;
        public const int CF_LOCALE = 16;
        public const int CF_DIBV5 = 17;
        public const int CF_MAX = 18;
        //public const int FVIRTKEY = TRUE;
        public const int ODT_MENU = 1;
        public const int ODT_LISTBOX = 2;
        public const int ODT_COMBOBOX = 3;
        public const int ODT_BUTTON = 4;
        public const int ODT_STATIC = 5;
        public const int DLGWINDOWEXTRA = 48;
        public const int INPUT_MOUSE = 0;
        public const int INPUT_KEYBOARD = 1;
        public const int INPUT_HARDWARE = 2;
        public const int MAX_TOUCH_COUNT = 256;
        public const int MAPVK_VK_TO_VSC = 0;
        public const int TIMERV_DEFAULT_COALESCING = 0;
        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;
        public const int SM_CXVSCROLL = 2;
        public const int SM_CYHSCROLL = 3;
        public const int SM_CYCAPTION = 4;
        public const int SM_CXBORDER = 5;
        public const int SM_CYBORDER = 6;
        public const int SM_CXDLGFRAME = 7;
        public const int SM_CYDLGFRAME = 8;
        public const int SM_CYVTHUMB = 9;
        public const int SM_CXHTHUMB = 10;
        public const int SM_CXICON = 11;
        public const int SM_CYICON = 12;
        public const int SM_CXCURSOR = 13;
        public const int SM_CYCURSOR = 14;
        public const int SM_CYMENU = 15;
        public const int SM_CXFULLSCREEN = 16;
        public const int SM_CYFULLSCREEN = 17;
        public const int SM_CYKANJIWINDOW = 18;
        public const int SM_MOUSEPRESENT = 19;
        public const int SM_CYVSCROLL = 20;
        public const int SM_CXHSCROLL = 21;
        public const int SM_DEBUG = 22;
        public const int SM_SWAPBUTTON = 23;
        public const int SM_RESERVED1 = 24;
        public const int SM_RESERVED2 = 25;
        public const int SM_RESERVED3 = 26;
        public const int SM_RESERVED4 = 27;
        public const int SM_CXMIN = 28;
        public const int SM_CYMIN = 29;
        public const int SM_CXSIZE = 30;
        public const int SM_CYSIZE = 31;
        public const int SM_CXFRAME = 32;
        public const int SM_CYFRAME = 33;
        public const int SM_CXMINTRACK = 34;
        public const int SM_CYMINTRACK = 35;
        public const int SM_CXDOUBLECLK = 36;
        public const int SM_CYDOUBLECLK = 37;
        public const int SM_CXICONSPACING = 38;
        public const int SM_CYICONSPACING = 39;
        public const int SM_MENUDROPALIGNMENT = 40;
        public const int SM_PENWINDOWS = 41;
        public const int SM_DBCSENABLED = 42;
        public const int SM_CMOUSEBUTTONS = 43;
        public const int SM_SECURE = 44;
        public const int SM_CXEDGE = 45;
        public const int SM_CYEDGE = 46;
        public const int SM_CXMINSPACING = 47;
        public const int SM_CYMINSPACING = 48;
        public const int SM_CXSMICON = 49;
        public const int SM_CYSMICON = 50;
        public const int SM_CYSMCAPTION = 51;
        public const int SM_CXSMSIZE = 52;
        public const int SM_CYSMSIZE = 53;
        public const int SM_CXMENUSIZE = 54;
        public const int SM_CYMENUSIZE = 55;
        public const int SM_ARRANGE = 56;
        public const int SM_CXMINIMIZED = 57;
        public const int SM_CYMINIMIZED = 58;
        public const int SM_CXMAXTRACK = 59;
        public const int SM_CYMAXTRACK = 60;
        public const int SM_CXMAXIMIZED = 61;
        public const int SM_CYMAXIMIZED = 62;
        public const int SM_NETWORK = 63;
        public const int SM_CLEANBOOT = 67;
        public const int SM_CXDRAG = 68;
        public const int SM_CYDRAG = 69;
        public const int SM_SHOWSOUNDS = 70;
        public const int SM_CXMENUCHECK = 71;
        public const int SM_CYMENUCHECK = 72;
        public const int SM_SLOWMACHINE = 73;
        public const int SM_MIDEASTENABLED = 74;
        public const int SM_MOUSEWHEELPRESENT = 75;
        public const int SM_XVIRTUALSCREEN = 76;
        public const int SM_YVIRTUALSCREEN = 77;
        public const int SM_CXVIRTUALSCREEN = 78;
        public const int SM_CYVIRTUALSCREEN = 79;
        public const int SM_CMONITORS = 80;
        public const int SM_SAMEDISPLAYFORMAT = 81;
        public const int SM_IMMENABLED = 82;
        public const int SM_CXFOCUSBORDER = 83;
        public const int SM_CYFOCUSBORDER = 84;
        public const int SM_TABLETPC = 86;
        public const int SM_MEDIACENTER = 87;
        public const int SM_STARTER = 88;
        public const int SM_SERVERR2 = 89;
        public const int SM_MOUSEHORIZONTALWHEELPRESENT = 91;
        public const int SM_CXPADDEDBORDER = 92;
        public const int SM_DIGITIZER = 94;
        public const int SM_MAXIMUMTOUCHES = 95;
        public const int SM_CMETRICS = 97;
        public const int MNC_IGNORE = 0;
        public const int MNC_CLOSE = 1;
        public const int MNC_EXECUTE = 2;
        public const int MNC_SELECT = 3;
        public const int MND_CONTINUE = 0;
        public const int MND_ENDMENU = 1;
        public const int HTSIZE = HTGROWBOX;
        public const int HTREDUCE = HTMINBUTTON;
        public const int HTZOOM = HTMAXBUTTON;
        public const int HTSIZEFIRST = HTLEFT;
        public const int HTSIZELAST = HTBOTTOMRIGHT;
        public const int SIZENORMAL = SIZE_RESTORED;
        public const int SIZEICONIC = SIZE_MINIMIZED;
        public const int SIZEFULLSCREEN = SIZE_MAXIMIZED;
        public const int SIZEZOOMSHOW = SIZE_MAXSHOW;
        public const int SIZEZOOMHIDE = SIZE_MAXHIDE;
        public const int SM_CXFIXEDFRAME = SM_CXDLGFRAME;
        public const int SM_CYFIXEDFRAME = SM_CYDLGFRAME;
        public const int SM_CXSIZEFRAME = SM_CXFRAME;
        public const int SM_CYSIZEFRAME = SM_CYFRAME;
        //public const int HBMMENU_CALLBACK = ((HBITMAP) - 1);
        //public const int HBMMENU_SYSTEM = ((HBITMAP)1);
        //public const int HBMMENU_MBAR_RESTORE = ((HBITMAP)2);
        //public const int HBMMENU_MBAR_MINIMIZE = ((HBITMAP)3);
        //public const int HBMMENU_MBAR_CLOSE = ((HBITMAP)5);
        //public const int HBMMENU_MBAR_CLOSE_D = ((HBITMAP)6);
        //public const int HBMMENU_MBAR_MINIMIZE_D = ((HBITMAP)7);
        //public const int HBMMENU_POPUP_CLOSE = ((HBITMAP)8);
        //public const int HBMMENU_POPUP_RESTORE = ((HBITMAP)9);
        //public const int HBMMENU_POPUP_MAXIMIZE = ((HBITMAP)10);
        //public const int HBMMENU_POPUP_MINIMIZE = ((HBITMAP)11);
        public const int LSFW_LOCK = 1;
        public const int LSFW_UNLOCK = 2;
        public const int CTLCOLOR_MSGBOX = 0;
        public const int CTLCOLOR_EDIT = 1;
        public const int CTLCOLOR_LISTBOX = 2;
        public const int CTLCOLOR_BTN = 3;
        public const int CTLCOLOR_DLG = 4;
        public const int CTLCOLOR_SCROLLBAR = 5;
        public const int CTLCOLOR_STATIC = 6;
        public const int CTLCOLOR_MAX = 7;
        public const int COLOR_SCROLLBAR = 0;
        public const int COLOR_BACKGROUND = 1;
        public const int COLOR_ACTIVECAPTION = 2;
        public const int COLOR_INACTIVECAPTION = 3;
        public const int COLOR_MENU = 4;
        public const int COLOR_WINDOW = 5;
        public const int COLOR_WINDOWFRAME = 6;
        public const int COLOR_MENUTEXT = 7;
        public const int COLOR_WINDOWTEXT = 8;
        public const int COLOR_CAPTIONTEXT = 9;
        public const int COLOR_ACTIVEBORDER = 10;
        public const int COLOR_INACTIVEBORDER = 11;
        public const int COLOR_APPWORKSPACE = 12;
        public const int COLOR_HIGHLIGHT = 13;
        public const int COLOR_HIGHLIGHTTEXT = 14;
        public const int COLOR_BTNFACE = 15;
        public const int COLOR_BTNSHADOW = 16;
        public const int COLOR_GRAYTEXT = 17;
        public const int COLOR_BTNTEXT = 18;
        public const int COLOR_INACTIVECAPTIONTEXT = 19;
        public const int COLOR_BTNHIGHLIGHT = 20;
        public const int COLOR_3DDKSHADOW = 21;
        public const int COLOR_3DLIGHT = 22;
        public const int COLOR_INFOTEXT = 23;
        public const int COLOR_INFOBK = 24;
        public const int COLOR_HOTLIGHT = 26;
        public const int COLOR_GRADIENTACTIVECAPTION = 27;
        public const int COLOR_GRADIENTINACTIVECAPTION = 28;
        public const int COLOR_MENUHILIGHT = 29;
        public const int COLOR_MENUBAR = 30;
        public const int GW_HWNDFIRST = 0;
        public const int GW_HWNDLAST = 1;
        public const int GW_HWNDNEXT = 2;
        public const int GW_HWNDPREV = 3;
        public const int GW_OWNER = 4;
        public const int GW_CHILD = 5;
        public const int GW_ENABLEDPOPUP = 6;
        public const int GW_MAX = 6;


        //public const int WS_TILED = WS_OVERLAPPED;
        //public const int WS_ICONIC = WS_MINIMIZE;
        //public const int WS_SIZEBOX = WS_THICKFRAME;
        //public const int WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;
        public const int IDHOT_SNAPWINDOW = (-1);
        public const int IDHOT_SNAPDESKTOP = (-2);
        public const int MAPVK_VSC_TO_VK = -1;
        public const int MAPVK_VK_TO_CHAR = -2;
        public const int MAPVK_VSC_TO_VK_EX = -3;
        public const int MAPVK_VK_TO_VSC_EX = -4;
        public const uint TIMERV_NO_COALESCING = (0xFFFFFFFF);
        public const int TIMERV_COALESCING_MIN = -1;
        public const int TIMERV_COALESCING_MAX = (0x7FFFFFF5);

        public const int COLOR_DESKTOP = COLOR_BACKGROUND;
        public const int COLOR_3DFACE = COLOR_BTNFACE;
        public const int COLOR_3DSHADOW = COLOR_BTNSHADOW;
        public const int COLOR_3DHIGHLIGHT = COLOR_BTNHIGHLIGHT;
        public const int COLOR_3DHILIGHT = COLOR_BTNHIGHLIGHT;
        public const int COLOR_BTNHILIGHT = COLOR_BTNHIGHLIGHT;

        //public const int ASFW_ANY = ((DWORD) - 1);
        public const int ASFW_ANY = (-1);
        public const int ESB_DISABLE_LTUP = ESB_DISABLE_LEFT;
        public const int ESB_DISABLE_RTDN = ESB_DISABLE_RIGHT;
        public const long MB_ICONWARNING = MB_ICONEXCLAMATION;
        public const long MB_ICONERROR = MB_ICONHAND;
        public const long MB_ICONINFORMATION = MB_ICONASTERISK;
        public const long MB_ICONSTOP = MB_ICONHAND;

        public const int SWP_DRAWFRAME = SWP_FRAMECHANGED;
        public const int SWP_NOREPOSITION = SWP_NOOWNERZORDER;

        public const IntPtr HWND_TOP = ((int)0);
        public const IntPtr HWND_BOTTOM = ((int)1);
        public const IntPtr HWND_TOPMOST = ((int) - 1);
        public const IntPtr HWND_NOTOPMOST = ((int) - 2);
        public const int TWF_FINETOUCH = (0x00000001);
        public const int TWF_WANTPALM = (0x00000002);
        public const int POINTER_MOD_SHIFT = (0x0004);
        public const int POINTER_MOD_CTRL = (0x0008);
        //public const int GET_POINTERID_WPARAM(wParam)	(LOWORD(wParam))	;
        //public const int IS_POINTER_FLAG_SET_WPARAM(wParam   flag)	(((DWORD) HIWORD(wParam);
        //public const int IS_POINTER_NEW_WPARAM(wParam)	IS_POINTER_FLAG_SET_WPARAM(wParam POINTER_MESSAGE_FLAG_NEW);
        //public const int IS_POINTER_INRANGE_WPARAM(wParam)	IS_POINTER_FLAG_SET_WPARAM(wParam POINTER_MESSAGE_FLAG_INRANGE);
        //public const int IS_POINTER_INCONTACT_WPARAM(wParam)	IS_POINTER_FLAG_SET_WPARAM(wParam POINTER_MESSAGE_FLAG_INCONTACT);
        //public const int IS_POINTER_FIRSTBUTTON_WPARAM(wParam)	IS_POINTER_FLAG_SET_WPARAM(wParam POINTER_MESSAGE_FLAG_FIRSTBUTTON);
        //public const int IS_POINTER_SECONDBUTTON_WPARAM(wParam)	IS_POINTER_FLAG_SET_WPARAM(wParam POINTER_MESSAGE_FLAG_SECONDBUTTON);
        //public const int IS_POINTER_THIRDBUTTON_WPARAM(wParam)	IS_POINTER_FLAG_SET_WPARAM(wParam POINTER_MESSAGE_FLAG_THIRDBUTTON);
        //public const int IS_POINTER_FOURTHBUTTON_WPARAM(wParam)	IS_POINTER_FLAG_SET_WPARAM(wParam POINTER_MESSAGE_FLAG_FOURTHBUTTON);
        //public const int IS_POINTER_FIFTHBUTTON_WPARAM(wParam)	IS_POINTER_FLAG_SET_WPARAM(wParam POINTER_MESSAGE_FLAG_FIFTHBUTTON);
        //public const int IS_POINTER_PRIMARY_WPARAM(wParam)	IS_POINTER_FLAG_SET_WPARAM(wParam POINTER_MESSAGE_FLAG_PRIMARY);
        //public const int HAS_POINTER_CONFIDENCE_WPARAM(wParam)	IS_POINTER_FLAG_SET_WPARAM(wParam POINTER_MESSAGE_FLAG_CONFIDENCE);
        //public const int IS_POINTER_CANCELED_WPARAM(wParam)	IS_POINTER_FLAG_SET_WPARAM(wParam POINTER_MESSAGE_FLAG_CANCELED);
        public const int PA_ACTIVATE = MA_ACTIVATE;
        public const int PA_NOACTIVATE = MA_NOACTIVATE;

        public const int HTERROR = (-2);
        public const int HTTRANSPARENT = (-1);
        public const int PWR_FAIL = (-1);
        public const uint UINT_MAX = 0xffffffff;
        public const uint WHEEL_PAGESCROLL = (UINT_MAX);

        public const int GWL_STYLE = -16;

        //public const int GET_WHEEL_DELTA_WPARAM(wParam)	((short) HIWORD(wParam))	;

        //public const int GET_KEYSTATE_WPARAM(wParam)	(LOWORD(wParam))	;
        //public const int GET_NCHITTEST_WPARAM(wParam)	((short) LOWORD(wParam))	;
        //public const int GET_XBUTTON_WPARAM(wParam)	(HIWORD(wParam))	;



        #endregion Const

        #region WindowsAPI
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
        private static extern int SHCreateItemFromParsingName(
               string pszPath,
               IntPtr pbc,
               [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
               out IntPtr ppv);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int FormatMessage(
            int flags,
            IntPtr source,
            int messageId,
            int languageId,
            StringBuilder buffer,
            int size,
            IntPtr arguments);
        [DllImport("Ole32.dll")]
        public static extern int CoInitialize(IntPtr pvReserved);

        // P/Invoke 声明 CoInitializeEx
        [DllImport("Ole32.dll")]
        public static extern int CoInitializeEx(IntPtr pvReserved, CoInit dwCoInit);

        // P/Invoke 声明 CoUninitialize
        [DllImport("Ole32.dll")]
        public static extern void CoUninitialize();

        [DllImport("Shell32.dll")]
        public static extern int SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags);
        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);


        // 引用 GetWindowText API
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        // 引用 GetWindowLong API
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
                                           
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        // 引用 SetFocus API
        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePolygonRgn(POINT[] lppt, int cPoints, int fnPolyFillMode);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        // Win32 API 定义
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);

        // Win32 API 调用
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        //[DllImport("dwmapi.dll")]
        //public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int pvAttr, int cbAttr);
        // Import dwmapi.dll and define DwmSetWindowAttribute in C# corresponding to the native function.
        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd,
            DWMWINDOWATTRIBUTE attribute,
            ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute,
            uint cbAttribute);
        // Win32 API 定义
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        // EnumChildWindows 的回调函数定义
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        //[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static uint GetLastError() { return (uint)Marshal.GetLastWin32Error(); }
        

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint FormatMessage(
            uint dwFlags,
            IntPtr lpSource,
            uint dwMessageId,
            uint dwLanguageId,
            StringBuilder lpBuffer,
            uint nSize,
            IntPtr Arguments);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern ushort RegisterClass(ref WNDCLASS lpWndClass);


        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern ushort RegisterClassEx(ref WNDCLASSEX lpwcx);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool GetClassInfoEx(
               IntPtr hInstance,
               string lpszClass,
               out WNDCLASSEX lpwcx
           );

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool GetClassInfo(IntPtr hInstance, string lpClassName, out WNDCLASS lpWndClass);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateWindowEx(
            int dwExStyle, string lpClassName, string lpWindowName, int dwStyle,
            int x, int y, int nWidth, int nHeight,
            IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // ShowWindow 参数常量
        public const int SW_HIDE = 0;    // 隐藏窗口
        public const int SW_SHOW = 5;    // 显示窗口

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, string lpCursorName);

        [DllImport("user32.dll")]
        public static extern bool InvalidateRgn(IntPtr hWnd, IntPtr hRgn, bool bErase);

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool GetModuleHandleEx(
            GetModuleHandleExFlags dwFlags,
            string lpModuleName,
            out IntPtr phModule
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpLibFileName);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryW(string lpLibFileName);

        // 导入 GetProcAddress 函数
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        // 导入 FreeLibrary 函数
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        // 定义一个委托来表示要调用的函数
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int AddNumbersDelegate(int a, int b);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        public static extern bool IsProcessorFeaturePresent(uint ProcessorFeature);

        [DllImport("kernel32.dll")]
        public static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentProcessId();

        [DllImport("kernel32.dll")]
        public static extern void GetSystemTimeAsFileTime(out long lpSystemTimeAsFileTime);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll")]
        public static extern IntPtr HeapAlloc(IntPtr hHeap, uint dwFlags, IntPtr dwBytes);

        [DllImport("kernel32.dll")]
        public static extern bool HeapFree(IntPtr hHeap, uint dwFlags, IntPtr lpMem);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcessHeap();

        [DllImport("kernel32.dll")]
        public static extern bool IsDebuggerPresent();

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr SetWindowLongPtrW(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorW(IntPtr hInstance, int lpCursorName);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern bool PostQuitMessage(int nExitCode);

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProcW(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr CreateFontW(
           int nHeight, int nWidth, int nEscapement, int nOrientation,
           int fnWeight, uint fdwItalic, uint fdwUnderline, uint fdwStrikeOut,
           uint fdwCharSet, uint fdwOutputPrecision, uint fdwClipPrecision,
           uint fdwQuality, uint fdwPitchAndFamily, string lpszFace);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int FillRect(IntPtr hDC, ref Rectangle lprc, IntPtr hbr);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowLongPtrW(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int LoadStringW(IntPtr hInstance, uint uID, StringBuilder lpBuffer, int nBufferMax);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetMessageW(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage(ref MSG lpMsg);

        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessageW(ref MSG lpMsg);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern ushort RegisterClassExW(ref WNDCLASSEX lpwcx);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr BeginPaint(IntPtr hWnd, out PAINTSTRUCT lpPaint);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr DialogBoxParamW(IntPtr hInstance, IntPtr lpTemplateName, IntPtr hWndParent, DialogProc lpDialogFunc, IntPtr dwInitParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool EndDialog(IntPtr hDlg, IntPtr nResult);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern ushort RegisterClassW(ref WNDCLASS lpWndClass);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CallWindowProcW(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr LoadIconW(IntPtr hInstance, IntPtr lpIconName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool TranslateAcceleratorW(IntPtr hWnd, IntPtr hAccTable, ref MSG lpMsg);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr LoadAcceleratorsW(IntPtr hInstance, IntPtr lpTableName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr GetModuleHandleW(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr HeapAlloc(IntPtr hHeap, uint dwFlags, UIntPtr dwBytes);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int VirtualQuery(IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, UIntPtr dwLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern void RaiseException(uint dwExceptionCode, uint dwExceptionFlags, uint nNumberOfArguments, IntPtr lpArguments);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int WideCharToMultiByte(uint CodePage, uint dwFlags, string lpWideCharStr, int cchWideChar, StringBuilder lpMultiByteStr, int cbMultiByte, IntPtr lpDefaultChar, IntPtr lpUsedDefaultChar);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int MultiByteToWideChar(uint CodePage, uint dwFlags, string lpMultiByteStr, int cbMultiByte, StringBuilder lpWideCharStr, int cchWideChar);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateSolidBrush(uint crColor);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool Ellipse(IntPtr hdc, int left, int top, int right, int bottom);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr GetStockObject(int fnObject);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int SetBkMode(IntPtr hdc, int iBkMode);

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern uint SetTextColor(IntPtr hdc, uint crColor);

        [DllImport("gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool TextOutW(IntPtr hdc, int x, int y, string lpString, int c);


        #endregion

        #region NotifyIcon related

        public const int NIM_ADD = 0x00000000;
        public const int NIM_MODIFY = 0x00000001;
        public const int NIM_DELETE = 0x00000002;
        public const int NIF_MESSAGE = 0x00000001;
        public const int NIF_ICON = 0x00000002;
        public const int NIF_TIP = 0x00000004;

        public const int WM_TRAYICON = WM_USER + 1;

        [StructLayout(LayoutKind.Sequential)]
        public struct NOTIFYICONDATA
        {
            public uint cbSize;
            public IntPtr hWnd;
            public uint uID;
            public uint uFlags;
            public uint uCallbackMessage;
            public IntPtr hIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szTip;
            public int dwState;
            public int dwStateMask;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szInfo;
            public int uTimeoutOrVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szInfoTitle;
            public int dwInfoFlags;
        }

        [DllImport("shell32.dll")]
        public static extern int Shell_NotifyIcon(int dwMessage, ref NOTIFYICONDATA lpdata);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr LoadImage(IntPtr hInst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr LoadIcon(IntPtr hInstance, int lpIconName);

        #endregion NotifyIcon related
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hWnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point pt;
    }

    [Flags]
    public enum GetModuleHandleExFlags : uint
    {
        GET_MODULE_HANDLE_EX_FLAG_PIN = 0x00000001,
        GET_MODULE_HANDLE_EX_FLAG_UNCHANGED_REFCOUNT = 0x00000002,
        GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS = 0x00000004
    }

    // 结构体定义
    [StructLayout(LayoutKind.Sequential)]
    public struct PAINTSTRUCT
    {
        public IntPtr hdc;
        public bool fErase;
        public Rectangle rcPaint;
        public bool fRestore;
        public bool fIncUpdate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] rgbReserved;
    }

    // 委托定义（例如用于 DialogProc）
    public delegate IntPtr DialogProc(IntPtr hwndDlg, uint uMsg, IntPtr wParam, IntPtr lParam);
    public enum DWMWINDOWATTRIBUTE
    {
        DWMWA_WINDOW_CORNER_PREFERENCE = 33
    }

    // The DWM_WINDOW_CORNER_PREFERENCE enum for DwmSetWindowAttribute's third parameter, which tells the function
    // what value of the enum to set.
    // Copied from dwmapi.h
    public enum DWM_WINDOW_CORNER_PREFERENCE
    {
        DWMWCP_DEFAULT = 0,
        DWMWCP_DONOTROUND = 1,
        DWMWCP_ROUND = 2,
        DWMWCP_ROUNDSMALL = 3
    }

    // POINT 结构体定义
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
        public POINT(int x, int y) { X = x; Y = y; }
    }

    // 必要的结构体定义
    [StructLayout(LayoutKind.Sequential)]
    public struct FILETIME
    {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SLIST_HEADER
    {
        public ulong Alignment;
        public ulong Region;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct STARTUPINFO
    {
        public uint cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public uint dwX;
        public uint dwY;
        public uint dwXSize;
        public uint dwYSize;
        public uint dwXCountChars;
        public uint dwYCountChars;
        public uint dwFillAttribute;
        public uint dwFlags;
        public ushort wShowWindow;
        public ushort cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_BASIC_INFORMATION
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public uint AllocationProtect;
        public IntPtr RegionSize;
        public uint State;
        public uint Protect;
        public uint Type;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WNDCLASS
    {
        public uint style;
        public IntPtr lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszMenuName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszClassName;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WNDCLASSEX
    {
        public uint cbSize;
        public int style;
        public IntPtr lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszMenuName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszClassName;
        public IntPtr hIconSm;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CREATESTRUCT
    {
        public IntPtr lpCreateParams;
        public IntPtr hInstance;
        public IntPtr hMenu;
        public IntPtr hwndParent;
        public int cy;
        public int cx;
        public int y;
        public int x;
        public int style;
        public IntPtr lpszName;
        public IntPtr lpszClass;
        public uint dwExStyle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CWPSTRUCT
    {
        public IntPtr lParam;
        public IntPtr wParam;
        public uint message;
        public IntPtr hwnd;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    };

    [Flags]
    public enum CoInit : uint
    {
        COINIT_APARTMENTTHREADED = 0x2, // 单线程单元 (STA)
        COINIT_MULTITHREADED = 0x0, // 多线程单元 (MTA)
        COINIT_DISABLE_OLE1DDE = 0x4,
        COINIT_SPEED_OVER_MEMORY = 0x8
    }
}
