using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedContentPresenter : System.Windows.Controls.ContentPresenter
    {
        public CustomizedContentPresenter() : base()
        {
            //DefaultStyleKey = typeof(CustomizedContentPresenter);
            Loaded += CustomizedContentPresenter_Loaded;
        }


        private void CustomizedContentPresenter_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}: private void CustomizedContentPresenter_Loaded Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");
            //Debug.WriteLine("$$$$ CustomizedContentPresenter_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            //Debug.WriteLine("  Width=" + this.Width + ", Height=" + this.Height + ", ActualWidth=" + this.ActualWidth + ", ActualHeight=" + this.ActualHeight);
        }

        #region Properties

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedContentPresenter),
            new PropertyMetadata("Gcj ContentPresenter v1.0"));


        [Description("Gets or sets the CustomizedContentPresenter Version")]
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
