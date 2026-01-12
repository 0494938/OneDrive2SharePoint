using GcjUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Threading;
using Brushes = System.Windows.Media.Brushes;


namespace GcjUiCtrl.Control
{

    public partial class CustomizedScrollTextBlock : System.Windows.Controls.ScrollViewer
    {
        public CustomizedScrollTextBlock() : base()
        {
            //DefaultStyleKey = typeof(CustomizedScrollTextBlock);
            Loaded += CustomizedScrollTextBlock_Loaded;
            Unloaded += CustomizedScrollTextBlock_Unloaded;
        }

        private void manangeSizeChangeEventForParents()
        {
            FrameworkElement? parent = null;
            lock (dictSizeChangeHooked)
            {
                ScrollViewer? parentScrollView = DepUtil.FindParent<ScrollViewer>(this.Parent);
                if(parentScrollView != null)
                    parent = parentScrollView;
                else
                    parent = this.Parent as FrameworkElement;
                
                if (parent != null)
                {
                    if (dictSizeChangeHooked.ContainsKey(parent))
                        dictSizeChangeHooked[parent] = dictSizeChangeHooked[parent] + 1;
                    else
                        dictSizeChangeHooked[parent] = 1;
                    if (dictSizeChangeHooked[parent] == 1)
                    {
                        parent.SizeChanged += AnyParentScrollView_SizeChanged;
                    }
                }
                if (this.Content is TextBlock childCtrl && childCtrl.Foreground == Brushes.Transparent)
                {
                    childCtrl.Foreground = BaseUtil.CtrlUtil.GetWhiteBlackForeGroundBrush(childCtrl.Background);
                }
                else if (Content is System.Windows.Controls.Control childCtrl1 && childCtrl1.Foreground == Brushes.Transparent)
                {
                    childCtrl1.Foreground = BaseUtil.CtrlUtil.GetWhiteBlackForeGroundBrush(childCtrl1.Background);
                }
                if (parent != null)
                {
                    this.Width = parent.ActualWidth;
                    StartScrolling(this, dictSizeChangeHooked[parent]);
                    if (AnimationDirection == OrientationType.Vertical && (this.Content as FrameworkElement)!=null)
                    {
                        AdjustScrollableHeight();
                    }
                }
            }
        }

        private void AdjustScrollableHeight()
        {
            System.Windows.Controls.ScrollContentPresenter? presenter = DepUtil.FindParent<System.Windows.Controls.ScrollContentPresenter>(this.Content as FrameworkElement);

            FrameworkElement? fwElem = this.Content as FrameworkElement;
            if (fwElem != null)
            {
                fwElem.Height = fwElem.ActualHeight;
            }
        }

        private void AnyParentScrollView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}:CustomizedScrollTextBlock::Parent_SizeChanged Triggered");

            FrameworkElement? scrollViewerParent = DepUtil.FindParent<ScrollViewer>(this.Parent);

            if (AnimationDirection == OrientationType.Horizontal)
            {
                if (scrollViewerParent != null)
                    this.Width = (double)scrollViewerParent.ActualWidth;
                else if (this.Parent is FrameworkElement fwElem)
                    this.Width = (double)fwElem.ActualWidth;
            }
            else if (AnimationDirection == OrientationType.Vertical)
            {
                if (AnimationDirection == OrientationType.Vertical && (this.Content as FrameworkElement) != null)
                {
                    AdjustScrollableHeight();
                }
            }

            if (this.Content is TextBlock childCtrl && childCtrl.Foreground == Brushes.Transparent)
            {
                childCtrl.Foreground = BaseUtil.CtrlUtil.GetWhiteBlackForeGroundBrush(childCtrl.Background);
            }
            else if (Content is System.Windows.Controls.Control childCtrl1 && childCtrl1.Foreground == Brushes.Transparent)
            {
                childCtrl1.Foreground = BaseUtil.CtrlUtil.GetWhiteBlackForeGroundBrush(childCtrl1.Background);
            }
        }


        private void StartScrolling(FrameworkElement? ctrl, int index)
        {
            if(AnimationDirection == OrientationType.Horizontal)
                StartHorizentalScrolling(index);
            else if (AnimationDirection == OrientationType.Vertical)
                StartVerticalScrolling(index);

        }

        private void StopScrolling()
        {
            if (timer != null) {
                timer.Stop();
                timer = null;
            }
        }

        DispatcherTimer? timer = null;
        private void StartHorizentalScrolling(int index)
        {
          
            if(timer == null)
            {
                timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(100 + index * 50)  // 每行速度不同
                };
            }

            double currentHorizontalOffset = 0;
            timer.Tick += (sender, args) =>
            {
                currentHorizontalOffset += 2;  // 控制每次滚动的像素
                if (currentHorizontalOffset >= this.ScrollableWidth)
                {
                    currentHorizontalOffset = 0;  // 如果到达右边，重置到左边
                }
                this.ScrollToHorizontalOffset(currentHorizontalOffset);
            };
            timer.Start();
        }

        private void StartVerticalScrolling(int index)
        {
            if (timer == null)
            {
                timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(100 + index * 50)  // 每行速度不同
                };
            }

            double currentVerticalOffset = 0;
            timer.Tick += (sender, args) =>
            {
                double nChildHeight = (this.Content as FrameworkElement)?.ActualHeight??double.NaN;
                currentVerticalOffset += 2;  // 控制每次滚动的像素
                if (currentVerticalOffset >= nChildHeight)
                {
                    currentVerticalOffset = 0;  // 如果到达右边，重置到左边
                }
                this.ScrollToVerticalOffset(currentVerticalOffset);
            };
            timer.Start();
        }

        private void CustomizedScrollTextBlock_Unloaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement? parent = null;
            lock (dictSizeChangeHooked)
            {
                FrameworkElement? scrollViewerParent = DepUtil.FindParent<ScrollViewer>(this.Parent);
                if (scrollViewerParent != null)
                    parent= scrollViewerParent;
                else if (this.Parent is FrameworkElement fwElem)
                    parent=fwElem;

                if (dictSizeChangeHooked.ContainsKey(parent))
                    dictSizeChangeHooked[parent] = dictSizeChangeHooked[parent] - 1;
                else 
                    Debug.Assert(false);
            }
            if(parent!=null)
                Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}:CustomizedScrollTextBlock_Unloaded ({dictSizeChangeHooked[parent]})" + (dictSizeChangeHooked[parent] ==0? "Parent_SizeChanged/ParentParent_SizeChanged":""));
            else
                Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}:CustomizedScrollTextBlock_Unloaded (NULL)");
        }

        static Dictionary<object, int> dictSizeChangeHooked = new Dictionary<object, int>();
        private void CustomizedScrollTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("$$$$ CustomizedScrollTextBlock_Loaded::(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
            //Debug.WriteLine("  Width=" + this.Width + ", Height=" + this.Height + ", ActualWidth=" + this.ActualWidth + ", ActualHeight=" + this.ActualHeight);
            manangeSizeChangeEventForParents();
        }

        #region Properties

        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedScrollTextBlock),
            new PropertyMetadata("Gcj ScrollTextBlock v1.0"));

        [Description("Gets or sets the CustomizedScrollTextBlock Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #region StartAnimation // 定义 StartAnimation 依赖属性
        public static readonly DependencyProperty StartAnimationProperty =
            DependencyProperty.Register("StartAnimation", typeof(bool), typeof(CustomizedScrollTextBlock), new PropertyMetadata(false));

        [Description("Gets or sets the CustomizedScrollTextBlock StartAnimation")]
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
                    lock (dictSizeChangeHooked)
                    {
                        StartScrolling(this, dictSizeChangeHooked[this.Parent]);
                    }
                }
                else
                {
                    StopScrolling();
                }
                SetValue(StartAnimationProperty, value);
            }
        }
        #endregion StartAnimation

        #region AnimationDirection // 定义 AnimationDirection 依赖属性
        public static readonly DependencyProperty AnimationDirectionProperty =
            DependencyProperty.Register("AnimationDirection", typeof(OrientationType), typeof(CustomizedScrollTextBlock), new PropertyMetadata(OrientationType.Horizontal));

        [Description("Gets or sets the CustomizedScrollTextBlock AnimationDirection")]
        [Category("GcjControl")]
        public OrientationType AnimationDirection
        {
            get
            {
                return (OrientationType)GetValue(AnimationDirectionProperty);
            }
            set
            {
                SetValue(AnimationDirectionProperty, value);
            }
        }
        #endregion AnimationDirection

        #endregion Properties
    }
}
