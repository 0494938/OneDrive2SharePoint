using System.ComponentModel;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public class GcjDiffBaseWindow : GcjBaseWindow
    {
        public GcjDiffBaseWindow() : base()
        {
            //DefaultStyleKey = typeof(GcjBaseWindow);
            Loaded += GcjDiffBaseWindow_Loaded;
        }

        private void GcjDiffBaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        #region Properties

        #region AutoStartCompare // 定义 AutoStartCompare 依赖属性
        public static readonly DependencyProperty AutoStartCompareProperty =
            DependencyProperty.Register("AutoStartCompare", typeof(bool), typeof(GcjDiffBaseWindow), new PropertyMetadata(false));

        [Description("Gets or sets the GcjDiffBaseWindow AutoStartCompare")]
        [Category("GcjControl")]
        public bool AutoStartCompare
        {
            get
            {
                return (bool)GetValue(AutoStartCompareProperty);
            }
            set
            {
                SetValue(AutoStartCompareProperty, value);
            }
        }
        #endregion AutoStartCompare

        #region OnlyShowDiff // 定义 OnlyShowDiff 依赖属性
        public static readonly DependencyProperty OnlyShowDiffProperty =
            DependencyProperty.Register("OnlyShowDiff", typeof(bool), typeof(GcjDiffBaseWindow), new PropertyMetadata(false));

        [Description("Gets or sets the GcjDiffBaseWindow OnlyShowDiff")]
        [Category("GcjControl")]
        public bool OnlyShowDiff
        {
            get
            {
                return (bool)GetValue(OnlyShowDiffProperty);
            }
            set
            {
                SetValue(OnlyShowDiffProperty, value);
            }
        }
        #endregion OnlyShowDiff

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static new readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(GcjDiffBaseWindow),
            new PropertyMetadata("Gcj Window 1.0"));


        [Description("Gets or sets the GcjDiffBaseWindow Version")]
        [Category("GcjControl")]
        public new string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #endregion Properties
    }
}
