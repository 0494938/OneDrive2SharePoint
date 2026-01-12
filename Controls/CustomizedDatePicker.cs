using System.ComponentModel;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedDatePicker : System.Windows.Controls.DatePicker
    {
        public CustomizedDatePicker() : base()
        {
            //DefaultStyleKey = typeof(CustomizedDatePicker);
            Loaded += CustomizedDatePicker_Loaded;
        }

        private void CustomizedDatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("$$$$ CustomizedDatePicker_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            //Debug.WriteLine("  Width=" + this.Width + ", Height=" + this.Height + ", ActualWidth=" + this.ActualWidth + ", ActualHeight=" + this.ActualHeight);
        }

        #region Properties

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedDatePicker),
            new PropertyMetadata("Gcj Date Picker v1.0"));


        [Description("Gets or sets the CustomizedDatePicker Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #endregion Properties
    }
}
