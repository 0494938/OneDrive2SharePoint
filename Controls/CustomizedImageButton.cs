using GcjUtil;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedImageButton : System.Windows.Controls.Button
    {
        CustomizedRoundImage? img = null;
        public CustomizedImageButton() : base()
        {
            //DefaultStyleKey = typeof(CustomizedImageButton);
            Loaded += CustomizedImageButton_Loaded;
            DragEnter += CustomizedImageButton_DragEnter;
            DragOver += CustomizedImageButton_DragOver;
            Drop += CustomizedImageButton_Drop;
        }

        private void CustomizedImageButton_Loaded(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("$$$$ CustomizedImageButton_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            //Debug.WriteLine("  Width=" + this.Width + ", Height=" + this.Height + ", ActualWidth=" + this.ActualWidth + ", ActualHeight=" + this.ActualHeight);
            img = DepUtil.GetFirstChildrenOfType<CustomizedRoundImage>(this);
        }

        #region Properties

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedImageButton),
            new PropertyMetadata("Gcj Image Button v1.0"));


        [Description("Gets or sets the CustomizedImageButton Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region Text // 定义 Text 依赖属性
        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(CustomizedImageButton), new PropertyMetadata("Text To Set"));

        [Description("Gets or sets the CustomizedImageButton Text")]
        [Category("GcjControl")]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion Text

        #region ImageSource         // 定义 ImageSource 依赖属性
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(CustomizedImageButton), new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedImageButton ImageSource")]
        [Category("GcjControl")]
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        #endregion ImageSource

        #region CornerRadius // 定义 ImageSource 依赖属性
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CustomizedImageButton), new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedImageButton CornerRadius")]
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

        #region TextWrapping // 定义 TextWrapping 依赖属性
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(System.Windows.TextWrapping), typeof(CustomizedImageButton), new PropertyMetadata(TextWrapping.NoWrap));

        [Description("Gets or sets the CustomizedImageButton TextWrapping")]
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

        #region StartIconAnimation // 定义 StartAnimation 依赖属性
        public static readonly DependencyProperty StartIconAnimationProperty =
            DependencyProperty.Register("StartIconAnimation", typeof(bool), typeof(CustomizedImageButton), new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedImageButton StartIconAnimation")]
        [Category("GcjControl")]
        public bool StartIconAnimation
        {
            get
            {
                if (img != null)
                {
                    return img.StartAnimation;
                }
                return (bool)GetValue(StartIconAnimationProperty);
            }
            set
            {
                SetValue(StartIconAnimationProperty, value);
                if (img != null)
                {
                    img.StartAnimation = value;
                }
            }
        }
        #endregion StartAnimation

        #region ImageMargin
        public static readonly DependencyProperty ImageMarginProperty = DependencyProperty.Register(
            "ImageMargin",
            typeof(Thickness),
            typeof(CustomizedImageButton),
            new PropertyMetadata(new Thickness(-2, -2, -2, -2)));

        [Description("Gets or sets the Customized Title Control Icon Image Margin")]
        [Category("GcjControl")]
        public Thickness ImageMargin
        {
            get { return (Thickness)GetValue(ImageMarginProperty); }
            set { SetValue(ImageMarginProperty, value); }
        }
        #endregion ImageMargin   

        #region DropFilePath         // 定义 DropFilePath 依赖属性
        public static readonly DependencyProperty _DropFilePath = DependencyProperty.Register(
            "DropFilePath",
            typeof(string),
            typeof(CustomizedImageButton),
            new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedImageButton Drop File Path")]
        [Category("GcjControl")]
        public string? DropFilePath
        {
            get { return (string?)GetValue(_DropFilePath); }
            set { SetValue(_DropFilePath, value); }
        }
        #endregion DropFilePath

        #endregion Properties


        #region EventHandlers

        #region OnFileDropped Event
        private void CustomizedImageButton_DragOver(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void CustomizedImageButton_DragEnter(object sender, DragEventArgs e)
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

        private void CustomizedImageButton_Drop(object sender, DragEventArgs e)
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
            typeof(CustomizedImageButton)); // 注册事件的类

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("Customized Button Control Drop File Event Handler")]
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

        #endregion EventHandlers
    }


}
