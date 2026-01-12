using BaseUtil;
using GcjUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using Point = System.Windows.Point;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedDragbleGrid : System.Windows.Controls.Grid
    {
        #region GcjVer // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedDragbleGrid),
            new PropertyMetadata("Gcj CustomizedDragble StackPanel v1.0"));


        [Description("Gets or sets the CustomizedDragbleGrid Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer // 定义 GcjVer 依赖属性

        public CustomizedDragbleGrid() : base()
        {
            //DefaultStyleKey = typeof(CustomizedImageButton);
            Loaded += CustomizedDragbleGrid_Loaded;
            Drop += CustomizedDragbleGrid_Drop;
            DragEnter += CustomizedDragbleGrid_DragEnter;
            DragOver += CustomizedDragbleGrid_DragOver;
        }


        private void CustomizedDragbleGrid_LayoutUpdated(object? sender, EventArgs e)
        {
        }

        System.Windows.Window? window = null;
        private void CustomizedDragbleGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("$$$$ CustomizedDragbleGrid_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            Debug.WriteLine("  Width=" + Width + ", Height=" + Height + ", ActualWidth=" + ActualWidth + ", ActualHeight=" + ActualHeight);
            window = DepUtil.FindParent<System.Windows.Window>(this);
            this.PreviewMouseLeftButtonDown += (sender, e) =>CustomizedDragbleGrid_MouseLeftButtonDown(this, sender, e);
            if (window != null)
            {
                window.PreviewMouseLeftButtonDown += (sender, e) => CustomizedDragbleGrid_MouseLeftButtonDown(this, sender, e);
            }
        }

        private static void HandleDragableControl(UIElement dragableControl, object sender, MouseButtonEventArgs e, bool bSkipEditableOrButtonCtrl = true)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender.GetType() == dragableControl.GetType())
            {
                Point mousePosition = e.GetPosition(dragableControl);
                if (IsMouseOverTargetArea(dragableControl, sender as UIElement, mousePosition))
                {
                    System.Windows.Window? wnd = DepUtil.FindParent<System.Windows.Window>(dragableControl);
                    wnd?.DragMove();
                }
            }
            else if (e.LeftButton == MouseButtonState.Pressed && sender is System.Windows.Window)
            {
                Point mousePosition = e.GetPosition(sender as Window);
                Debug.WriteLine($"{sender.GetType().Name}_MouseLeftButtonDown, Mouse Position relative to {sender.GetType().Name}, X:{mousePosition.X}, Y:{mousePosition.Y}");

                // 假设 targetElement 是你要检测的某个控件，例如一个 Button
                if (IsMouseOverTargetArea(dragableControl, dragableControl, mousePosition))
                {
                    Debug.WriteLine($"鼠标点击在{dragableControl.GetType().Name}区域内");
                    (sender as Window)?.DragMove();
                }
                //else
                //{
                //    Window window = DepUtil.FindParent<Window>(this);

                //    if (!IsMouseOverTargetArea(this.Parent as UIElement, mousePosition))
                //    {
                //        Debug.WriteLine("鼠标点击在Border边框区域内");
                //        (sender as Window)?.DragMove();
                //    }
                //}
            }
        }

        public static string GetUIObjectInfo(object? sender)
        {
            if (sender is Window)
                return $"sender: Class:{sender?.GetType()?.Name} (Base Class:{sender?.GetType()?.BaseType?.Name}, Title:{(sender as Window)?.Name}";
            else if (sender is FrameworkElement)
                return $"sender: Class:{sender?.GetType()?.Name} (Base Class:{sender?.GetType()?.BaseType?.Name}, Name:{(sender as FrameworkElement)?.Name}";
            else if (sender is ResourceDictionary)
                return $"sender: Class:{sender?.GetType()?.Name} (Base Class:{sender?.GetType()?.BaseType?.Name}, Name:{(sender as FrameworkElement)?.Name}";
            else if(sender==null)
                return "sender: null";
            else
                return $"sender: Class:{sender?.GetType()?.Name} (Base Class:{sender?.GetType()?.BaseType?.Name}, Name:#NoName)";
        }

        private void CustomizedDragbleGrid_MouseLeftButtonDown(CustomizedDragbleGrid dragableTargetControl, object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine($"CustomizedDragbleGrid_MouseLeftButtonDown({GetUIObjectInfo(sender)}, MouseButtonEventArgs:{e.ToString()})");
            HandleDragableControl(dragableTargetControl, sender, e);
        }

        private static bool IsMouseOverInputableControl(UIElement relativeToElem, UIElement? targetElement, Point mousePosition)
        {
            if (targetElement == null)
                return false;

            bool bInInputableArea = false;
            if (targetElement is TextBox || targetElement is RichTextBox || targetElement is HwndHost || targetElement is Button || targetElement is ButtonBase)
            {
                // 获取目标控件相对于窗口的矩形区域
                if (targetElement.IsVisible && targetElement.IsEnabled)
                {
                    Rect targetRect = new Rect(targetElement.TranslatePoint(new Point(0, 0), relativeToElem),
                                           new Size(targetElement.RenderSize.Width, targetElement.RenderSize.Height));
                    bInInputableArea = targetRect.Contains(mousePosition);
                }
            }
            else
            {
                List<UIElement> childs = DepUtil.GetDirectChildrenOfType<UIElement>(targetElement);
                foreach (UIElement child in childs)
                {
                    if (IsMouseOverInputableControl(relativeToElem, child, mousePosition))
                        bInInputableArea = true;
                    if (bInInputableArea)
                        break;
                }
            }

            // 判断鼠标是否在目标控件的区域内
            if (bInInputableArea == true) Debug.Assert(true);
            return bInInputableArea;
        }

        private static bool IsMouseOverTargetArea(UIElement relativeToElem, UIElement? targetElement, Point mousePosition)
        {
            if (targetElement == null)
                return false;
            // 获取目标控件相对于窗口的矩形区域
            Rect targetRect = new Rect(targetElement.TranslatePoint(new Point(0, 0), relativeToElem),
                                       new Size(targetElement.RenderSize.Width, targetElement.RenderSize.Height));
            // 判断鼠标是否在目标控件的区域内
            bool bInArea = targetRect.Contains(mousePosition);
            if (bInArea)
            {
                //check it's in Inputable sub-area
                List<UIElement> childs = DepUtil.GetDirectChildrenOfType<UIElement>(targetElement);
                foreach (UIElement child in childs)
                {
                    if (IsMouseOverInputableControl(relativeToElem, child, mousePosition))
                        bInArea = false;
                    if (bInArea == false)
                        break;
                }
            }
            if (bInArea == true) Debug.Assert(true);
            return bInArea;
        }

        #region DropFileSupport

        #region DropFilePath         // 定义 DropFilePath 依赖属性
        public static readonly DependencyProperty _DropFilePath = DependencyProperty.Register(
            "DropFilePath",
            typeof(string),
            typeof(CustomizedDragbleGrid),
            new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedDragbleGrid Drop File Path")]
        [Category("GcjControl")]
        public string? DropFilePath
        {
            get { return (string?)GetValue(_DropFilePath); }
            set { SetValue(_DropFilePath, value); }
        }
        #endregion DropFilePath

        #region OnFileDropped Event
        private void CustomizedDragbleGrid_DragOver(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void CustomizedDragbleGrid_DragEnter(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                // Allow copy action
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                // No valid data - disable drag effect
                e.Effects = DragDropEffects.None;
            }
        }

        private void CustomizedDragbleGrid_Drop(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
            {
                // Get the file paths from the dropped data
                string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                DropFilePath = droppedFiles.FirstOrDefault();

                RaiseOnFileDropped(e);
            }
        }

        // 注册一个路由事件
        public static readonly RoutedEvent OnFileDroppedEvent = EventManager.RegisterRoutedEvent(
            "OnFileDropped", // 事件名称
            RoutingStrategy.Bubble, // 路由策略：冒泡、隧道、直接
            typeof(GcjDragEventHandler), // 事件处理程序的类型
            typeof(CustomizedDragbleGrid)); // 注册事件的类

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("Customized CustomizedDragbleGrid Drop File Event Handler")]
        [Category("GcjControl")]
        public event GcjDragEventHandler OnFileDropped
        {
            add { AddHandler(OnFileDroppedEvent, value); }
            remove { RemoveHandler(OnFileDroppedEvent, value); }
        }
        protected void RaiseOnFileDropped(DragEventArgs e)
        {
            GcjDragEventArgs newArgs = new GcjDragEventArgs()
            {
                RoutedEvent = OnFileDroppedEvent,
                DropFiles = (string[])e.Data.GetData(DataFormats.FileDrop)

            };
            RaiseEvent(newArgs);
        }

        #endregion OnFileDropped Event

        #endregion DropFileSupport

    }
}
