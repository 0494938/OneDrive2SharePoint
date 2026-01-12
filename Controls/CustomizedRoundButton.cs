using System.ComponentModel;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedRoundButton : System.Windows.Controls.Button
    {
        CustomizedRoundImage? img = null;
        public CustomizedRoundButton() : base()
        {
            //DefaultStyleKey = typeof(CustomizedRoundButton);
            Loaded += CustomizedRoundButton_Loaded;
            Drop += CustomizedRoundButton_Drop;
            DragEnter += CustomizedRoundButton_DragEnter;
            DragOver += CustomizedRoundButton_DragOver;
        }

        private void CustomizedRoundButton_Loaded(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("$$$$ CustomizedRoundButton_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
        }

        #region Properties

        #region GcjVer  // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedRoundButton),
            new PropertyMetadata("Gcj Image Button v1.0"));


        [Description("Gets or sets the CustomizedRoundButton Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region CornerRadius // 定义 ImageSource 依赖属性
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CustomizedRoundButton), new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedRoundButton CornerRadius")]
        [Category("GcjControl")]
        public CornerRadius CornerRadius
        {
            get { 
                object? value = GetValue(CornerRadiusProperty);
                if (value == null || ((System.Windows.CornerRadius)value).TopLeft == 0)
                    return new CornerRadius(5);
                return (CornerRadius)value;
            }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion CornerRadius

        #endregion Properties

        #region EventHandlers
        #endregion EventHandlers

        #region DropFileSupport
        #region DropFilePath         // 定义 DropFilePath 依赖属性
        public static readonly DependencyProperty _DropFilePath = DependencyProperty.Register(
            "DropFilePath",
            typeof(string),
            typeof(CustomizedRoundButton),
            new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedRoundButton Drop File Path")]
        [Category("GcjControl")]
        public string? DropFilePath
        {
            get { return (string?)GetValue(_DropFilePath); }
            set { SetValue(_DropFilePath, value); }
        }
        #endregion DropFilePath


        #region OnFileDropped Event
        private void CustomizedRoundButton_DragOver(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void CustomizedRoundButton_DragEnter(object sender, DragEventArgs e)
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

        private void CustomizedRoundButton_Drop(object sender, DragEventArgs e)
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
            typeof(CustomizedRoundButton)); // 注册事件的类

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
