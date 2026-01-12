using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedMenuItem : System.Windows.Controls.MenuItem
    {
        public CustomizedMenuItem() : base()
        {
            //DefaultStyleKey = typeof(CustomizedMenuItem);
            Loaded += CustomizedMenuItem_Loaded;
        }

        private void CustomizedMenuItem_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}: private void CustomizedMenuItem_Loaded(object sender, RoutedEventArgs e)\r\n Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");
            //Debug.WriteLine("$$$$ CustomizedMenuItem_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            //Debug.WriteLine("  Width=" + this.Width + ", Height=" + this.Height + ", ActualWidth=" + this.ActualWidth + ", ActualHeight=" + this.ActualHeight);
        }

        #region Properties

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedMenuItem),
            new PropertyMetadata("Gcj MenuItem v1.0"));


        [Description("Gets or sets the CustomizedMenuItem Version")]
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
