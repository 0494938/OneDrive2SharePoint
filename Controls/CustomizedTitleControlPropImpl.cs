using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace GcjUiCtrl.Control
{
    public partial class CustomizedTitleControl : System.Windows.Controls.ContentControl
    {

        #region GcjVer
        public static readonly DependencyProperty GcjVerProperty = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedTitleControl),
            new PropertyMetadata("Gcj ToolStyle Title Control v1.0"));

        [Description("Gets or sets the Customized Title Control Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(GcjVerProperty); }
            set { SetValue(GcjVerProperty, value); }
        }
        #endregion GcjVer

        #region ShowBkColorPicker
        public static readonly DependencyProperty ShowBkColorPickerProperty = DependencyProperty.Register(
            "ShowBkColorPicker",
            typeof(Visibility),
            typeof(CustomizedTitleControl),
            new PropertyMetadata(Visibility.Hidden));

        [Description("Gets or sets the Customized Title Control Show Color Picker Or Not")]
        [Category("GcjControl")]
        public Visibility ShowBkColorPicker
        {
            get { return (Visibility)GetValue(ShowBkColorPickerProperty); }
            set { SetValue(ShowBkColorPickerProperty, value); }
        }
        #endregion ShowBkColorPicker   

        #region ShowMinimiumBox
        public static readonly DependencyProperty ShowMinimumButtonProperty = DependencyProperty.Register(
            "ShowMinimumButton",
            typeof(Visibility),
            typeof(CustomizedTitleControl),
            new PropertyMetadata(Visibility.Hidden));

        [Description("Gets or sets the Customized Title Control Minimum Button")]
        [Category("GcjControl")]
        public Visibility ShowMinimumButton
        {
            get { return (Visibility)GetValue(ShowMinimumButtonProperty); }
            set { SetValue(ShowMinimumButtonProperty, value); }
        }
        #endregion ShowMinimiumBox    

        #region ShowMaximumBox
        public static readonly DependencyProperty ShowMaximumButtonProperty = DependencyProperty.Register(
            "ShowMaximumButton",
            typeof(Visibility),
            typeof(CustomizedTitleControl),
            new PropertyMetadata(Visibility.Hidden));

        [Description("Gets or sets the Customized Title Control Maximum Button")]
        [Category("GcjControl")]
        public Visibility ShowMaximumButton
        {
            get { return (Visibility)GetValue(ShowMaximumButtonProperty); }
            set { SetValue(ShowMaximumButtonProperty, value); }
        }
        #endregion ShowMaximumBox    

        #region ShowCloseButton
        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(
            "ShowCloseButton",
            typeof(Visibility),
            typeof(CustomizedTitleControl),
            new PropertyMetadata(Visibility.Visible));

        [Description("Gets or sets the Customized Title Control Close Button")]
        [Category("GcjControl")]
        public Visibility ShowCloseButton
        {
            get { return (Visibility)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, value); }
        }
        #endregion ShowCloseButton    

        #region TitleText
        public static readonly DependencyProperty TitleTextProperty = DependencyProperty.Register(
            "TitleText",
            typeof(string),
            typeof(CustomizedTitleControl),
            new PropertyMetadata(null));

        [Description("Gets or sets the Customized Title Control Text Property")]
        [Category("GcjControl")]
        public string TitleText
        {
            get { return (string)GetValue(TitleTextProperty); }
            set { SetValue(TitleTextProperty, value); }
        }
        #endregion TitleText

        #region TitleFontSize
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register(
            "TitleFontSize",
            typeof(double),
            typeof(CustomizedTitleControl),
            new PropertyMetadata((double)14));

        [Description("Gets or sets the Customized Title Control Text FontSize")]
        [Category("GcjControl")]
        public double TitleFontSize
        {
            get { return (double)GetValue(TitleFontSizeProperty); }
            set { SetValue(TitleFontSizeProperty, value); }
        }
        #endregion ColorPickerFontSize   

        #region SysButtonBackground
        public static readonly DependencyProperty SysButtonBackgroundProperty = DependencyProperty.Register(
            "SysButtonBackground",
            typeof(System.Windows.Media.Brush),
            typeof(CustomizedTitleControl),
            new PropertyMetadata(new SolidColorBrush(Colors.LightGreen)));

        [Description("Gets or sets the Customized Title Control Min/Max/Close Button BackGroundColor")]
        [Category("GcjControl")]
        public System.Windows.Media.Brush SysButtonBackground
        {
            get { return (System.Windows.Media.Brush)GetValue(SysButtonBackgroundProperty); }
            set { SetValue(SysButtonBackgroundProperty, value); }
        }
        #endregion SysButtonBackground    

        #region SysButtonForeground
        public static readonly DependencyProperty SysButtonForegroundProperty = DependencyProperty.Register(
            "SysButtonForeground",
            typeof(System.Windows.Media.Brush),
            typeof(CustomizedTitleControl),
            new PropertyMetadata(null));

        [Description("Gets or sets the Customized Title Control Min/Max/Close Button ForeGroundColor")]
        [Category("GcjControl")]
        public System.Windows.Media.Brush SysButtonForeground
        {
            get { return (System.Windows.Media.Brush)GetValue(SysButtonForegroundProperty); }
            set { SetValue(SysButtonForegroundProperty, value); }
        }
        #endregion SysButtonForeground        

        #region TitleForeGround
        public static readonly DependencyProperty TitleForeGroundProperty = DependencyProperty.Register(
            "TitleForeGround",
            typeof(System.Windows.Media.Brush),
            typeof(CustomizedTitleControl),
            new PropertyMetadata(new SolidColorBrush(Colors.White)));

        [Description("Gets or sets the Customized Title Control TextArea ForeGround")]
        [Category("GcjControl")]
        public System.Windows.Media.Brush TitleForeGround
        {
            get { return (System.Windows.Media.Brush)GetValue(TitleForeGroundProperty); }
            set { SetValue(TitleForeGroundProperty, value); }
        }
        #endregion TitleForeGround   

        #region TitleImageSource
        public static readonly DependencyProperty TitleImageSourceProperty = DependencyProperty.Register(
            "TitleImageSource",
            typeof(System.Windows.Media.ImageSource),
            typeof(CustomizedTitleControl),
            new PropertyMetadata(null));

        [Description("Gets or sets the Customized Title Control Image Icon Source")]
        [Category("GcjControl")]
        public System.Windows.Media.ImageSource TitleImageSource
        {
            get { return (System.Windows.Media.ImageSource)GetValue(TitleImageSourceProperty); }
            set { SetValue(TitleImageSourceProperty, value); }
        }
        #endregion TitleImageSource   

        #region TitleIconMargin
        public static readonly DependencyProperty TitleIconMarginProperty = DependencyProperty.Register(
            "TitleIconMargin",
            typeof(Thickness),
            typeof(CustomizedTitleControl),
            new PropertyMetadata(new Thickness(-10,-10,-10,-10)));

        [Description("Gets or sets the Customized Title Control Icon Image Margin")]
        [Category("GcjControl")]
        public Thickness TitleIconMargin
        {
            get { return (Thickness)GetValue(TitleIconMarginProperty); }
            set { SetValue(TitleIconMarginProperty, value); }
        }
        #endregion TitleIconMargin   

        #region ColorPickerFontSize
        public static readonly DependencyProperty ColorPickerFontSizeProperty = DependencyProperty.Register(
            "ColorPickerFontSize",
            typeof(double),
            typeof(CustomizedTitleControl),
            new PropertyMetadata((double)12));

        [Description("Gets or sets the Customized Title Control ColorPicker FontSize")]
        [Category("GcjControl")]
        public double ColorPickerFontSize
        {
            get { return (double)GetValue(ColorPickerFontSizeProperty); }
            set { SetValue(ColorPickerFontSizeProperty, value); }
        }
        #endregion ColorPickerFontSize   

        #region ColorPickerMargin
        public static readonly DependencyProperty ColorPickerMarginProperty = DependencyProperty.Register(
            "ColorPickerMargin",
            typeof(Thickness),
            typeof(CustomizedTitleControl),
            new PropertyMetadata(new Thickness(0, -3, 12, 10)));

        [Description("Gets or sets the Customized Title Control ColorPicker Margin")]
        [Category("GcjControl")]
        public Thickness ColorPickerMargin
        {
            get { return (Thickness)GetValue(ColorPickerMarginProperty); }
            set { SetValue(ColorPickerMarginProperty, value); }
        }
        #endregion ColorPickerMargin   

        #region SelectedColor // 定义 SelectedColor 依赖属性
        public static readonly DependencyProperty SelectedColorProperty =
        DependencyProperty.Register("SelectedColor", typeof(Color), typeof(CustomizedTitleControl), new PropertyMetadata(null));

        public Color SelectedColor
        {
            get {
                if (dpColor != null && dpColor.SelectedColor!=null) return (Color) dpColor.SelectedColor;
                else return (Color)GetValue(SelectedColorProperty); 
            }
            set{
                SetValue(SelectedColorProperty, value);
                if(dpColor!=null) dpColor.SelectedColor = value;
            }
        }
        #endregion SelectedColor

        #region ColorPickerHeight
        public static readonly DependencyProperty ColorPickerHeightProperty = DependencyProperty.Register(
            "ColorPickerHeight",
            typeof(double),
            typeof(CustomizedTitleControl),
            new PropertyMetadata((double)12));

        [Description("Gets or sets the Customized Title Control ColorPicker FontSize")]
        [Category("GcjControl")]
        public double ColorPickerHeight
        {
            get { return (double)GetValue(ColorPickerHeightProperty); }
            set { SetValue(ColorPickerHeightProperty, value); }
        }
        #endregion ColorPickerHeight   

        #region ColorPickerWidth
        public static readonly DependencyProperty ColorPickerWidthProperty = DependencyProperty.Register(
            "ColorPickerWidth",
            typeof(double),
            typeof(CustomizedTitleControl),
            new PropertyMetadata((double)12));

        [Description("Gets or sets the Customized Title Control ColorPicker FontSize")]
        [Category("GcjControl")]
        public double ColorPickerWidth
        {
            get { return (double)GetValue(ColorPickerWidthProperty); }
            set { SetValue(ColorPickerWidthProperty, value); }
        }
        #endregion ColorPickerWidth   

        #region SysButtonSize
        public static readonly DependencyProperty SysButtonSizeProperty = DependencyProperty.Register(
            "SysButtonSize",
            typeof(double),
            typeof(CustomizedTitleControl),
            new PropertyMetadata((double)12));

        [Description("Gets or sets the Customized Title Control ColorPicker FontSize")]
        [Category("GcjControl")]
        public double SysButtonSize
        {
            get { return (double)GetValue(SysButtonSizeProperty); }
            set { SetValue(SysButtonSizeProperty, value); }
        }
        #endregion SysButtonSize   

        #region StartIconAnimation // 定义 StartAnimation 依赖属性
        public static readonly DependencyProperty StartIconAnimationProperty =
            DependencyProperty.Register("StartIconAnimation", typeof(bool), typeof(CustomizedTitleControl), new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedTitleControl StartIconAnimation")]
        [Category("GcjControl")]
        public bool StartIconAnimation
        {
            get
            {
                return (bool)GetValue(StartIconAnimationProperty);
            }
            set
            {
                SetValue(StartIconAnimationProperty, value);
            }
        }
        #endregion StartAnimation

        //#region ColorPickerVisible // 定义 ColorPickerVisible 依赖属性
        //public static readonly DependencyProperty ColorPickerVisibleProperty =
        //    DependencyProperty.Register("ColorPickerVisible", typeof(bool), typeof(CustomizedTitleControl), new PropertyMetadata(false));

        //[Description("Gets or sets the CustomizedTitleControl ColorPickerVisible")]
        //[Category("GcjControl")]
        //public bool ColorPickerVisible
        //{
        //    get
        //    {
        //        return (bool)GetValue(ColorPickerVisibleProperty);
        //    }
        //    set
        //    {
        //        SetValue(ColorPickerVisibleProperty, value);
        //        if(dpColor!=null)
        //            dpColor.Visibility = ColorPickerVisible ? Visibility.Visible : Visibility.Collapsed;
        //    }
        //}
        //#endregion ColorPickerVisible
        
        //#region ShowMaxButton // 定义 ShowMaxButton 依赖属性
        //public static readonly DependencyProperty ShowMaxButtonProperty =
        //    DependencyProperty.Register("ShowMaxButton", typeof(Visibility), typeof(CustomizedTitleControl), new PropertyMetadata(false));

        //[Description("Gets or sets the CustomizedTitleControl ShowMaxButton")]
        //[Category("GcjControl")]
        //public Visibility ShowMaxButton
        //{
        //    get
        //    {
        //        return (Visibility)GetValue(ShowMaxButtonProperty);
        //    }
        //    set
        //    {
        //        SetValue(ShowMaxButtonProperty, value);
        //    }
        //}
        //#endregion ShowMaxButton

        //#region ShowMinButton // 定义 ShowMinButton 依赖属性
        //public static readonly DependencyProperty ShowMinButtonProperty =
        //    DependencyProperty.Register("ShowMinButton", typeof(Visibility), typeof(CustomizedTitleControl), new PropertyMetadata(false));

        //[Description("Gets or sets the CustomizedTitleControl ShowMinButton")]
        //[Category("GcjControl")]
        //public Visibility ShowMinButton
        //{
        //    get
        //    {
        //        return (Visibility)GetValue(ShowMinButtonProperty);
        //    }
        //    set
        //    {
        //        SetValue(ShowMinButtonProperty, value);
        //    }
        //}
        //#endregion ShowMinButton

    }
}
