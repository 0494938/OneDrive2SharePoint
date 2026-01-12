using GcjUtil;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;
using Point = System.Windows.Point;
using Size = System.Windows.Size;
namespace GcjUiCtrl.Control
{
    public partial class CustomizedTitleControl : System.Windows.Controls.ContentControl
    {

        System.Windows.Controls.Button? btnMin;
        System.Windows.Controls.Button? btnMax;
        System.Windows.Controls.Button? btnClose;
        System.Windows.Controls.Button? btnMenu;
        System.Windows.Window? window;
        ColorPicker? dpColor = null;
        System.Windows.Controls.TextBlock? txtTitle = null;

        public CustomizedTitleControl() : base()
        {
            DefaultStyleKey = typeof(CustomizedTitleControl);
            //this.MouseDown += CustomizedTitleGrid_MouseDown;
            this.PreviewMouseDown += CustomizedTitleGrid_PreviewMouseDown;
            this.Loaded += CustomizedTitleControl_Loaded;
            this.MouseLeftButtonDown += CustomizedTitleControl_MouseLeftButtonDown;
            this.Drop += CustomizedTitleControl_Drop;
            this.DragEnter += CustomizedTitleControl_DragEnter;
            this.DragOver += CustomizedTitleControl_DragOver;
        }

        private void CustomizedTitleControl_Loaded(object sender, RoutedEventArgs e)
        {
            btnMin = this.Template.FindName("btnMin", this) as System.Windows.Controls.Button;
            btnMax = this.Template.FindName("btnMax", this) as System.Windows.Controls.Button;
            btnClose = this.Template.FindName("btnClose", this) as System.Windows.Controls.Button;
            btnMenu = this.Template.FindName("btnMenu", this) as System.Windows.Controls.Button;
            window = DepUtil.FindParent<System.Windows.Window>(this);
            dpColor = this.Template.FindName("colorPicker", this) as ColorPicker;
            txtTitle = this.Template.FindName("txtTitle", this) as System.Windows.Controls.TextBlock;

            btnMin.Click += BtnMin_Click;
            btnMax.Click += BtnMax_Click;
            btnClose.Click += BtnClose_Click;
            btnMenu.Click += BtnMenu_Click;
            btnMenu.PreviewMouseLeftButtonDown += BtnMenu_PreviewMouseLeftButtonDown;

            if (window != null)
            {
                window.MouseDoubleClick += BtnMenu_MouseDoubleClick;
                window.MouseLeftButtonDown += CustomizedTitleControl_MouseLeftButtonDown;
            }
            if(dpColor!=null)//OnBackGroundColorChanged;
            {
                dpColor.SelectedColorChanged += DpColor_SelectedColorChanged;
            }
        }

        private void BtnMenu_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Window? wnd = DepUtil.FindParent<System.Windows.Window>(this);
            wnd?.DragMove();
        }

        private void CustomizedTitleControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("TitleBar_MouseDown(" + sender?.GetType()?.Name + " - " + (sender as FrameworkElement)?.Name + ", MouseButtonEventArgs(" + e.ToString() + "))");
            if (e.LeftButton == MouseButtonState.Pressed && sender is System.Windows.Window)
            {
                Point mousePosition = e.GetPosition(this);

                // 假设 targetElement 是你要检测的某个控件，例如一个 Button
                if (IsMouseOverTargetArea(this, mousePosition))
                {
                    Debug.WriteLine("鼠标点击在Title区域内");
                    (sender as Window)?.DragMove();
                }else if(!IsMouseOverTargetArea(this.Parent as UIElement, mousePosition)) {
                    Debug.WriteLine("鼠标点击在Border边框区域内");
                    (sender as Window)?.DragMove();
                }
            }
            else if (e.LeftButton == MouseButtonState.Pressed && sender is CustomizedTitleControl)
            {
                System.Windows.Window? wnd = DepUtil.FindParent<System.Windows.Window>(this);
                wnd?.DragMove();
            }
        }

        private void BtnMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("sender:" + sender?.GetType()?.Name + " ,  e.OriginalSource" + e.OriginalSource?.GetType()?.Name);
            System.Windows.Window? wnd = null;
            if (sender is System.Windows.Window)
                wnd = (System.Windows.Window)sender;
            else
                wnd = DepUtil.FindParent<Window>(this);

            bool isLogicTitleChild = DepUtil.IsDescendantOf(e.OriginalSource as DependencyObject, this);
            if (isLogicTitleChild)
            {
                if (wnd.WindowState == System.Windows.WindowState.Maximized)
                    wnd.WindowState = System.Windows.WindowState.Normal;
                else
                    wnd.WindowState = System.Windows.WindowState.Maximized;
            }
        }

        private void CustomizedTitleGrid_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
       
        }

        private bool IsMouseOverTargetArea(UIElement? targetElement, Point mousePosition)
        {
            if (targetElement == null)
                return false;
            // 获取目标控件相对于窗口的矩形区域
            Rect targetRect = new Rect(targetElement.TranslatePoint(new Point(0, 0), this),
                                       new Size(targetElement.RenderSize.Width, targetElement.RenderSize.Height));

            // 判断鼠标是否在目标控件的区域内
            return targetRect.Contains(mousePosition);
        }
    }
}
