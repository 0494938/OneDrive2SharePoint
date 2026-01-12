using GcjUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Point = System.Windows.Point;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedDragbleStackPanel : System.Windows.Controls.StackPanel
    {
        #region GcjVer // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedDragbleStackPanel),
            new PropertyMetadata("Gcj CustomizedDragble StackPanel v1.0"));


        [Description("Gets or sets the CustomizedDragbleStackPanel Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer // 定义 GcjVer 依赖属性

        public CustomizedDragbleStackPanel() : base()
        {
            //DefaultStyleKey = typeof(CustomizedImageButton);
            Loaded += CustomizedDragbleStackPanel_Loaded;
        }


        private void CustomizedDragbleStackPanel_LayoutUpdated(object? sender, EventArgs e)
        {
        }

        System.Windows.Window? window = null;
        private void CustomizedDragbleStackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("$$$$ CustomizedDragbleStackPanel_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            Debug.WriteLine("  Width=" + Width + ", Height=" + Height + ", ActualWidth=" + ActualWidth + ", ActualHeight=" + ActualHeight);
            window = DepUtil.FindParent<System.Windows.Window>(this);
            this.MouseLeftButtonDown += CustomizedDragbleStackPanel_MouseLeftButtonDown;
            if (window != null)
            {
                window.MouseLeftButtonDown += CustomizedDragbleStackPanel_MouseLeftButtonDown;
            }
        }

        private void CustomizedDragbleStackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("CustomizedDragbleStackPanel_MouseLeftButtonDown(" + sender?.GetType()?.Name + " - " + (sender as FrameworkElement)?.Name + ", MouseButtonEventArgs(" + e.ToString() + "))");
            if (e.LeftButton == MouseButtonState.Pressed && sender is System.Windows.Window)
            {
                Point mousePosition = e.GetPosition(this);
                Debug.WriteLine($"CustomizedDragbleStackPanel_MouseLeftButtonDown, Mouse Position relative to Current Dragging{this.GetType().Name}, X:{mousePosition.X}, Y:{mousePosition.Y}");

                // 假设 targetElement 是你要检测的某个控件，例如一个 Button
                if (IsMouseOverTargetArea(this, mousePosition))
                {
                    Debug.WriteLine("鼠标点击在CustomizedDragbleStackPanel区域内");
                    (sender as Window)?.DragMove();
                }
                else if (!IsMouseOverTargetArea(this.Parent as UIElement, mousePosition))
                {
                    Debug.WriteLine("鼠标点击在Border边框区域内");
                    (sender as Window)?.DragMove();
                }
            }
            else if (e.LeftButton == MouseButtonState.Pressed && sender is CustomizedDragbleStackPanel)
            {
                System.Windows.Window? wnd = DepUtil.FindParent<System.Windows.Window>(this);
                wnd?.DragMove();
            }
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
