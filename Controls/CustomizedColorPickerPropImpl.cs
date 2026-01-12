using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

namespace GcjUiCtrl.Control
{
    public interface IColorPickerIdentfier
    {
        BindingExpression? ColorBindingExpression();
    }

    public partial class CustomizedColorPicker : Xceed.Wpf.Toolkit.ColorPicker, IColorPickerIdentfier
    {
        #region Properties
       
        #region GcjVer // 定义 ShowColorDetail 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
               "GcjVer",
               typeof(string),
               typeof(CustomizedColorPicker),
               new PropertyMetadata("Gcj ColorPicker v1.0"));

        [Description("Gets or sets the CustomizedColorPicker Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region ShowColorAndNameStreched // 定义 ShowColorAndNameStreched 依赖属性
        public static readonly DependencyProperty ShowColorAndNameStrechedProperty =
        DependencyProperty.Register("ShowColorAndNameStreched", typeof(bool), typeof(CustomizedColorPicker), new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedColorPicker ShowColorAndNameStreched, Change Default ShowColorAndNames Option Behavioral ")]
        [Category("GcjControl")]
        public bool ShowColorAndNameStreched
        {
            get { return (bool)GetValue(ShowColorAndNameStrechedProperty); }
            set { SetValue(ShowColorAndNameStrechedProperty, value); }
        }
        #endregion ShowColorAndNameStreched

        #region ShowColorDetail // 定义 ShowColorDetail 依赖属性
        public static readonly DependencyProperty ShowColorDetailProperty =
        DependencyProperty.Register("ShowColorDetail", typeof(bool), typeof(CustomizedColorPicker), new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedColorPicker ShowColorDetail")]
        [Category("GcjControl")]
        public bool ShowColorDetail
        {
            get { return (bool)GetValue(ShowColorDetailProperty); }
            set { SetValue(ShowColorDetailProperty, value); }
        }
        #endregion ShowColorDetail

        #region ShowColorDetailText // 定义 ShowColorDetailText 依赖属性
        public static readonly DependencyProperty ShowColorDetailTextProperty =
        DependencyProperty.Register("ShowColorDetailText", typeof(string), typeof(CustomizedColorPicker), new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedColorPicker ShowColorDetailText")]
        [Category("GcjControl")]
        public string ShowColorDetailText
        {
            get { return (string)GetValue(ShowColorDetailTextProperty); }
            set { SetValue(ShowColorDetailTextProperty, value); }
        }
        #endregion ShowColorDetailText

        #region TextForeground // 定义 TextForeground 依赖属性
        public static readonly DependencyProperty TextForegroundProperty =
        DependencyProperty.Register("TextForeground", typeof(Brush), typeof(CustomizedColorPicker), new PropertyMetadata(new SolidColorBrush( Colors.Black)));

        [Description("Gets or sets the CustomizedColorPicker TextForeground")]
        [Category("GcjControl")]
        public System.Windows.Media.Brush TextForeground
        {
            get {
                if (this.lblColorText != null && lblColorText?.Foreground!=null) 
                    return lblColorText?.Foreground;
                else 
                    return (Brush)GetValue(TextForegroundProperty);
            }
            set { SetValue(TextForegroundProperty, value);
                if(lblColorText!=null)
                    lblColorText.Foreground = value;
            }
        }
        #endregion TextForeground

        #endregion Properties
    }
}
