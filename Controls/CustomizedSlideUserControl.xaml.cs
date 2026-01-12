using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using UserControl = System.Windows.Controls.UserControl;

namespace GcjUiCtrl.Control
{
    public enum SlideDirection
    {
        RightToLeft = 0,
        LeftToRight = 1,
        TopToBottom = 2,
        BottomToTop = 3
    }

    public partial class CustomizedSlideUserControl : UserControl
    {
        private int _currentIndex = 0;
        private DispatcherTimer? _timer = null;

        public CustomizedSlideUserControl()
        {
            InitializeComponent();
            SetupImageSources();
            SetupTimer();
        }

        private void SetupImageSources()
        {
            if (IsInDesignMode())
            {
                InitSlideControl();
            }
            else
                Loaded += CustomizedSlideUserControl_Loaded;
        }
        private string LeadDir = "";//"Controls/";
        private BitmapImage LoadImageFromResources(string imageName)
        {
            // 创建 BitmapImage 对象
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri($"pack://application:,,,/{LeadDir}{imageName}");
            //bitmap.UriSource = new Uri($"/{LeadDir}{imageName}");
            bitmap.EndInit();

            // 设置 Image 控件的 Source
            //MyImage.Source = bitmap;
            return bitmap;
        }

        void InitSlideControl()
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}:CustomizedSlideUserControl::InitSlideControl Triggered");

            if (IsInDesignMode())
            {
                ImageSourceList.Add(ImgBackGround.Source);
                ImageSourceList.Add(ImgForeGround.Source);

                _currentIndex = new Random(RuntimeHelpers.GetHashCode(this) + DateTime.UtcNow.Millisecond).Next(0, ImageSourceList.Count);
                ImgBackGround.Source = ImageSourceList[_currentIndex];
            }
            else
            {
                if (ImageSourceList.Count <= 0)
                {
                    ImageSourceList.Add(ImgBackGround.Source);
                    ImageSourceList.Add(ImgForeGround.Source);
                }
                if (ImageSourceList.Count > 0)
                {
                    // 初始化第一张图片
                    _currentIndex = new Random(RuntimeHelpers.GetHashCode(this) + DateTime.UtcNow.Millisecond).Next(0, ImageSourceList.Count);
                    ImgBackGround.Source = ImageSourceList[_currentIndex];
                }
            }
        }

        private void CustomizedSlideUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitSlideControl();
        }

        private void SetupTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(3); // 每5秒切换一次
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void StartTimer()
        {
            _timer?.Start();
        }

        private void StopTimer()
        {
            _timer?.Stop();
        }

        private bool IsInDesignMode()
        {
            return DesignerProperties.GetIsInDesignMode(this);
        }

        private SlideDirection _currentDirection = SlideDirection.RightToLeft;

        private int GetNextImageIdx(int _currentIndex)
        {
            int __orgIndex = _currentIndex;
            //_currentIndex = (_currentIndex + 1) % _imageSources.Count;
            int currentIndex = (_currentIndex + new Random(RuntimeHelpers.GetHashCode(this) + DateTime.UtcNow.Millisecond).Next(0, ImageSourceList.Count)) % ImageSourceList.Count;
            int nLoop = 0;
            while (currentIndex == __orgIndex && nLoop < ImageSourceList.Count)
            {
                nLoop++;
                currentIndex = (currentIndex + new Random(RuntimeHelpers.GetHashCode(this) + DateTime.UtcNow.Millisecond + nLoop).Next(0, ImageSourceList.Count)) % ImageSourceList.Count;
            }
            if (currentIndex == __orgIndex)
                currentIndex = (_currentIndex + 1) % ImageSourceList.Count;
            return currentIndex;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _currentIndex = GetNextImageIdx(_currentIndex);
            ImgForeGround.Source = ImageSourceList[_currentIndex];

            // 根据当前方向选择动画
            string direction = _currentDirection.ToString();
            var storyboard = (Storyboard)FindResource($"Slide{direction}");
            storyboard.Completed += Storyboard_Completed;
            storyboard.Begin();

            // 更新方向（可根据需要改变方向）
            _currentDirection = GetNextDirection();

            // 交换 ImgBackGround 和 ImgForeGround
            var temp = ImgBackGround;
            ImgBackGround = ImgForeGround;
            ImgForeGround = temp;
        }

        private SlideDirection GetNextDirection()
        {
            // 实现您的逻辑来确定下一个方向
            // 例如，循环使用不同的方向
            //return SlideDirection.RightToLeft; // 示例;
            //return (SlideDirection) ((DateTime.Now.Millisecond % 7 )% 4 ); // 示例
            if (SlideDirection == null)
                return (SlideDirection)(new Random(RuntimeHelpers.GetHashCode(this) + DateTime.UtcNow.Millisecond).Next(0, 4));
            else
                return SlideDirection;

        }

        private void Storyboard_Completed(object? sender, EventArgs e)
        {
            // 动画完成后，重置 ImgForeGround 的位置和透明度
            ImgForeGround.RenderTransform = new TranslateTransform();
            ImgForeGround.Opacity = 1;
        }

        #region Properties

        #region StartAnimation // 定义 StartAnimation 依赖属性
        public static readonly DependencyProperty StartAnimationProperty =
            DependencyProperty.Register("StartAnimation", typeof(bool), typeof(CustomizedSlideUserControl), new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedSlideUserControl StartAnimation")]
        [Category("GcjControl")]
        public bool StartAnimation
        {
            get
            {
                return (bool)GetValue(StartAnimationProperty);
            }
            set
            {
                if (value == true)
                {
                    StartTimer();
                }
                else
                {
                    StopTimer();
                }
                SetValue(StartAnimationProperty, value);
            }
        }
        #endregion StartAnimation


        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedSlideUserControl),
            new PropertyMetadata("Gcj Date Picker v1.0"));


        [Description("Gets or sets the CustomizedSlideUserControl Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        //private List<ImageSource> _imageSources = new List<ImageSource>();
        #region ImageSourceList         // 定义 ImageSourceList 依赖属性
        public static readonly DependencyProperty _ImageSourceList = DependencyProperty.Register(
            "ImageSourceList",
            typeof(List<ImageSource>),
            typeof(CustomizedSlideUserControl),
            new PropertyMetadata(new List<ImageSource>()));


        [Description("Gets or sets the CustomizedSlideUserControl ImageSourceList")]
        [Category("GcjControl")]
        public List<ImageSource> ImageSourceList
        {
            get { return (List<ImageSource>)GetValue(_ImageSourceList); }
            set { SetValue(_ImageSourceList, value); }
        }
        #endregion ImageSourceList

        #region ImageList         // 定义 ImageList 依赖属性
        public static readonly DependencyProperty _ImageList = DependencyProperty.Register(
            "ImageList",
            typeof(List<System.Windows.Controls.Image>),
            typeof(CustomizedSlideUserControl),
            new PropertyMetadata(new List<System.Windows.Controls.Image>()));


        [Description("Gets or sets the CustomizedSlideUserControl ImageList")]
        [Category("GcjControl")]
        public List<System.Windows.Controls.Image> ImageList
        {
            get { return (List<System.Windows.Controls.Image>)GetValue(_ImageList); }
            set { SetValue(_ImageList, value); }
        }
        #endregion ImageList

        #region ActiveIndex         // 定义 ActiveIndex 依赖属性
        public static readonly DependencyProperty _ActiveIndex = DependencyProperty.Register(
            "ActiveIndex",
            typeof(int),
            typeof(CustomizedSlideUserControl),
            new PropertyMetadata(0));


        [Description("Gets or sets the CustomizedSlideUserControl ActiveIndex")]
        [Category("GcjControl")]
        public int ActiveIndex
        {
            get { return _currentIndex; }

        }
        #endregion ActiveIndex

        #region SlideDirection         // 定义 SlideDirection 依赖属性
        public static readonly DependencyProperty _SlideDirection = DependencyProperty.Register(
            "SlideDirection",
            typeof(SlideDirection),
            typeof(CustomizedSlideUserControl),
            new PropertyMetadata(null));


        [Description("Gets or sets the CustomizedSlideUserControl SlideDirection")]
        [Category("GcjControl")]
        public SlideDirection SlideDirection
        {
            get { return (SlideDirection)GetValue(_SlideDirection); }
            set { SetValue(_SlideDirection, value); }
        }
        #endregion SlideDirection

        #endregion Properties

    }
}
