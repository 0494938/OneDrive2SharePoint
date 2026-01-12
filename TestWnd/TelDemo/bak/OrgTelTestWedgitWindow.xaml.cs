using GcjUiCtrl.Control;
using GcjUtil;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace IsidTestApp
{
    /// <summary>
    /// TestControlWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class OrgTelTestWedgitWindow : GcjBaseWindow
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
        private Point _startPoint;

        public OrgTelTestWedgitWindow()
        {
            InitializeComponent();
            InitBrowser();

            //this.MouseLeftButtonDown += IsidTestControlWindow_MouseLeftButtonDown;
            //this.MouseMove += IsidTestWedgitWindow_MouseMove;
            //this.Loaded += IsidTestWedgitWindow_Loaded;

            this.ShowInTaskbar = true;

            // 设置窗口不能调整大小
            //this.ResizeMode = ResizeMode.NoResize;
            //_widgets = new ObservableCollection<Widget>();

            // ... 其他初始化代码

        }

        private void IsidTestWedgitWindow_Loaded(object sender, RoutedEventArgs e)
        {
         
        }

        private void IsidTestWedgitWindow_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(null);
                Left += pos.X - _startPoint.X;
                Top += pos.Y - _startPoint.Y;
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            // 显示窗口
            this.Show();
            this.WindowState = WindowState.Normal;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (WindowState == WindowState.Minimized)
            {
                // 隐藏窗口
                //this.Hide();
            }
        }

        private void IsidTestControlWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                _startPoint = e.GetPosition(this);
            }
        }

        public async void InitBrowser()
        {
            TestContext? datacontext = this.DataContext as TestContext;
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            TestContext? datacontext = this.DataContext as TestContext;
            if ((datacontext != null))
            {
                datacontext.IsFileFrameSelected = true;
                selectedFile.GetBindingExpression(System.Windows.Shapes.Rectangle.VisibilityProperty).UpdateTarget();
                selectedSystem.GetBindingExpression(System.Windows.Shapes.Rectangle.VisibilityProperty).UpdateTarget();
                grdFile.Visibility = Visibility.Visible;
                grdSystem.Visibility=Visibility.Hidden;
            }
        }

        private void btnSystem_Click(object sender, RoutedEventArgs e)
        {
            TestContext? datacontext = this.DataContext as TestContext;
            if ((datacontext != null))
            {
                datacontext.IsSystemFrameSelected = true;
                selectedFile.GetBindingExpression(System.Windows.Shapes.Rectangle.VisibilityProperty).UpdateTarget();
                selectedSystem.GetBindingExpression(System.Windows.Shapes.Rectangle.VisibilityProperty).UpdateTarget();
                grdFile.Visibility = Visibility.Hidden;
                grdSystem.Visibility = Visibility.Visible;
            }
        }

        private void btnShowAllRecentFile_Click(object sender, RoutedEventArgs e)
        {
            TestContext? datacontext = this.DataContext as TestContext;
            if ((datacontext != null))
            {
                datacontext.FileRecentShowAll = !datacontext.FileRecentShowAll;
                List<CustomizedFileInfoCtrl> fileInfos = DepUtil.GetAllChildrenOfType<CustomizedFileInfoCtrl>(grdFile);
                foreach (CustomizedFileInfoCtrl fileInfo in fileInfos)
                {
                    if (datacontext.FileRecentShowAll && fileInfo.ItemIndex <= 1000)
                    {
                        fileInfo.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        if (fileInfo.ItemIndex > 4 && fileInfo.ItemIndex <= 1000)
                            fileInfo.Visibility = Visibility.Collapsed;
                    }
                }
                //UpdateAllControlsVisual(grdFile);
                grdFile.UpdateLayout();
            }
        }

        private void btnShowAllShareFile_Click(object sender, RoutedEventArgs e)
        {
            TestContext? datacontext = this.DataContext as TestContext;
            if ((datacontext != null))
            {
                datacontext.FileSharedShowAll = !datacontext.FileSharedShowAll;
                //UpdateAllControlsVisual(grdFile);
                List<CustomizedFileInfoCtrl> fileInfos = DepUtil.GetAllChildrenOfType<CustomizedFileInfoCtrl>(grdFile);
                foreach (CustomizedFileInfoCtrl fileInfo in fileInfos)
                {
                    if (datacontext.FileSharedShowAll&& fileInfo.ItemIndex > 1000)
                    {
                        fileInfo.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        if (fileInfo.ItemIndex > 1004)
                            fileInfo.Visibility = Visibility.Collapsed;
                    }
                }
            }
            //UpdateAllControlsVisual(grdFile);
            grdFile.UpdateLayout();
        }
    }
}
