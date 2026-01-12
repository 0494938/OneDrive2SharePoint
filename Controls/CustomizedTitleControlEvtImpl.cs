using BaseUtil;
using GcjUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;

namespace GcjUiCtrl.Control
{
    public partial class CustomizedTitleControl : System.Windows.Controls.ContentControl
    {
        #region MinButtonClicked
        // 注册一个路由事件
        public static readonly RoutedEvent MinButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "MinButtonClicked", // 事件名称
            RoutingStrategy.Bubble, // 路由策略：冒泡、隧道、直接
            typeof(RoutedEventHandler), // 事件处理程序的类型
            typeof(CustomizedTitleControl)); // 注册事件的类

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("Customized Title Control Minimum Button Click Event Handler")]
        [Category("GcjControl")]
        public event RoutedEventHandler MinButtonClicked
        {
            add { AddHandler(MinButtonClickedEvent, value); }
            remove { RemoveHandler(MinButtonClickedEvent, value); }
        }
        protected RoutedEventArgs RaiseMinButtonClicked(RoutedEventArgs e)
        {
            System.Windows.RoutedEventArgs newArgs = new System.Windows.RoutedEventArgs(MinButtonClickedEvent, this);// {RoutedEvent = MinButtonClickedEvent};
            RaiseEvent(newArgs);
            if (!newArgs.Handled)
                MinimizeWindow(this, e);
            return newArgs;
        }

        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            if (btnMin?.Visibility != Visibility.Hidden)
                RaiseMinButtonClicked(e);
        }


        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            Window? wnd = DepUtil.FindParent<Window>(this);
            if (wnd != null)
                wnd.WindowState = System.Windows.WindowState.Minimized;
        }

        #endregion MinButtonClicked

        // 触发路由事件的方式
        #region MaxButtonClicked
        // 注册一个路由事件
        public static readonly RoutedEvent MaxButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "MaxButtonClicked", // 事件名称
            RoutingStrategy.Bubble, // 路由策略：冒泡、隧道、直接
            typeof(RoutedEventHandler), // 事件处理程序的类型
            typeof(CustomizedTitleControl)); // 注册事件的类

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("Customized Title Control Maximum Button Click Event Handler")]
        [Category("GcjControl")]
        public event RoutedEventHandler MaxButtonClicked
        {
            add { AddHandler(MaxButtonClickedEvent, value); }
            remove { RemoveHandler(MaxButtonClickedEvent, value); }
        }

        protected RoutedEventArgs RaiseMaxButtonClicked(RoutedEventArgs e)
        {
            System.Windows.RoutedEventArgs newArgs = new System.Windows.RoutedEventArgs(MaxButtonClickedEvent, this);// {RoutedEvent = MinButtonClickedEvent};
            RaiseEvent(newArgs);
            if (!newArgs.Handled)
                MaximizeWindow(this, e);
            return newArgs;
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            Window? wnd = DepUtil.FindParent<Window>(this);
            if (wnd != null)
            {
                if (wnd.WindowState == System.Windows.WindowState.Maximized)
                    wnd.WindowState = System.Windows.WindowState.Normal;
                else
                    wnd.WindowState = System.Windows.WindowState.Maximized;
            }
        }

        private void BtnMax_Click(object sender, RoutedEventArgs e)
        {
            if (btnMax?.Visibility != Visibility.Hidden)
                RaiseMaxButtonClicked(e);
        }
        #endregion MaxButtonClicked

        #region CloseButtonClicked
        // 注册一个路由事件
        public static readonly RoutedEvent CloseButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "CloseButtonClicked", // 事件名称
            RoutingStrategy.Bubble, // 路由策略：冒泡、隧道、直接
            typeof(RoutedEventHandler), // 事件处理程序的类型
            typeof(CustomizedTitleControl)); // 注册事件的类

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("Customized Title Control Close Button Click Event Handler")]
        [Category("GcjControl")]
        public event RoutedEventHandler CloseButtonClicked
        {
            add { AddHandler(CloseButtonClickedEvent, value); }
            remove { RemoveHandler(CloseButtonClickedEvent, value); }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Window? wnd = DepUtil.FindParent<Window>(this);
            if (wnd != null)
                wnd.Close();
        }

        protected RoutedEventArgs RaiseCloseButtonClicked(RoutedEventArgs e)
        {
            System.Windows.RoutedEventArgs newArgs = new System.Windows.RoutedEventArgs(CloseButtonClickedEvent, this);// {RoutedEvent = MinButtonClickedEvent};
            Debug.WriteLine("CustomizedTitleControl::Before Raise CloseButtonClickedEvent Event");
            RaiseEvent(newArgs);
            Debug.WriteLine("CustomizedTitleControl::After Raise CloseButtonClickedEvent Event");
            if (!newArgs.Handled)
            {
                Debug.WriteLine("Routed Event Has not been marked as handled, so run else...");
                CloseWindow(this, e);
            }
            return newArgs;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("CustomizedTitleControl::BtnClose_Click fired");
            if (btnClose?.Visibility != Visibility.Hidden)
                RaiseCloseButtonClicked(e);
        }

        #endregion CloseButtonClicked

        #region MenuButtonClicked
        // 注册一个路由事件
        public static readonly RoutedEvent MenuButtonClickedEvent = EventManager.RegisterRoutedEvent(
            "MenuButtonClicked", // 事件名称
            RoutingStrategy.Bubble, // 路由策略：冒泡、隧道、直接
            typeof(RoutedEventHandler), // 事件处理程序的类型
            typeof(CustomizedTitleControl)); // 注册事件的类

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("Customized Title Control Menu Button/Image Click Event Handler")]
        [Category("GcjControl")]
        public event RoutedEventHandler MenuButtonClicked
        {
            add { AddHandler(MenuButtonClickedEvent, value); }
            remove { RemoveHandler(MenuButtonClickedEvent, value); }
        }
        
        protected RoutedEventArgs RaiseMenuButtonClicked(RoutedEventArgs e)
        {
            System.Windows.RoutedEventArgs newArgs = new System.Windows.RoutedEventArgs(MenuButtonClickedEvent, this);// {RoutedEvent = MinButtonClickedEvent};
            Debug.WriteLine("CustomizedTitleControl::Before Raise MenuButtonClickedEvent Event");
            RaiseEvent(newArgs);
            Debug.WriteLine("CustomizedTitleControl::After Raise MenuButtonClickedEvent Event");
            if (!newArgs.Handled)
            {
                Debug.WriteLine("Routed Event Has not been marked as handled, so run else...");
                CloseWindow(this, e);
            }
            return newArgs;
        }

        private void BtnMenu_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("CustomizedTitleControl::BtnMenu_Click fired");
            if (btnMenu?.Visibility != Visibility.Hidden)
                RaiseMenuButtonClicked(e);
        }

        #endregion MenuButtonClicked

        #region SelectedColorChangedEvent
        // 注册一个路由事件
        public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectedColorChanged", // 事件名称
            RoutingStrategy.Bubble, // 路由策略：冒泡、隧道、直接
            typeof(RoutedPropertyChangedEventHandler<Color?>), // 事件处理程序的类型
            typeof(CustomizedTitleControl)); // 注册事件的类

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("Customized Title Control SelectedColorChanged Event Handler")]
        [Category("GcjControl")]
        public event RoutedPropertyChangedEventHandler<Color?> SelectedColorChanged
        {
            add { AddHandler(SelectedColorChangedEvent, value); }
            remove { RemoveHandler(SelectedColorChangedEvent, value); }
        }

        // 触发路由事件的方式
        protected void RaiseSelectedColorChangedEvent(System.Windows.Media.Color? newvalue)
        {
            RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> newEventArgs = new RoutedPropertyChangedEventArgs<System.Windows.Media.Color?>(dpColor.SelectedColor, newvalue, SelectedColorChangedEvent);
            RaiseEvent(newEventArgs);
        }

        protected void RaiseSelectedColorChangedEvent(RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> newEventArgs = new RoutedPropertyChangedEventArgs<System.Windows.Media.Color?>(e.OldValue, e.NewValue, SelectedColorChangedEvent);
            RaiseEvent(newEventArgs);
        }


        private void DpColor_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            System.Windows.Media.Color newForeGroundColor = CtrlUtil.GetWhiteBlackForeGroundColor((System.Windows.Media.Color)e.NewValue);

            Window? topWnd = DepUtil.FindParent<Window>(this);
            if (topWnd != null)
            {
                System.Collections.Generic.List<System.Windows.Controls.Control> ctrls = DepUtil.GetAllChildrenOfTypeInterface<System.Windows.Controls.Control, IColorPickerIdentfier>(topWnd);
                foreach (System.Windows.Controls.Control ctl in ctrls)
                {
                    if (ctl is IColorPickerIdentfier && sender != ctl)
                    {
                        (ctl as IColorPickerIdentfier)?.ColorBindingExpression()?.UpdateTarget();
                    }
                }
            }
            if (txtTitle != null)
            {
                txtTitle.Foreground = new SolidColorBrush(newForeGroundColor);
                txtTitle.GetBindingExpression(TextBlock.ForegroundProperty)?.UpdateTarget();
            }
            RaiseSelectedColorChangedEvent(e);
        }
        #endregion SelectedColorChangedEvent

        #region DropFileSupport
        #region DropFilePath         // 定义 DropFilePath 依赖属性
        public static readonly DependencyProperty _DropFilePath = DependencyProperty.Register(
            "DropFilePath",
            typeof(string),
            typeof(CustomizedTitleControl),
            new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedTitleControl Drop File Path")]
        [Category("GcjControl")]
        public string? DropFilePath
        {
            get { return (string?)GetValue(_DropFilePath); }
            set { SetValue(_DropFilePath, value); }
        }
        #endregion DropFilePath


        #region OnFileDropped Event
        private void CustomizedTitleControl_DragOver(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void CustomizedTitleControl_DragEnter(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
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

        private void CustomizedTitleControl_Drop(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
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
            typeof(CustomizedTitleControl)); // 注册事件的类

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("Customized Title Control Drop File Event Handler")]
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
