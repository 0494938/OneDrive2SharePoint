using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedTreeView : System.Windows.Controls.TreeView
    {
        public CustomizedTreeView() : base()
        {
            //DefaultStyleKey = typeof(CustomizedTreeView);
            Loaded += CustomizedTreeView_Loaded;
        }

        private void CustomizedTreeView_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}: private void CustomizedTreeView_Loaded Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");
            //Debug.WriteLine("$$$$ CustomizedTreeView_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            //Debug.WriteLine("  Width=" + this.Width + ", Height=" + this.Height + ", ActualWidth=" + this.ActualWidth + ", ActualHeight=" + this.ActualHeight);
        }

        #region Properties

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedTreeView),
            new PropertyMetadata("Gcj TreeView v1.0"));


        [Description("Gets or sets the CustomizedTreeView Version")]
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
