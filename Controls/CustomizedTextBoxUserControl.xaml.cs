using GcjUiCtrl.Control;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace GcjUiCtrl.UiUtil
{
    /// <summary>
    /// CustomizedTextBox.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomizedTextBoxUserControl : UserControl
    {
        public CustomizedTextBoxUserControl()
        {
            InitializeComponent();
            Loaded += CustomizedTextBoxUserControl_Loaded;
        }

        private void CustomizedTextBoxUserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}: private void CustomizedTextBoxUserControl_Loaded Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");
        }
    }
}
