using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using Point = System.Windows.Point;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedScrollControl : System.Windows.Controls.ScrollViewer
    {

        private Point _scrollStartPoint;
        private Point _scrollStartOffset;
        private bool _isDragging;

        public CustomizedScrollControl() : base()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomizedScrollControl),
            //   new FrameworkPropertyMetadata(typeof(CustomizedScrollControl)));

            Loaded += CustomizedScrollControl_Loaded;
            LostFocus += CustomizedScrollControl_LostFocus;
            GotFocus += CustomizedScrollControl_GotFocus;
            IsVisibleChanged += CustomizedScrollControl_IsVisibleChanged;
            IsEnabledChanged += CustomizedScrollControl_IsEnabledChanged;
            SizeChanged += CustomizedScrollControl_SizeChanged;

            // 隐藏滚动条
            this.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            this.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

            // 添加鼠标事件
            this.PreviewMouseLeftButtonDown += DragScrollViewer_PreviewMouseLeftButtonDown;
            this.PreviewMouseMove += DragScrollViewer_PreviewMouseMove;
            this.PreviewMouseLeftButtonUp += DragScrollViewer_PreviewMouseLeftButtonUp;
            this.PreviewMouseWheel += DragScrollViewer_PreviewMouseWheel;
        }

        #region EventHandler
        private void CustomizedScrollControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DisableHScroll)
            {
                var scrollViewer = sender as ScrollViewer;
                if (scrollViewer?.Content is FrameworkElement childElement)
                {
                    childElement.Width = scrollViewer.ActualWidth;
                }
            }
        }

        private void DragScrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ButtonBase || e.Source is System.Windows.Controls.Button 
                || e.Source is CustomizedRoundButton || e.Source is CustomizedLaunchImage || e.Source is CustomizedRoundCornerImage || e.Source is CustomizedFlatButton
                || e.Source is System.Windows.Controls.TextBox || e.Source is TextBoxBase
                )  //if you add other control which use MouseEvent, you should skip them. else that control will not receive event. 
                return;
            // 获取鼠标初始点击的位置
            _scrollStartPoint = e.GetPosition(this);
            _scrollStartOffset.X = this.HorizontalOffset;
            _scrollStartOffset.Y = this.VerticalOffset;

            // 启用捕获鼠标，开始拖动
            this.CaptureMouse();
            _isDragging = true;
        }

        private void DragScrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                // 计算当前鼠标位置与初始位置的偏移量
                Point currentPoint = e.GetPosition(this);
                Point delta = new Point(currentPoint.X - _scrollStartPoint.X, currentPoint.Y - _scrollStartPoint.Y);

                // 根据鼠标移动的偏移量滚动内容
                if (!DisableHScroll)
                    this.ScrollToHorizontalOffset(_scrollStartOffset.X - delta.X);
                this.ScrollToVerticalOffset(_scrollStartOffset.Y - delta.Y);
            }
        }

        private void DragScrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // 停止拖动，释放鼠标捕获
            this.ReleaseMouseCapture();
            _isDragging = false;
        }

        private void DragScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // 支持鼠标滚轮进行垂直滚动
            this.ScrollToVerticalOffset(this.VerticalOffset - e.Delta);
        }
        
        private void CustomizedScrollControl_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        // 失去焦点时启动滚动
        private void CustomizedScrollControl_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void CustomizedScrollControl_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("CustomizedScrollControl::CustomizedScrollControl_Loaded(" + this.Name + ")");
            if (DisableHScroll)
            {
                if (Content is FrameworkElement childElement)
                {
                    childElement.Width = this.ActualWidth;
                }
            }
        }

        private void CustomizedScrollControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("CustomizedScrollControl::CustomizedScrollControl_IsEnabledChanged(" + this.Name + ") " + e.OldValue + " => " + e.NewValue);
          
        }

        private void CustomizedScrollControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("CustomizedScrollControl::CustomizedScrollControl_IsVisibleChanged(" + this.Name + ") " + e.OldValue + " => " + e.NewValue);
        }
        #endregion EventHandler

        #region Properties

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedScrollControl),
            new PropertyMetadata("Gcj RoundTextBox v1.0"));


        [Description("Gets or sets the CustomizedScrollControl Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region DisableHScroll // 定义 DisableHScroll 依赖属性
        public static readonly DependencyProperty DisableHScrollProperty =
            DependencyProperty.Register("DisableHScroll", typeof(bool), typeof(CustomizedScrollControl), new PropertyMetadata(true));

        [Description("Gets or sets the CustomizedScrollControl DisableHScroll")]
        [Category("GcjControl")]
        public bool DisableHScroll
        {
            get
            {
                return (bool)GetValue(DisableHScrollProperty);
            }
            set
            {
                SetValue(DisableHScrollProperty, value);
            }
        }
        #endregion DisableHScroll

        #region DisableVScroll // 定义 DisableVScroll 依赖属性
        public static readonly DependencyProperty DisableVScrollProperty =
            DependencyProperty.Register("DisableVScroll", typeof(bool), typeof(CustomizedScrollControl), new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedScrollControl DisableVScroll")]
        [Category("GcjControl")]
        public bool DisableVScroll
        {
            get
            {
                return (bool)GetValue(DisableVScrollProperty);
            }
            set
            {
                SetValue(DisableVScrollProperty, value);
            }
        }
        #endregion DisableVScroll

        #endregion Properties
    }
}
