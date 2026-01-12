using BaseUtil;
using GcjUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace GcjUiCtrl.Control
{
    public partial class CustomizedNativeContainer : System.Windows.Controls.Grid
    {
        public CustomizedNativeContainer() : base()
        {
            Loaded += CustomizedNativeContainer_Loaded;
            SizeChanged += CustomizedNativeContainer_SizeChanged;
            LayoutUpdated += CustomizedNativeContainer_LayoutUpdated;
        }

        private void RefreshLayOut(string evtInfo, bool RefreshAll=true)
        {
            double nActiveWidth = this.ActualWidth;
            double nActiveHeight = this.ActualHeight;
            if (WndCtrlHost != null && WndCtrlHost?.HWnd != null && WndCtrlHost?.HWnd != IntPtr.Zero)
            {
                var source = PresentationSource.FromVisual(this);
                if (source != null)
                {
                    var dpiScale = source.CompositionTarget.TransformToDevice;
                    nActiveWidth = (int)(nActiveWidth * dpiScale.M11);
                    nActiveHeight = (int)(ActualHeight * dpiScale.M22);
                }

                if(WndCtrlHost?.HWnd!=null && WndCtrlHost?.HWnd != IntPtr.Zero){
                    IntPtr hRgn = GcjWinApi.CreateRoundRectRgn(0, 0, (int)nActiveWidth, (int)nActiveHeight, (int)CornerRadius.TopLeft, (int)CornerRadius.BottomRight);
                    GcjWinApi.SetWindowRgn((IntPtr)WndCtrlHost?.HWnd, hRgn, true);
                    GcjWinApi.RedrawWindow((IntPtr)WndCtrlHost?.HWnd, IntPtr.Zero, IntPtr.Zero, GcjWinApi.RDW_INVALIDATE | GcjWinApi.RDW_ERASE);
                }

                Trace.WriteLine($"{evtInfo} Recreate Rgn and Update Width:{(int)nActiveWidth}, Height:{(int)nActiveHeight} with CornerRadius({CornerRadius.TopLeft}, {CornerRadius.BottomRight})");
            }
        }

        #region event handler
        private void CustomizedNativeContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshLayOut($"CustomizedNativeContainer_SizeChanged({NativeCtrlClsName})", true);
        }

        private void CustomizedNativeContainer_LayoutUpdated(object? sender, EventArgs e)
        {
            //RefreshLayOut("CustomizedNativeContainer_LayoutUpdated", false);
        }

        private void CustomizedNativeContainer_Loaded(object sender, RoutedEventArgs e)
        {
        }
        #endregion event handler

        #region properties
        #region GcjVer // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedNativeContainer),
            new PropertyMetadata("Gcj Native Control Container v1.0"));

        [Description("Gets or sets the CustomizedNativeContainer Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer // 定义 GcjVer 依赖属性

        #region IsTransparent // 定义 IsTransparent 依赖属性
        public static readonly DependencyProperty IsTransparentProperty =
            DependencyProperty.Register("IsTransparent", typeof(bool), typeof(CustomizedNativeContainer), new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedNativeContainer IsTransparent")]
        [Category("GcjControl")]
        public bool IsTransparent
        {
            get
            {
                return (bool)GetValue(IsTransparentProperty);
            }
            set
            {
                SetValue(IsTransparentProperty, value);
            }
        }
        #endregion IsTransparent

        #region CornerRadius // 定义 CornerRadius 依赖属性
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CustomizedNativeContainer),
                new FrameworkPropertyMetadata(new CornerRadius(0.0), FrameworkPropertyMetadataOptions.AffectsRender, OnCornerRadiusChanged));

        [Description("Gets or sets the CustomizedNativeContainer CornerRadius")]
        [Category("GcjControl")]
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomizedNativeContainer cont)
            {
                cont.UpdateLayout();
            }
        }
        #endregion CornerRadius

        #region NativeCtrlClsName // 定义 NativeCtrlClsName 依赖属性
        public static readonly DependencyProperty _NativeCtrlClsName = DependencyProperty.Register(
            "NativeCtrlClsName",
            typeof(string),
            typeof(CustomizedNativeContainer),
            new PropertyMetadata("Gcj Native Control Container v1.0"));

        [Description("Gets or sets the Native Control Window Class Name")]
        [Category("GcjControl")]
        public string NativeCtrlClsName
        {
            get { return (string)GetValue(_NativeCtrlClsName); }
            set { SetValue(_NativeCtrlClsName, value); }
        }
        #endregion NativeCtrlClsName // 定义 NativeCtrlClsName 依赖属性

        #region CustomizedNativeCtrlClsNameInEmptyContainer
        public static readonly DependencyProperty _CustomizedNativeCtrlClsNameInEmptyContainer = DependencyProperty.Register(
            "CustomizedNativeCtrlClsNameInEmptyContainer",
            typeof(string),
            typeof(CustomizedNativeContainer),
            new PropertyMetadata("EDIT"));

        [Description("Gets or sets the Native Control Window Class Name")]
        [Category("GcjControl")]
        string CustomizedNativeCtrlClsNameInEmptyContainer_ExplictValue = "EDIT";
        public string CustomizedNativeCtrlClsNameInEmptyContainer
        {
            get {
                if (string.Compare(NativeCtrlClsName, "EmptyContainer", true) == 0) {
                    CustomizedNativeWndCtrlHost? wndHost = WndCtrlHost;
                    Debug.Assert(this.Children?.Count　>=1);
                    List<UIElement> childs = DepUtil.GetDirectChildrenExceptType<CustomizedNativeWndCtrlHost>(this);
                    Debug.Assert(childs.Count <= 1);
                    if (childs.Count == 0 && wndHost != null)
                        return CustomizedNativeCtrlClsNameInEmptyContainer_ExplictValue;
                    foreach (UIElement cld in childs)
                    {
                        HwndHost? child = cld as HwndHost;
                        if (child != null)
                        {
                            return child.ToString()??"Invlid";
                        }
                    }
                    return (string)GetValue(_CustomizedNativeCtrlClsNameInEmptyContainer); 
                }else
                    return NativeCtrlClsName;
            }
            set { SetValue(_CustomizedNativeCtrlClsNameInEmptyContainer, value);
                CustomizedNativeCtrlClsNameInEmptyContainer_ExplictValue = (string)value;
            }
        }
        #endregion CustomizedNativeCtrlClsNameInEmptyContainer

        #region WndCtrlHost // 定义 CustomizedNativeWndCtrlHost 依赖属性
        CustomizedNativeWndCtrlHost? _WndCtrlHost=null;
        public CustomizedNativeWndCtrlHost? WndCtrlHost
        {
            get { return _WndCtrlHost; }
            set { _WndCtrlHost = value; }
        }
        #endregion CustomizedNativeWndCtrlHost // 定义 CustomizedNativeWndCtrlHost 依赖属性
        #endregion properties
    }
}
