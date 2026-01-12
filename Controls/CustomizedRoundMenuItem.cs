using System.ComponentModel;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedRoundMenuItem : System.Windows.Controls.ContextMenu
    {
        #region GcjVer // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedRoundMenuItem),
            new PropertyMetadata("Gcj Menu Item v1.0"));


        [Description("Gets or sets the CustomizedRoundMenuItem Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer // 定义 GcjVer 依赖属性

        public CustomizedRoundMenuItem() : base()
        {
        }
    }
}
