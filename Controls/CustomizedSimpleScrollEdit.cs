using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedSimpleScrollEdit : System.Windows.Controls.TextBox
    {
        public CustomizedSimpleScrollEdit() : base()
        {
            //DefaultStyleKey = typeof(CustomizedSimpleScrollEdit);
            Loaded += CustomizedSimpleScrollEdit_Loaded;
            LostFocus += CustomizedSimpleScrollEdit_LostFocus;
            GotFocus += CustomizedSimpleScrollEdit_GotFocus;
            IsVisibleChanged += CustomizedSimpleScrollEdit_IsVisibleChanged;
            IsEnabledChanged += CustomizedSimpleScrollEdit_IsEnabledChanged;

            // 初始化定时器
            _scrollTimer = new DispatcherTimer();
            _scrollTimer.Interval = TimeSpan.FromMilliseconds(100); // 滚动间隔时间
            _scrollTimer.Tick += ScrollTimer_Tick;
        }

        private DispatcherTimer _scrollTimer;
        private double _currentHorizontalOffset = 0;

        private void CustomizedSimpleScrollEdit_GotFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("CustomizedSimpleScrollEdit::CustomizedSimpleScrollEdit_GotFocus(" + this.Name + ")");
            if (ScrollAnimation == true)
                _scrollTimer.Stop();
        }

        // 失去焦点时启动滚动
        private void CustomizedSimpleScrollEdit_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("CustomizedSimpleScrollEdit::CustomizedSimpleScrollEdit_LostFocus(" + this.Name + ")");
            if (ScrollAnimation == true)
                _scrollTimer.Start();
        }

        private void CustomizedSimpleScrollEdit_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("CustomizedSimpleScrollEdit::CustomizedSimpleScrollEdit_Loaded(" + this.Name + ")");
            if (string.IsNullOrEmpty(Text))
            {
                Text = "This is line 1 with a long content to test horizontal scrolling.\r\n"
+ "This is line 2 with a long content to test horizontal scrolling.\r\n"
+ "This is line 3 with a long content to test horizontal scrolling.\r\n"
+ "This is line 4 with a long content to test horizontal scrolling.\r\n"
+ "This is line 5 with a long content to test horizontal scrolling.\r\n"
+ "This is line 6 with a long content to test horizontal scrolling.\r\n"
+ "This is line 7 with a long content to test horizontal scrolling.\r\n"
+ "This is line 8 with a long content to test horizontal scrolling.\r\n"
+ "This is line 9 with a long content to test horizontal scrolling.\r\\n"
+ "This is line 10 with a long content to test horizontal scrolling.\r\\n";
            }
            //Debug.WriteLine("$$$$ CustomizedSimpleScrollEdit_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            //Debug.WriteLine("  Width=" + this.Width + ", Height=" + this.Height + ", ActualWidth=" + this.ActualWidth + ", ActualHeight=" + this.ActualHeight);

            if (ScrollAnimation == true && IsVisible==true && IsFocused !=true)
            {
                _scrollTimer.Start();
            }
        }

        private void CustomizedSimpleScrollEdit_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("CustomizedSimpleScrollEdit::CustomizedSimpleScrollEdit_IsEnabledChanged(" + this.Name + ") " + e.OldValue + " => " + e.NewValue);
          
        }

        private void CustomizedSimpleScrollEdit_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("CustomizedSimpleScrollEdit::CustomizedSimpleScrollEdit_IsVisibleChanged(" + this.Name + ") " + e.OldValue + " => " + e.NewValue);
        }

        #region Properties

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedSimpleScrollEdit),
            new PropertyMetadata("Gcj TextBox v1.0"));


        [Description("Gets or sets the CustomizedSimpleScrollEdit Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region ScrollAnimation         // 定义 ScrollAnimation 依赖属性
        public static readonly DependencyProperty _ScrollAnimation = DependencyProperty.Register(
            "ScrollAnimation",
            typeof(bool),
            typeof(CustomizedSimpleScrollEdit),
            new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedSimpleScrollEdit Version")]
        [Category("GcjControl")]
        public bool ScrollAnimation
        {
            get { return (bool)GetValue(_ScrollAnimation); }
            set { SetValue(_ScrollAnimation, value); }
        }
        #endregion ScrollAnimation

        #endregion Properties

        // 滚动事件
        private void ScrollTimer_Tick(object? sender, EventArgs e)
        {
            var scrollViewer = GetScrollViewer(this);

            if (scrollViewer != null)
            {
                // 计算新的横向滚动位置
                _currentHorizontalOffset = scrollViewer.HorizontalOffset + 1;

                if (_currentHorizontalOffset >= scrollViewer.ScrollableWidth)
                {
                    _currentHorizontalOffset = 0; // 如果到达右侧，重置到左侧
                }

                // 滚动到新的横向偏移量
                scrollViewer.ScrollToHorizontalOffset(_currentHorizontalOffset);
            }
        }

        // 获取TextBox的ScrollViewer
        private ScrollViewer GetScrollViewer(DependencyObject obj)
        {
            if (obj is ScrollViewer)
                return (ScrollViewer)obj;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                var scrollViewer = GetScrollViewer(child);
                if (scrollViewer != null)
                    return scrollViewer;
            }

            return null;
        }
    }
}
