using GcjUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace GcjUiCtrl.Control
{
    public partial class CustomizedStatusBarGrid : System.Windows.Controls.Grid
    {
        public CustomizedStatusBarGrid() : base()
        {
            //DefaultStyleKey = typeof(CustomizedTitleControl);
            //this.MouseDown += CustomizedTitleGrid_MouseDown;
            this.Loaded += CustomizedTitleControl_Loaded;
            this.MouseLeftButtonDown += CustomizedTitleControl_MouseLeftButtonDown;
        }

        private void CustomizedTitleControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void CustomizedTitleControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("CustomizedTitleControl_MouseLeftButtonDown(" + sender?.GetType()?.Name + " - " + (sender as FrameworkElement)?.Name + ", MouseButtonEventArgs(" + e.ToString() + "))");
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                System.Windows.Window? wnd = DepUtil.FindParent<System.Windows.Window>(this);
                wnd?.DragMove();
            }
        }

        #region GcjVer // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedStatusBarGrid),
            new PropertyMetadata("Gcj Grid based Status Bar v1.0"));


        [Description("Gets or sets the CustomizedStatusBarControl Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer // 定义 GcjVer 依赖属性
    }
}
