using GcjDiff.DiffBuilder.Model;
using GcjDiff.Wpf.Controls;
using GcjUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using UserControl = System.Windows.Controls.UserControl;

namespace GcjUiCtrl.Control
{
    /// <summary>
    /// CustomizedSideBySideDiffViewerUserControl.xaml の相互作用ロジック
    /// </summary>
    public partial class CustomizedSideBySideDiffViewerUserControl : UserControl
    {
        public CustomizedSideBySideDiffViewerUserControl()
        {
            InitializeComponent();
            diffViewer.DragOver += DiffCtrl_DragOver;
            diffViewer.DragEnter += DiffCtrl_DragEnter;
            diffViewer.Drop += DiffCtrl_DropFile;
            this.Loaded += CustomizedSideBySideDiffViewerUserControl_Loaded;
        }

        //DiffPlex.Wpf.Controls.InternalLinesViewer? leftView=null
        System.Windows.Controls.Control? leftViewer = null;
        System.Windows.Controls.Control? rightViewer = null;

        //ScrollViewer? leftNumberScrollViewer = null;
        //ScrollViewer? leftOperationScrollViewer = null;
        //ScrollViewer? leftValueScrollViewer = null;

        //ScrollViewer? rightNumberScrollViewer = null;
        //ScrollViewer? rightOperationScrollViewer = null;
        //ScrollViewer? rightValueScrollViewer = null;

        private void CustomizedSideBySideDiffViewerUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"{string.Format("{0:d5}", ++GcjConst.nDbgSequence)}: private void CustomizedSideBySideDiffViewerUserControl_Loaded(object sender, RoutedEventArgs e)\r\n Triggered(" + sender?.GetType()?.Name + " <" + (sender as FrameworkElement)?.Name + ">)");

            //leftViewer = Template?.FindName("LeftContentPanel", this) as System.Windows.Controls.Control;
            //rightViewer = Template?.FindName("RightContentPanel", this) as System.Windows.Controls.Control;

            List <UserControl> userCtrls = DepUtil.GetAllChildrenOfType<UserControl>(this);
            leftViewer = userCtrls.Where(x => x.Name == "LeftContentPanel").FirstOrDefault();
            rightViewer = userCtrls.Where(x => x.Name == "RightContentPanel").FirstOrDefault();

            //Debug.Assert(leftViewer != null && rightViewer != null);
            //if(leftViewer!=null && rightViewer != null)
            //{
            //    List<ScrollViewer> listScrollViews = DepUtil.GetAllChildrenOfType<ScrollViewer>(leftViewer);
            //    leftNumberScrollViewer = listScrollViews.Where(x => x.Name == "NumberScrollViewer").FirstOrDefault();
            //    leftOperationScrollViewer = listScrollViews.Where(x => x.Name == "OperationScrollViewer").FirstOrDefault();
            //    leftValueScrollViewer = listScrollViews.Where(x => x.Name == "ValueScrollViewer").FirstOrDefault();
            //    if (leftNumberScrollViewer != null && leftOperationScrollViewer !=null && leftValueScrollViewer!=null)
            //    {
            //        System.Windows.Controls.ScrollContentPresenter? scPresenter = DepUtil.GetFirstChildrenOfType<System.Windows.Controls.ScrollContentPresenter>(leftNumberScrollViewer);
            //        //scPresenter.VirtualizingStackPanel.IsVirtualizing = true;
            //        //scPresenter.vi = true;
            //    }
            //    List<ScrollViewer> rightScrollViews = DepUtil.GetAllChildrenOfType<ScrollViewer>(rightViewer);
            //    rightNumberScrollViewer = rightScrollViews.Where(x => x.Name == "NumberScrollViewer").FirstOrDefault();
            //    rightOperationScrollViewer = rightScrollViews.Where(x => x.Name == "OperationScrollViewer").FirstOrDefault();
            //    rightValueScrollViewer = rightScrollViews.Where(x => x.Name == "ValueScrollViewer").FirstOrDefault();
            //}
        }

        private void DiffCtrl_DropFile(object sender, DragEventArgs e)
        {
            if (diffViewer.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Get the file paths from the dropped data
                RaiseOnFileDropped(e);
            }
        }

        private void DiffCtrl_DragEnter(object sender, DragEventArgs e)
        {
            if (diffViewer.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
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

        private void DiffCtrl_DragOver(object sender, DragEventArgs e)
        {
            if (diffViewer.AllowDrop == true && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        #region DiffModel

        public static readonly DependencyProperty DiffModelProperty = 
            DependencyProperty.Register("DiffModel", typeof(SideBySideDiffModel), typeof(CustomizedSideBySideDiffViewerUserControl), new FrameworkPropertyMetadata(null, null));

        [Bindable(true)]
        [Category("Appearance")]
        public SideBySideDiffModel DiffModel
        {
            get
            {
                return (SideBySideDiffModel)GetValue(DiffModelProperty);
            }
            set
            {
                SetValue(DiffModelProperty, value);
            }
        }

        #endregion DiffModel

        #region OnFileDropped Event
        // 注册一个路由事件
        public static readonly RoutedEvent OnFileDroppedEvent = EventManager.RegisterRoutedEvent(
            "OnFileDropped", // 事件名称
            RoutingStrategy.Bubble, // 路由策略：冒泡、隧道、直接
            typeof(GcjDragEventHandler), // 事件处理程序的类型
            typeof(CustomizedButton)); // 注册事件的类

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

        #region Properties
        #region GcjVer         // 定义 GcjVer 依赖属性
        public static readonly DependencyProperty _GcjVer = DependencyProperty.Register(
            "GcjVer",
            typeof(string),
            typeof(CustomizedSideBySideDiffViewerUserControl),
            new PropertyMetadata("Gcj CheckBox v1.0"));


        [Description("Gets or sets the CustomizedSideBySideDiffViewerUserControl Version")]
        [Category("GcjControl")]
        public string GcjVer
        {
            get { return (string)GetValue(_GcjVer); }
            set { SetValue(_GcjVer, value); }
        }
        #endregion GcjVer

        #endregion Properties

        #region Methods
        public void Refresh()
        {
            diffViewer.Refresh();
        }

        public bool GoTo(int lineIndex, bool isLeftLine = false)
        {
            return diffViewer.GoTo(lineIndex, isLeftLine);
        }

        public bool GoTo(DiffPiece line, bool isLeftLine = false)
        {
            return diffViewer.GoTo(line, isLeftLine);
        }

        public DiffPiece GetLine(int lineIndex, bool isLeftLine = false)
        {
            return diffViewer.GetLine(lineIndex, isLeftLine);
        }

        public IEnumerable<DiffPiece> GetLinesInViewport(bool isLeftLine = false, VisibilityLevels level = VisibilityLevels.Any)
        {
            return diffViewer.GetLinesInViewport(isLeftLine, level);
        }

        public IEnumerable<DiffPiece> GetLinesInViewport(VisibilityLevels level)
        {
            return diffViewer.GetLinesInViewport(level);
        }

        public IEnumerable<DiffPiece> GetLinesBeforeViewport(bool isLeftLine = false, VisibilityLevels level = VisibilityLevels.Any)
        {
            return diffViewer.GetLinesBeforeViewport(isLeftLine, level);
        }

        public IEnumerable<DiffPiece> GetLinesBeforeViewport(VisibilityLevels level)
        {
            return diffViewer.GetLinesBeforeViewport(level);
        }

        public IEnumerable<DiffPiece> GetLinesAfterViewport(bool isLeftLine = false, VisibilityLevels level = VisibilityLevels.Any)
        {
            return diffViewer.GetLinesAfterViewport(isLeftLine, level);
        }

        public IEnumerable<DiffPiece> GetLinesAfterViewport(VisibilityLevels level)
        {
            return diffViewer.GetLinesAfterViewport(level);
        }

        public void CollapseUnchangedSections(int? contextLineCount = null)
        {
            diffViewer.CollapseUnchangedSections(contextLineCount);
        }

        public void ExpandUnchangedSections()
        {
            diffViewer.ExpandUnchangedSections();
        }

        #endregion Methods

    }
}
