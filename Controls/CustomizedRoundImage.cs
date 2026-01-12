using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using Point = System.Windows.Point;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedRoundImage : System.Windows.Controls.Image
    {
        #region GcjVer // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedRoundImage),
            new PropertyMetadata("Gcj Image Button v1.0"));


        [Description("Gets or sets the CustomizedRoundImage Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer // 定义 GcjVer 依赖属性

        public CustomizedRoundImage() : base()
        {
            Loaded += CustomizedRoundImage_Loaded;
            SizeChanged += CustomizedRoundImage_SizeChanged;
            Drop += CustomizedRoundImage_Drop;
            DragEnter += CustomizedRoundImage_DragEnter;
            DragOver += CustomizedRoundImage_DragOver;
        }

        private void CustomizedRoundImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine("$$$$ CustomizedRoundImage_LayoutUpdated::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.ToString() + ") $$$$");
            Debug.WriteLine("  Width=" + Width + ", Height=" + Height + ", ActualWidth=" + ActualWidth + ", ActualHeight=" + ActualHeight);

            double radiusX = ActualWidth == double.NaN ? Width / 2 : ActualWidth / 2;
            double radiusY = ActualHeight == double.NaN ? Height / 2 : ActualHeight / 2;
            Point center = new System.Windows.Point(radiusX, radiusY);
            Clip = new EllipseGeometry(center, radiusX, radiusY);
        }

        private void CustomizedRoundImage_LayoutUpdated(object? sender, EventArgs e)
        {
        }

        private void CustomizedRoundImage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("$$$$ CustomizedRoundImage_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            Debug.WriteLine("  Width=" + Width + ", Height=" + Height + ", ActualWidth=" + ActualWidth + ", ActualHeight=" + ActualHeight);
            if (StartAnimation == true)
            {
                StartImageRotation();
            }
        }

        Storyboard? storyboard = null;

        void StartImageRotation()
        {
            storyboard = (Storyboard)FindResource("RotateImageStoryboard5s");
            RotateTransform? curTransform = RenderTransform as RotateTransform;
            if (curTransform == null)
            {
                curTransform = new RotateTransform();
            }
            curTransform.CenterX = ActualWidth / 2;
            curTransform.CenterY = ActualHeight / 2;
            storyboard?.Begin(this, true); // 开始动画
        }

        #region StartAnimation // 定义 StartAnimation 依赖属性
        public static readonly DependencyProperty StartAnimationProperty =
            DependencyProperty.Register("StartAnimation", typeof(bool), typeof(CustomizedRoundImage), new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedRoundImage StartAnimation")]
        [Category("GcjControl")]
        public bool StartAnimation
        {
            get
            {
                return (bool)GetValue(StartAnimationProperty);
            }
            set
            {
                //if (value == StartAnimation)
                //{
                //    //do nothing
                //}
                //else
                {
                    if (value == true)
                    {
                        StartImageRotation();
                    }
                    else
                    {
                        storyboard?.Stop(this);
                    }
                    SetValue(StartAnimationProperty, value);
                }
            }
        }
        #endregion StartAnimation

        #region DropFileSupport
        #region DropFilePath         // 定义 DropFilePath 依赖属性
        public static readonly DependencyProperty _DropFilePath = DependencyProperty.Register(
            "DropFilePath",
            typeof(string),
            typeof(CustomizedRoundImage),
            new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedRoundImage Drop File Path")]
        [Category("GcjControl")]
        public string? DropFilePath
        {
            get { return (string?)GetValue(_DropFilePath); }
            set { SetValue(_DropFilePath, value); }
        }
        #endregion DropFilePath


        #region OnFileDropped Event
        private void CustomizedRoundImage_DragOver(object sender, DragEventArgs e)
        {
            if (AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void CustomizedRoundImage_DragEnter(object sender, DragEventArgs e)
        {
            if (AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
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

        private void CustomizedRoundImage_Drop(object sender, DragEventArgs e)
        {
            if (AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
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
            typeof(CustomizedRoundImage)); // 注册事件的类

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("Customized Round Image Control Drop File Event Handler")]
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
