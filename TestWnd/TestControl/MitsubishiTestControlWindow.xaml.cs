using GcjUiCtrl.Control;
using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace IsidTestApp
{

    /// <summary>
    /// TestControlWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class IsidTestControlWindow : GcjBaseWindow
    {
        private void OnBackGroundColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            TestContext? datacontext = this.DataContext as TestContext;
            if (datacontext != null && e.NewValue != null)
            {
                datacontext.CustomizedBgColor = e.NewValue ?? (Colors.BlueViolet);
                RgnBorder.GetBindingExpression(Border.BackgroundProperty)?.UpdateTarget();
            }
        }

        private void StartStopAnimation(object sender, RoutedEventArgs e)
        {
            btnAnimationButton.StartAnimation = !btnAnimationButton.StartAnimation;
        }

        public IsidTestControlWindow()
        {
            InitializeComponent();
            Loaded += IsidTestControlWindow_Loaded;
            Unloaded += IsidTestControlWindow_Unloaded;
            // 初始化TextBox内容
            for (int j = 0; j < 100; j++)
            {
                AutoScrollTextBox.AppendText($"This is line {j + 1} with a long content to test horizontal scrolling. \n");
            }
        }

        private void IsidTestControlWindow_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void IsidTestControlWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void TextScrollView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}:TextScrollView_SizeChanged Triggered");
        }

        
        private void TextPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}:TextPanel_SizeChanged Triggered");
        }

        private void grdLeft_LayoutUpdated(object sender, EventArgs e)
        {
            Debug.WriteLine ($"grdLeft_LayoutUpdated: (ActualWidth:{grdLeft.ActualWidth}, ActualHeight:{grdLeft.ActualHeight})");
            //webBrowser.GetBindingExpression(HwndHost.MarginProperty)?.UpdateTarget();
        }

        private void gridTitle_LayoutUpdated(object sender, EventArgs e)
        {
            Debug.WriteLine($"gridTitle_LayoutUpdated: (ActualWidth:{gridTitle.ActualWidth}, ActualHeight:{gridTitle.ActualHeight})");
            //webBrowser.GetBindingExpression(HwndHost.MarginProperty)?.UpdateTarget();
        }

        private void GcjBaseWindow_LayoutUpdated(object sender, EventArgs e)
        {
            Debug.WriteLine($"GcjBaseWindow_LayoutUpdated: (ActualWidth:{grdLeft.ActualWidth}, ActualHeight:{grdLeft.ActualHeight})");
        }

        private void grdLeft_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"grdLeft_Loaded: (ActualWidth:{grdLeft.ActualWidth}, ActualHeight:{grdLeft.ActualHeight})");
        }

        private void gridTitle_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"gridTitle_Loaded: (ActualWidth:{gridTitle.ActualWidth}, ActualHeight:{gridTitle.ActualHeight})");
        }

        private void GcjBaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"GcjBaseWindow_Loaded: (ActualWidth:{this.ActualWidth}, ActualHeight:{this.ActualHeight})");
        }
    }
}
