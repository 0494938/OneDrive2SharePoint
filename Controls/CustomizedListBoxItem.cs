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

    public partial class CustomizedListBoxItem : System.Windows.Controls.ListBoxItem
    {
        #region GcjVer // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedListBoxItem),
            new PropertyMetadata("Gcj Image Button v1.0"));


        [Description("Gets or sets the CustomizedListBoxItem Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer // 定义 GcjVer 依赖属性

        public CustomizedListBoxItem() : base()
        {
            Loaded += CustomizedListBoxItem_Loaded;
            SizeChanged += CustomizedListBoxItem_SizeChanged;
            Drop += CustomizedListBoxItem_Drop;
            DragEnter += CustomizedListBoxItem_DragEnter;
            DragOver += CustomizedListBoxItem_DragOver;
        }

        private void CustomizedListBoxItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        private void CustomizedListBoxItem_LayoutUpdated(object? sender, EventArgs e)
        {
        }

        private void CustomizedListBoxItem_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("$$$$ CustomizedListBoxItem_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            Debug.WriteLine("  Width=" + Width + ", Height=" + Height + ", ActualWidth=" + ActualWidth + ", ActualHeight=" + ActualHeight);
        }

        #region DropFileSupport
        #region DropFilePath         // 定义 DropFilePath 依赖属性
        public static readonly DependencyProperty _DropFilePath = DependencyProperty.Register(
            "DropFilePath",
            typeof(string),
            typeof(CustomizedListBoxItem),
            new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedListBoxItem Drop File Path")]
        [Category("GcjControl")]
        public string? DropFilePath
        {
            get { return (string?)GetValue(_DropFilePath); }
            set { SetValue(_DropFilePath, value); }
        }
        #endregion DropFilePath


        #region OnFileDropped Event
        private void CustomizedListBoxItem_DragOver(object sender, DragEventArgs e)
        {
            if (AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void CustomizedListBoxItem_DragEnter(object sender, DragEventArgs e)
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

        private void CustomizedListBoxItem_Drop(object sender, DragEventArgs e)
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
            typeof(CustomizedListBoxItem)); // 注册事件的类

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
