using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedRoundCornerImage : System.Windows.Controls.Image
    {
        #region GcjVer // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedRoundCornerImage),
            new PropertyMetadata("Gcj Image Button v1.0"));


        [Description("Gets or sets the CustomizedRoundCornerImage Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer // 定义 GcjVer 依赖属性
        public static void RegisterSourceChangeListener(System.Windows.Controls.Image image, Action callback)
        {
            DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(
                System.Windows.Controls.Image.SourceProperty, typeof(System.Windows.Controls.Image));

            descriptor.AddValueChanged(image, (sender, args) =>
            {
                callback?.Invoke();
            });
        }

        #region CornerRadius // 定义 CornerRadius 依赖属性
        //public static readonly DependencyProperty CornerRadiusProperty =
        //    DependencyProperty.Register("CornerRadius", typeof(double), typeof(CustomizedRoundCornerImage), new PropertyMetadata(null));
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CustomizedRoundCornerImage),
                new FrameworkPropertyMetadata(new CornerRadius(0.0), FrameworkPropertyMetadataOptions.AffectsRender, OnCornerRadiusChanged));

        [Description("Gets or sets the CustomizedRoundCornerImage CornerRadius")]
        [Category("GcjControl")]
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion CornerRadius

        public CustomizedRoundCornerImage() : base()
        {
            Loaded += CustomizedRoundCornerImage_Loaded;
            Drop += CustomizedRoundCornerImage_Drop;
            DragEnter += CustomizedRoundCornerImage_DragEnter;
            DragOver += CustomizedRoundCornerImage_DragOver;
            RegisterSourceChangeListener(this, () =>
            {
                UpdateClip();
            });
        }

        private void CustomizedRoundCornerImage_LayoutUpdated(object? sender, EventArgs e)
        {
        }

        private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomizedRoundCornerImage roundedImage)
            {
                roundedImage.UpdateClip();
            }
        }

        private void CustomizedRoundCornerImage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("$$$$ CustomizedRoundCornerImage_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            Debug.WriteLine("  Width=" + Width + ", Height=" + Height + ", ActualWidth=" + ActualWidth + ", ActualHeight=" + ActualHeight);

            UpdateClip();
        }

        private void UpdateClip()
        {
            CornerRadius radius = CornerRadius;
            this.Clip = new RectangleGeometry
            {
                RadiusX = radius.TopLeft,
                RadiusY = radius.BottomRight,
                Rect = new Rect(0, 0, this.ActualWidth, this.ActualHeight)
            };
        }

        #region DropFileSupport
        #region DropFilePath         // 定义 DropFilePath 依赖属性
        public static readonly DependencyProperty _DropFilePath = DependencyProperty.Register(
            "DropFilePath",
            typeof(string),
            typeof(CustomizedRoundCornerImage),
            new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedRoundCornerImage Drop File Path")]
        [Category("GcjControl")]
        public string? DropFilePath
        {
            get { return (string?)GetValue(_DropFilePath); }
            set { SetValue(_DropFilePath, value); }
        }
        #endregion DropFilePath


        #region OnFileDropped Event
        private void CustomizedRoundCornerImage_DragOver(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void CustomizedRoundCornerImage_DragEnter(object sender, DragEventArgs e)
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

        private void CustomizedRoundCornerImage_Drop(object sender, DragEventArgs e)
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
            typeof(CustomizedRoundCornerImage)); // 注册事件的类

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
