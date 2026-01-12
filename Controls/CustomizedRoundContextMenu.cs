using System.ComponentModel;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedRoundContextMenu : System.Windows.Controls.ContextMenu
    {
        #region GcjVer // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedRoundContextMenu),
            new PropertyMetadata("Gcj Round Context Menu v1.0"));


        [Description("Gets or sets the CustomizedRoundContextMenu Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer // 定义 GcjVer 依赖属性

        public CustomizedRoundContextMenu() : base()
        {
        }
    }
}
