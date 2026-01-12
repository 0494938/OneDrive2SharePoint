using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedProgressBar : System.Windows.Controls.ProgressBar
    {
        public CustomizedProgressBar() : base()
        {
            //DefaultStyleKey = typeof(CustomizedProgressBar);
            Loaded += CustomizedProgressBar_Loaded;
        }

        private void CustomizedProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}:private void CustomizedProgressBar_Loaded(object sender, RoutedEventArgs e)\r\n Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");

            //Debug.WriteLine("$$$$ CustomizedProgressBar_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            //Debug.WriteLine("  Width=" + this.Width + ", Height=" + this.Height + ", ActualWidth=" + this.ActualWidth + ", ActualHeight=" + this.ActualHeight);
        }

        #region Properties

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedProgressBar),
            new PropertyMetadata("Gcj ProgressBar v1.0"));


        [Description("Gets or sets the CustomizedProgressBar Version")]
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
