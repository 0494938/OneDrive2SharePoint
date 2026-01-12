using GcjUiCtrl.Control;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace IsidTestApp
{

    /// <summary>
    /// TestControlWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MitsubishiTMigrateDataToSP : GcjBaseWindow
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

        void InitSrcFolder(TestContext context)
        {
            // 构造示例数据
            var root = new SrcNodeViewModel { Name = "folder1", IsFolder = true, IsChecked = true, };
            var sub = new SrcNodeViewModel { Name = "subfolder11", IsFolder = true, Parent = root };
            sub.Children.Add(new SrcNodeViewModel { Name = "file111", IsFolder = false, Parent = sub });
            sub.Children.Add(new SrcNodeViewModel { Name = "file112", IsFolder = false, Parent = sub });
            sub.Children.Add(new SrcNodeViewModel { Name = "file113", IsFolder = false, Parent = sub });
            root.Children.Add(sub);
            context.SrcFolders.Add(root);

            root = new SrcNodeViewModel { Name = "folder2", IsFolder = true, IsChecked = true, };
            root.Children.Add(new SrcNodeViewModel { Name = "file211", IsFolder = false, Parent = sub });
            root.Children.Add(new SrcNodeViewModel { Name = "file212", IsFolder = false, Parent = sub });
            root.Children.Add(new SrcNodeViewModel { Name = "file213", IsFolder = false, Parent = sub });
            context.SrcFolders.Add(root);

            root = new SrcNodeViewModel { Name = "folder3", IsFolder = true, IsChecked = true, };
            sub = new SrcNodeViewModel { Name = "subfolder31", IsFolder = true, Parent = root };
            sub.Children.Add(new SrcNodeViewModel { Name = "file311", IsFolder = false, Parent = sub });
            sub.Children.Add(new SrcNodeViewModel { Name = "file312", IsFolder = false, Parent = sub });
            sub.Children.Add(new SrcNodeViewModel { Name = "file313", IsFolder = false, Parent = sub });
            root.Children.Add(sub);
            context.SrcFolders.Add(root);

            root = new SrcNodeViewModel { Name = "file1", IsFolder = false, IsChecked = true, };
            context.SrcFolders.Add(root);
            root = new SrcNodeViewModel { Name = "file2", IsFolder = false, IsChecked = true, };
            context.SrcFolders.Add(root);
            root = new SrcNodeViewModel { Name = "file3", IsFolder = false, IsChecked = true, };
            context.SrcFolders.Add(root);
        }

        void InitDstFolder(TestContext context)
        {
            // 构造示例数据
            var root = new DstNodeViewModel { Name = "folder1", IsFolder = true, Status = ProcessStatus.Completed, };
            var sub = new DstNodeViewModel { Name = "subfolder11", Status = ProcessStatus.Completed, Parent = root };
            sub.Children.Add(new DstNodeViewModel { Name = "file111", IsFolder = false, Parent = sub });
            sub.Children.Add(new DstNodeViewModel { Name = "file112", IsFolder = false, Parent = sub });
            sub.Children.Add(new DstNodeViewModel { Name = "file113", IsFolder = false, Parent = sub });
            root.Children.Add(sub);
            context.DstFolders.Add(root);

            root = new DstNodeViewModel { Name = "folder2", IsFolder = true, Status = ProcessStatus.Completed,};
            root.Children.Add(new DstNodeViewModel { Name = "file211", IsFolder = false, Parent = sub });
            root.Children.Add(new DstNodeViewModel { Name = "file212", IsFolder = false, Parent = sub });
            root.Children.Add(new DstNodeViewModel { Name = "file213", IsFolder = false, Parent = sub });
            context.DstFolders.Add(root);

            root = new DstNodeViewModel { Name = "folder3", IsFolder = true, Status = ProcessStatus.Processing, };
            sub = new DstNodeViewModel { Name = "subfolder31", IsFolder = true, Parent = root };
            sub.Children.Add(new DstNodeViewModel { Name = "file311", IsFolder = false, Parent = sub });
            sub.Children.Add(new DstNodeViewModel { Name = "file312", IsFolder = false, Parent = sub });
            sub.Children.Add(new DstNodeViewModel { Name = "file313", IsFolder = false, Parent = sub });
            root.Children.Add(sub);
            context.DstFolders.Add(root);

            root = new DstNodeViewModel { Name = "file1", IsFolder = false, Status = ProcessStatus.Pending, };
            context.DstFolders.Add(root);
            root = new DstNodeViewModel { Name = "file2", IsFolder = false, Status = ProcessStatus.Pending, };
            context.DstFolders.Add(root);
            root = new DstNodeViewModel { Name = "file3", IsFolder = false, Status = ProcessStatus.Pending, };
            context.DstFolders.Add(root);
        }

        public MitsubishiTMigrateDataToSP()
        {
            InitializeComponent();
            Loaded += IsidTestControlWindow_Loaded;
            Unloaded += IsidTestControlWindow_Unloaded;

            TestContext? context = this.DataContext as TestContext;
            if (context!=null)
            {
                InitSrcFolder(context);
                InitDstFolder(context);
            }
            //this.DataContext = this;
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

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink link) {
                Process.Start(new ProcessStartInfo(link.NavigateUri.ToString()) { UseShellExecute = true });
                e.Handled = true;
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }

        private void btnSrcBrowser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDstBrowser_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
