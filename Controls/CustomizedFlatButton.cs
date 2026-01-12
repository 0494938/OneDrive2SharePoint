using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace GcjUiCtrl.Control
{

    public partial class CustomizedFlatButton : System.Windows.Controls.Button
    {
        CustomizedRoundImage? img = null;
        public CustomizedFlatButton() : base()
        {
            //DefaultStyleKey = typeof(CustomizedFlatButton);
            Loaded += CustomizedFlatButton_Loaded;
            Click += CustomizedFlatButton_Click;
            Drop += CustomizedFlatButton_Drop;
            DragEnter += CustomizedFlatButton_DragEnter;
            DragOver += CustomizedFlatButton_DragOver;
            PreviewMouseLeftButtonDown += CustomizedFlatButton_PreviewMouseLeftButtonDown;
        }

        private void CustomizedFlatButton_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Debug.WriteLine("CustomizedFlatButton_PreviewMouseLeftButtonDown Event Triggered");
        }

        static readonly string[] randUrls = new string[] {
            "https://www.tel.co.jp/news/topics/2024/20240930_001.html",
            "https://www.tel.co.jp/rd/base/index.html",
            "https://www.tel.co.jp/product/all/index.html",
            "https://www.tel.co.jp/product/service/index.html",
            "https://www.tel.co.jp/ir/personal/index.html",
            "https://www.tel.co.jp/corporatesummary/index.html",
        };

        static Random random = new Random();
        private void CustomizedFlatButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("CustomizedFlatButton_Click Clicked");
            if (EnableDefaultClickAction) {
                int randomNumber = random.Next(0, randUrls.Length);
                Process.Start(new ProcessStartInfo
                {
                    FileName = randUrls[randomNumber],
                    UseShellExecute = true // 必须设置为 true，才能使用默认浏览器
                });
            }
        }

        private void CustomizedFlatButton_Loaded(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("$$$$ CustomizedFlatButton_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
        }

        #region Properties

        #region GcjVer  // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedFlatButton),
            new PropertyMetadata("Gcj Image Button v1.0"));


        [Description("Gets or sets the CustomizedFlatButton Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region EnableDefaultClickAction // 定义 ReleaseMode 依赖属性
        public static readonly DependencyProperty __EnableDefaultClickAction = DependencyProperty.Register(
            "EnableDefaultClickAction",
            typeof(bool),
            typeof(CustomizedFlatButton),
            new PropertyMetadata(true));

        [Description("Gets or sets the CustomizedFlatButton Version")]
        [Category("GcjControl")]

        public bool EnableDefaultClickAction
        {
            get { return (bool)GetValue(__EnableDefaultClickAction); }
            set { SetValue(__EnableDefaultClickAction ,value); }
        }
        #endregion EnableDefaultClickAction

        #region CornerRadius // 定义 ImageSource 依赖属性
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CustomizedFlatButton), new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedFlatButton CornerRadius")]
        [Category("GcjControl")]
        public CornerRadius CornerRadius
        {
            get
            {
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
            typeof(CustomizedFlatButton),
            new PropertyMetadata(null));

        [Description("Gets or sets the CustomizedFlatButton Drop File Path")]
        [Category("GcjControl")]
        public string? DropFilePath
        {
            get { return (string?)GetValue(_DropFilePath); }
            set { SetValue(_DropFilePath, value); }
        }
        #endregion DropFilePath

        #region OnFileDropped Event
        private void CustomizedFlatButton_DragOver(object sender, DragEventArgs e)
        {
            if (this.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void CustomizedFlatButton_DragEnter(object sender, DragEventArgs e)
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

        private void CustomizedFlatButton_Drop(object sender, DragEventArgs e)
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
            typeof(CustomizedFlatButton)); // 注册事件的类

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // CLR 包装器，允许通过 += 和 -= 订阅和解除订阅事件
        [Description("Customized CustomizedFlatButton Drop File Event Handler")]
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
