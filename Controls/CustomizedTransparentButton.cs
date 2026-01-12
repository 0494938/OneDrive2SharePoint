using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedTransparentButton : System.Windows.Controls.Button
    {
        public CustomizedTransparentButton() : base()
        {
            //DefaultStyleKey = typeof(CustomizedITransparentButton);
            Loaded += CustomizedITransparentButton_Loaded;
            Drop += CustomizedTransparentButton_Drop;
            DragEnter += CustomizedTransparentButton_DragEnter;
            DragOver += CustomizedTransparentButton_DragOver;
        }

        private void CustomizedITransparentButton_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("$$$$ CustomizedITransparentButton_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
        }

        #region Properties

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedTransparentButton),
            new PropertyMetadata("Gcj Image Button v1.0"));


        [Description("Gets or sets the CustomizedITransparentButton Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region TextWrapping // 定义 TextWrapping 依赖属性
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(System.Windows.TextWrapping), typeof(CustomizedTransparentButton), new PropertyMetadata(TextWrapping.NoWrap));

        [Description("Gets or sets the CustomizedITransparentButton TextWrapping")]
        [Category("GcjControl")]
        public System.Windows.TextWrapping TextWrapping
        {
            get
            {
                return (System.Windows.TextWrapping)GetValue(TextWrappingProperty);
            }
            set { SetValue(TextWrappingProperty, value); }
        }
        #endregion TextWrapping

        #endregion Properties


        #region EventHandlers

        #endregion EventHandlers

        #region DropFileSupport

        #region DropFilePath         // 定义 DropFilePath 依赖属性
        public static readonly DependencyProperty _DropFilePath = DependencyProperty.Register(
            "DropFilePath",
            typeof(string),
            typeof(CustomizedTransparentButton),
            new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedTransparentButton Drop File Path")]
        [Category("GcjControl")]
        public string? DropFilePath
        {
            get { return (string?)GetValue(_DropFilePath); }
            set { SetValue(_DropFilePath, value); }
        }
        #endregion DropFilePath

        #region OnFileDropped Event
        private void CustomizedTransparentButton_DragOver(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void CustomizedTransparentButton_DragEnter(object sender, DragEventArgs e)
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

        private void CustomizedTransparentButton_Drop(object sender, DragEventArgs e)
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
            typeof(CustomizedTransparentButton)); // 注册事件的类

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("Customized CustomizedTransparentButton Drop File Event Handler")]
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
