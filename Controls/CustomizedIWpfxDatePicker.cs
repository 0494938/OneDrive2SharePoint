using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Xceed.Wpf.Toolkit;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedTookKitDatePicker : Xceed.Wpf.Toolkit.DateTimeUpDown
    {
        public CustomizedTookKitDatePicker() : base()
        {
            //DefaultStyleKey = typeof(CustomizedTookKitDatePicker);
            Loaded += CustomizedTookKitDatePicker_Loaded;
        }

        private void CustomizedTookKitDatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}: private void CustomizedTookKitDatePicker_Loaded Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");
            //Debug.WriteLine("$$$$ CustomizedTookKitDatePicker_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            //Debug.WriteLine("  Width=" + this.Width + ", Height=" + this.Height + ", ActualWidth=" + this.ActualWidth + ", ActualHeight=" + this.ActualHeight);
        }

        #region Properties

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedTookKitDatePicker),
            new PropertyMetadata("Gcj Extented Tools Date Picker v1.0"));


        [Description("Gets or sets the CustomizedTookKitDatePicker Version")]
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
