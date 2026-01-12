using GcjUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static GcjUtil.DepUtil;

namespace GcjUiCtrl.Control
{
    /// <summary>
    /// CustomizedColorPicker3.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomizedColorPickerUserControl : UserControl, IColorPickerIdentfier
    {
        public CustomizedColorPickerUserControl()
        {
            InitializeComponent();
            _clrPicker.SelectedColorChanged += _clrPicker_SelectedColorChanged;
            _clrPicker.PreviewMouseLeftButtonDown += _clrPicker_PreviewMouseLeftButtonDown;
            this.Loaded += CustomizedColorPickerUserControl_Loaded;
        }

        private bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(this);
        }

        private void CustomizedColorPickerUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}: private void CustomizedColorPickerUserControl_Loaded Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");
        }

        #region Properties
        #region GcjVer // 定义 SelectedColor 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
          "GcjVer",
          typeof(string),
          typeof(CustomizedColorPickerUserControl),
          new PropertyMetadata("Gcj UserControl based ColorPicker v1.0"));


        [Description("Gets or sets the CustomizedColorPicker Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region ShowColorDetail // 定义 ShowColorDetail 依赖属性
        public static readonly DependencyProperty ShowColorDetailProperty =
        DependencyProperty.Register("ShowColorDetail", typeof(bool), typeof(CustomizedColorPickerUserControl), new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedColorPicker ShowColorDetail")]
        [Category("GcjControl")]
        public bool ShowColorDetail
        {
            get { return (bool)GetValue(ShowColorDetailProperty); }
            set { SetValue(ShowColorDetailProperty, value); }
        }
        #endregion ShowColorDetail

        #region ShowColorDetailText // 定义 ShowColorDetailText 依赖属性
        public static readonly DependencyProperty ShowColorDetailTextProperty =
        DependencyProperty.Register("ShowColorDetailText", typeof(string), typeof(CustomizedColorPickerUserControl), new PropertyMetadata(null));
        [Description("Gets or sets the CustomizedColorPicker ShowColorDetailText")]
        [Category("GcjControl")]
        public string ShowColorDetailText
        {
            get { return (string)GetValue(ShowColorDetailTextProperty); }
            set { SetValue(ShowColorDetailTextProperty, value); }
        }
        #endregion ShowColorDetailText

        #region SelectedColor // 定义 SelectedColor 依赖属性
        public static readonly DependencyProperty SelectedColorProperty =
        DependencyProperty.Register("SelectedColor", typeof(Color), typeof(CustomizedColorPickerUserControl), new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedColorPicker Selected Color")]
        [Category("GcjControl")]
        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set
            {
                SetValue(SelectedColorProperty, value);
                //_clrPicker.SelectedColor = value;
            }
        }
        #endregion SelectedColor

        #region SelectedColorChangedEvent
        // 注册一个路由事件
        public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectedColorChanged", // 事件名称
            RoutingStrategy.Bubble, // 路由策略：冒泡、隧道、直接
            typeof(RoutedPropertyChangedEventHandler<Color?>), // 事件处理程序的类型
            typeof(CustomizedColorPickerUserControl)); // 注册事件的类

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("CustomizedColorPicker SelectedColorChanged Event Handler")]
        [Category("GcjControl")]
        public event RoutedPropertyChangedEventHandler<Color?> SelectedColorChanged
        {
            add { AddHandler(SelectedColorChangedEvent, value); }
            remove { RemoveHandler(SelectedColorChangedEvent, value); }
        }

        // 触发路由事件的方式
        protected void RaiseSelectedColorChangedEvent(System.Windows.Media.Color? newvalue)
        {
            RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> newEventArgs = new RoutedPropertyChangedEventArgs<System.Windows.Media.Color?>(_clrPicker.SelectedColor, newvalue, SelectedColorChangedEvent);
            RaiseEvent(newEventArgs);
        }

        protected void RaiseSelectedColorChangedEvent(RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> newEventArgs = new RoutedPropertyChangedEventArgs<System.Windows.Media.Color?>(e.OldValue, e.NewValue, SelectedColorChangedEvent);
            RaiseEvent(newEventArgs);
        }

        private void _clrPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}:\r\n        private void _clrPicker_SelectedColorChanged Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");
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

            RaiseSelectedColorChangedEvent(e);
        }
        #endregion SelectedColorChangedEvent

        #region PreviewMouseLeftButtonDownEvent
        // 注册一个路由事件
        public static readonly RoutedEvent PreviewMouseLeftButtonDownEvent = EventManager.RegisterRoutedEvent(
            "PreviewMouseLeftButtonDown", // 事件名称
            RoutingStrategy.Bubble, // 路由策略：冒泡、隧道、直接
            typeof(MouseButtonEventHandler), // 事件处理程序的类型
            typeof(CustomizedColorPickerUserControl)); // 注册事件的类

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("CustomizedColorPicker PreviewMouseLeftButtonDownEvent Event Handler")]
        [Category("GcjControl")]
        public event MouseButtonEventHandler PreviewMouseLeftButtonDown
        {
            add { AddHandler(PreviewMouseLeftButtonDownEvent, value); }
            remove { RemoveHandler(PreviewMouseLeftButtonDownEvent, value); }
        }

        // 触发路由事件的方式
        protected void RaisePreviewMouseLeftButtonDownEvent(System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Input.MouseButtonEventArgs newMouseEven = new System.Windows.Input.MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left)
            {
                RoutedEvent = PreviewMouseLeftButtonDownEvent
            };
            RaiseEvent(newMouseEven);
        }

        private void _clrPicker_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                Debug.WriteLine("$$$$ OnDebugPreviewMouseLeftButtonDown(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
                DepUtil.DumpObjectStructure(_clrPicker, 0, (int)DUMP_LEVEL.DUMP_ALIGNMENT_SIZE_POSITION_DUMP_MARGIN);
            }
            RaisePreviewMouseLeftButtonDownEvent(e);
        }

        public BindingExpression? ColorBindingExpression()
        {
            return GetBindingExpression(CustomizedColorPickerUserControl.SelectedColorProperty);
        }

        #endregion PreviewMouseLeftButtonDownEvent

        #region ShowColorAndNameStreched // 定义 ShowColorAndNameStreched 依赖属性
        public static readonly DependencyProperty ShowColorAndNameStrechedProperty =
        DependencyProperty.Register("ShowColorAndNameStreched", typeof(bool), typeof(CustomizedColorPickerUserControl), new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedColorPicker ShowColorAndNameStreched, Change Default ShowColorAndNames Option Behavioral ")]
        [Category("GcjControl")]
        public bool ShowColorAndNameStreched
        {
            get { return (bool)GetValue(ShowColorAndNameStrechedProperty); }
            set { SetValue(ShowColorAndNameStrechedProperty, value); }
        }
        #endregion ShowColorAndNameStreched

        #endregion Properties
    }
}
